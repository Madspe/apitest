
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicGroupAccessRequestController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("Group"); //velger riktig container
        

        // Metode for å lage ny GroupAccessRequest--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/GroupAccessRequestCreate")]
        public async Task GroupAccessRequestCreate(string groupId, GroupAccessRequest groupAccessRequest){
            groupAccessRequest.id = Guid.NewGuid(); //autogenererer ID for objektet

            //PatchItemAsunc updater egentlig et item, men siden GroupAccessRequest er inni Group, må denne benyttes for å "lage en ny" GroupAccessRequest
           await container.PatchItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId),
                patchOperations: [
                    PatchOperation.Add("/groupAccessRequests/-", groupAccessRequest)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en GroupAccessRequest gjennom id --------------------------------------------------------------------------->
        
        [HttpGet]
        [Route("/GroupAccessRequestGetById")]
        public async Task<GroupAccessRequest> GroupAccessRequestGetById(string groupAccessRequestId, string groupId){

            List<GroupAccessRequest> returnResponseList = new();
            //henter riktig gruppe:
            Group response = await container.ReadItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (GroupAccessRequest item in response.groupAccessRequests){
                if (item.id.ToString() == groupAccessRequestId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å oppdatere en GroupAccessRequest gjennom id --------------------------------------------------------------------------->

        private int index;
        [HttpPost]
        [Route("/GroupAccessRequestUpdateById")]
        public async Task GroupAccessRequestUpdateById(string groupId, string groupAccessRequestId, GroupAccessRequest groupAccessRequest){
            Group response = await container.ReadItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id - så finner index til objektet i listen:
            foreach (GroupAccessRequest item in response.groupAccessRequests){
                if (item.id.ToString() == groupAccessRequestId){
                    index = response.groupAccessRequests.IndexOf(item);
                    break;
                }
            }
            //Erstatter det utvalgte objektet med nytt objekt
            await container.PatchItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId),
                patchOperations: [
                    PatchOperation.Replace($"/groupAccessRequests/{index}", groupAccessRequest)
                ]
                );
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
