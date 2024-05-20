
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentMemberController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        
        
        // Metode for å lage ny IncidentMember--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentMemberCreate")]
        public async Task IncidentMemberCreate(string incidentId, IncidentMember incidentMember){
            incidentMember.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<Group>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentMembers/-", incidentMember)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentMember gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentMemberGetById")]
       public async Task<IncidentMember> IncidentMemberGetById(string incidentId, string incidentMemberId){

            List<IncidentMember> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentMember item in response.incidentMembers){
                if (item.id.ToString() == incidentMemberId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentMember gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentMemberUpdateById")]
        public async Task IncidentMemberUpdateById(string id, string partitionKey, IncidentMember incidentMember){

            await containerI.ReplaceItemAsync<IncidentMember>(
                item : incidentMember,
                id: id,
                partitionKey: new PartitionKey(partitionKey)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
