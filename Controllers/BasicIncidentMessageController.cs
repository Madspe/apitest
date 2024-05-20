
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentMessageController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        
        
        // Metode for å lage ny IncidentMessage--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentMessageCreate")]
        public async Task IncidentMessageCreate(string incidentId, IncidentMessage incidentMessage){
            incidentMessage.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentMessages/-", incidentMessage)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentMessage gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentMessageGetById")]
       public async Task<IncidentMessage> IncidentMessageGetById(string incidentId, string incidentMessageId){

            List<IncidentMessage> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentMessage item in response.incidentMessages){
                if (item.id.ToString() == incidentMessageId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å oppdatere en IncidentMessage gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentMessageUpdateById")]

        //Trenger vi egt denne? Er det nødvendig å kunne redigere en message i chaten? Evt. må en slik endring lagres?
        public async Task IncidentMessageUpdateById(string incidentId, string incidentMessageId, string incidentArchive, IncidentMessage newIncidentMessage){

            List<IncidentMessage> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );

             foreach (IncidentMessage item in response.incidentMessages){
                if (item.id.ToString() == incidentMessageId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            int index = response.incidentMessages.IndexOf(returnResponse);
            Guid id = returnResponse.id;

            //adder gammel incidentMarker til IncidentArchive
            await containerIA.PatchItemAsync<IncidentMessage>(
                id : incidentArchive,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/changedMessages/-", returnResponse)
                ]
            );
            //Replacer gammel incidentMarker med ny
             newIncidentMessage.id = id;
            await containerI.PatchItemAsync<IncidentMessage>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Replace($"/incidentMessages/{index}", newIncidentMessage)
                ]
                );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
