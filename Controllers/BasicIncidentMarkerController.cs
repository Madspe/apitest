
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentMarkerController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident");
        static Container containerIA = db.GetContainer("IncidentArchive");
        
        
        // Metode for å lage ny IncidentMarker--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentMarkerCreate")]
        public async Task IncidentMarkerCreate(string incidentId, IncidentMarker incidentMarker){
            incidentMarker.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<IncidentMarker>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentMarkers/-", incidentMarker)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentMarker gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentMarkerGetById")]
       public async Task<IncidentMarker> IncidentMarkerGetById(string incidentId, string incidentMarkerId){

            List<IncidentMarker> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentMarker item in response.incidentMarkers){
                if (item.id.ToString() == incidentMarkerId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentMarker gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentMarkerUpdateById")]
        public async Task IncidentMarkerUpdateById(string incidentId, string incidentMarkerId, string incidentArchive, IncidentMarker newIncidentMarker){

            List<IncidentMarker> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );

             foreach (IncidentMarker item in response.incidentMarkers){
                if (item.id.ToString() == incidentMarkerId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            int index = response.incidentMarkers.IndexOf(returnResponse);
            Guid id = returnResponse.id;

            //adder gammel incidentMarker til IncidentArchive
            await containerIA.PatchItemAsync<IncidentMarker>(
                id : incidentArchive,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/changedMarkers/-", returnResponse)
                ]
            );
            //Sletter gammel incidentMarker fra Incident
            newIncidentMarker.id = id;
            await containerI.PatchItemAsync<IncidentMarker>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Replace($"/incidentMarkers/{index}", newIncidentMarker)
                ]
            );          
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
