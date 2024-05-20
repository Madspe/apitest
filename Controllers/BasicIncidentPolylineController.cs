
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentPolylineController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        
        
        // Metode for å lage ny IncidentPolyline--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentPolylineCreate")]
        public async Task IncidentPolylineCreate(string incidentId, IncidentPolyline incidentPolyline){
            incidentPolyline.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<Group>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentPolylines/-", incidentPolyline)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentPolyline gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentPolylineGetById")]
       public async Task<IncidentPolyline> IncidentPolylineGetById(string incidentId, string incidentPolylineId){

            List<IncidentPolyline> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentPolyline item in response.incidentPolylines){
                if (item.id.ToString() == incidentPolylineId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentPolyline gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentPolylineUpdateById")]
        public async Task IncidentPolylineUpdateById(string incidentId, string incidentPolylineId, string incidentArchive, IncidentPolyline newIncidentPolyline){

            List<IncidentPolyline> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );

             foreach (IncidentPolyline item in response.incidentPolylines){
                if (item.id.ToString() == incidentPolylineId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            int index = response.incidentPolylines.IndexOf(returnResponse);
            Guid id = returnResponse.id;

            //adder gammel incidentMarker til IncidentArchive
            await containerIA.PatchItemAsync<IncidentPolyline>(
                id : incidentArchive,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/changedPolylines/-", returnResponse)
                ]
            );
            //Sletter gammel incidentMarker fra Incident
            newIncidentPolyline.id = id;
            await containerI.PatchItemAsync<IncidentPolyline>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Replace($"/incidentPolylines/{index}", newIncidentPolyline)
                ]
                );            
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
