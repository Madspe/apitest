
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicGroupController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("Group"); //velger riktig container
        
        
        // Metode for å lage ny Group--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/GroupCreate")]
        public async Task GroupCreate(Group group){
            group.id = Guid.NewGuid();
            group.groupId = group.id.ToString();

           await container.UpsertItemAsync<Group>(
                item: group,
                partitionKey: new PartitionKey(group.groupId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en Group gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/GroupGetById")]
        public async Task<Group> GroupGetById(string id){
            Group response = await container.ReadItemAsync<Group>(
                id : id,
                partitionKey: new PartitionKey(id)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en Group gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/GroupUpdateById")]
        public async Task GroupUpdateById(string id, Group group){

            await container.ReplaceItemAsync<Group>(
                item : group,
                id: id,
                partitionKey: new PartitionKey(id)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
