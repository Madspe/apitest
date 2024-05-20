
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicGroupMessageController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("Group"); //velger riktig container
        
        
        // Metode for å lage ny GroupMessage--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/GroupMessageCreate")]
        public async Task GroupMessageCreate(string groupId, GroupMessage groupMessage){

           groupMessage.id = Guid.NewGuid(); //autogenererer ID for objektet

           await container.PatchItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId),
                patchOperations: [
                    PatchOperation.Add("/groupMessages/-", groupMessage)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en GroupMessage gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/GroupMessageGetById")]
        public async Task<GroupMessage> GroupMessageGetById(string groupId, string groupMessageId){

            List<GroupMessage> returnResponseList = new();
            //henter riktig gruppe:
            Group response = await container.ReadItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (GroupMessage item in response.groupMessages){
                if (item.id.ToString() == groupMessageId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en GroupMessage gjennom id --------------------------------------------------------------------------->

         private int index;
        [HttpPost]
        [Route("/GroupMessageUpdateById")]
        public async Task GroupMessageUpdateById(string groupId, string groupMessageId, GroupMessage groupMessage){
            Group response = await container.ReadItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id - så finner index til objektet i listen:
            foreach (GroupMessage item in response.groupMessages){
                if (item.id.ToString() == groupMessageId){
                    index = response.groupMessages.IndexOf(item);
                    break;
                }
            }
            //Erstatter det utvalgte objektet med nytt objekt
            await container.PatchItemAsync<Group>(
                id : groupId,
                partitionKey: new PartitionKey(groupId),
                patchOperations: [
                    PatchOperation.Replace($"/groupMessages/{index}", groupMessage)
                ]
                );
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
