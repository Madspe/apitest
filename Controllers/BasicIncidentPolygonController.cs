
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentPolygonController : ControllerBase
    {
         // Database connection og tilkobling til riktig containerIP:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        
        
        // Metode for å lage ny IncidentPolygon--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentPolygonCreate")]
        public async Task IncidentPolygonCreate(string incidentId, IncidentPolygon incidentPolygon){
            incidentPolygon.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<Group>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentPolygons/-", incidentPolygon)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentPolygon gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentPolygonGetById")]
       public async Task<IncidentPolygon> IncidentPolygonGetById(string incidentId, string incidentPolygonId){

            List<IncidentPolygon> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentPolygon item in response.incidentPolygons){
                if (item.id.ToString() == incidentPolygonId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å oppdatere en IncidentPolygon gjennom id --------------------------------------------------------------------------->
        
        [HttpPost]
        [Route("/IncidentPolygonUpdateById")]
        public async Task IncidentPolygonUpdateById(string incidentId, string incidentPolygonId, string incidentArchive, IncidentPolygon newIncidentPolygon){

            List<IncidentPolygon> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );

             foreach (IncidentPolygon item in response.incidentPolygons){
                if (item.id.ToString() == incidentPolygonId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            int index = response.incidentPolygons.IndexOf(returnResponse);
            Guid id = returnResponse.id;

            //adder gammel incidentMarker til IncidentArchive
            await containerIA.PatchItemAsync<IncidentPolygon>(
                id : incidentArchive,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/changedPolygons/-", returnResponse)
                ]
            );
            //Replace
            newIncidentPolygon.id = id;   
            await containerI.PatchItemAsync<IncidentPolygon>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Replace($"/incidentPolygons/{index}", newIncidentPolygon)
                ]
                );
                 
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
