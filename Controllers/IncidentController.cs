using Microsoft.AspNetCore.Mvc;
using SQUARE_API.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.AspNetCore.Cors;


namespace SQUARE_API.Controllers
{
    [ApiController]
    public class IncidentController : ControllerBase
    {
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        //string archiveId=string.Empty;


        // Metode for å hente alle Incidents ut i fra hvor mange du ønsker{limit} og hvilke gruppe de tilhører{groupName}. Sorteres nyeste først -------->

        [HttpGet]
        [Route("/GetIncidentByGroupNameLimitSortCreated/{limit}/{groupName}")]
        public async Task<List<Incident>> GetIncidentByGroupNameLimitSortCreated(int limit, string groupName){

        /*
        DETTE ER BARE KLADD
        test2
        string name = "Mads";
        string q = "SELECT * from Incident i OFFSET 0 LIMIT " + limit;
        string q2 = "SELECT * from Incident i orderby i.ended desc";
        string q3 = "SELECT * FROM Incident i ORDER BY i.ended DESC OFFSET 0 LIMIT {limit}";
        string q4 = "SELECT * FROM Incident i ORDER BY i.ended ASC OFFSET 0 LIMIT " + limit;
        string q5 = "SELECT * FROM Incident i ORDER BY i.ended ASC";
        string q6 = $"SELECT * FROM Incident i ORDER BY i.ended DESC OFFSET 0 LIMIT {limit}";
        string q7 = $"SELECT * FROM Incident i ORDER BY i.created DESC OFFSET 0 LIMIT {limit}";
        string q8 = "SELECT * FROM Incident i WHERE i.authorId = 'mads' ORDER BY i.created DESC OFFSET 0 LIMIT " + limit;
        string q9 = $"SELECT * FROM Incident i WHERE i.authorId = {name} ORDER BY i.created DESC OFFSET 0 LIMIT {limit}";
        string q10 = $"SELECT * FROM Incident i WHERE i.authorId = '{name}' ORDER BY i.created DESC OFFSET 0 LIMIT {limit}";
        */
        string q11 = $"SELECT * FROM Incident i WHERE i.groupId = '{groupName}' ORDER BY i.created ASC OFFSET 0 LIMIT {limit}";
    
            QueryDefinition query4 = new(q11); //definerer spørring               
            using FeedIterator<Incident> feedIterator = containerI.GetItemQueryIterator<Incident>(query4);

                List<Incident> returnResponse= new(); 
                while (feedIterator.HasMoreResults){

                    FeedResponse<Incident> response = await feedIterator.ReadNextAsync();
                    foreach (Incident incident in response){
                        returnResponse.Add(incident);
                    }
                }
             return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|



        [HttpPost]
        [Route("/IncidentEnd")]
        public async Task IncidentEnd(string incidentId, string archiveId){
            
            //setter "endedTimeStamp" til true
            await containerI.PatchItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Set("/endedTimeStamp", DateTime.Now)
                ]);

            //finner riktig Incident
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId)
            );
            //finner id'en til IncidentArchive som hører til Incident (funket ikke, gir opp):
           // QueryDefinition query = new QueryDefinition(query: "select * from IncidentArchive IA where IA.incidetId = @incidentId").WithParameter("@incidentId", incidentId); //definerer spørring

            //using FeedIterator<IncidentArchive> feedIterator = containerIA.GetItemQueryIterator<IncidentArchive>(query); 
                //while (feedIterator.HasMoreResults){

                   // FeedResponse<IncidentArchive> responseIA = await feedIterator.ReadNextAsync();
                    //foreach (IncidentArchive IA in responseIA){
                        //archiveId = IA.id.ToString();
                    //}

            //}

            //"Kopierer" aktive incidet objekter (som poligoner, photos, etc) over til IncidentArchive:
            
            // For Incident polygons
            foreach(IncidentPolygon item in response.incidentPolygons){
                await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Add("/changedPolygons/-", item)]
                    );   
            }
           
                    //sletter alle Polygons fra Incident:
                await containerI.PatchItemAsync<IncidentPolygon>(
                    id : incidentId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Remove("/incidentPolygons/0")]
                    );
            
                

                //For Incident Polylines---------------------------------------------------------------------------------------------------------------------------------------------------
            foreach(IncidentPolyline item in response.incidentPolylines){
                int index = response.incidentPolylines.IndexOf(item);
                await containerI.PatchItemAsync<IncidentPolyline>(
                    id : incidentId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Remove($"/incidentPolylines/{index}")]
                    );
                await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Add("/changedPolylines/-", item)]
                    );
                //sletter alle Polylines fra Incident:
                
            }

                //For Incident Photos------------------------------------------------------------------------------------------------------------------------------------------------------
            foreach(IncidentPhoto item in response.incidentPhotos){
                await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Add("/changedPhotos/-", item)]
                    );
                    
                int index = response.incidentPhotos.IndexOf(item);
                //sletter alle Photos fra Incident:
                await containerI.PatchItemAsync<IncidentPhoto>(
                    id : incidentId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Remove($"/incidentPhotos/{index}")]
                    );                
            }

                //For Incident Markers: ----------------------------------------------------------------------------------------------------------------------------------------------------
            foreach(IncidentMarker item in response.incidentMarkers){
                await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Add("/changedMarkers/-", item)]
                );
                int index = response.incidentMarkers.IndexOf(item);                   
                 //sletter alle Markers fra Incident:
                await containerI.PatchItemAsync<IncidentMarker>(
                    id : incidentId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Remove($"/incidentMarkers/{index}")]
                    );
            }

            //For Incident Messages: ----------------------------------------------------------------------------------------------------------------------------------------------------
            foreach(IncidentMessage item in response.incidentMessages){
                await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Add("/changedMessages/-", item)]
                    );
                int index = response.incidentMessages.IndexOf(item);
                await containerI.PatchItemAsync<IncidentMessage>(
                    id : incidentId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Remove($"/incidentMessages/{index}")]
                    );
            }
               

            //Legger til resten av incidenten i "incidentEnded" inni IncidentArchive:-----------------------------------------------!!
            await containerIA.PatchItemAsync<IncidentArchive>(
                    id : archiveId,
                    partitionKey: new PartitionKey(incidentId),
                    patchOperations: [
                    PatchOperation.Set("/incidentEnded", response)]
                    );

            //Sletter den avsluttede Incidenten fra Incident:
            await containerI.DeleteItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId)
                );
                

            }


            

        }


    }
