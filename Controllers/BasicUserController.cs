using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;


namespace SQUARE_API.Controllers
{
    [ApiController]
    public class BasicUserController : ControllerBase
    {
        // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("User"); //velger riktig container
        
        
        // Metode for å lage ny User--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/UserCreate")]
        public async Task UserCreate(Models.User user){
            user.id = Guid.NewGuid();
            user.userId = user.id.ToString();

            await container.UpsertItemAsync<Models.User>(
                item : user,
                partitionKey: new PartitionKey(user.userId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en User gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/UserGetById")]
        public async Task<Models.User> UserGetById(string id){
            Models.User response = await container.ReadItemAsync<Models.User>(
                id : id,
                partitionKey: new PartitionKey(id)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en User gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/UserUpdateById")]
        public async Task UserUpdateById(string id, Models.User user){

            await container.ReplaceItemAsync<Models.User>(
                item : user,
                id: id,
                partitionKey: new PartitionKey(id)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
