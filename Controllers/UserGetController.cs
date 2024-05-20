using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQUARE_API.Models;
using Microsoft.Azure.Cosmos;

namespace SQUARE_API.Controllers
{
    [ApiController]
    public class UserGetController : ControllerBase
    {
        
        DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection

        [HttpGet]
        [Route("/GetAllUsers")]
        public async Task<List<Models.User>> GetAllUsers(){
            Database db = db_conn.Database; // henter database fra db_conn
            Container container = db.GetContainer("User"); //velger riktig container
        
            QueryDefinition query = new("select * from User"); //definerer spørring

            using FeedIterator<Models.User> feedIterator = container.GetItemQueryIterator<Models.User>(query);

                List<Models.User> returnResponse= new(); 
                
                while (feedIterator.HasMoreResults){

                    FeedResponse<Models.User> response = await feedIterator.ReadNextAsync();
                    foreach (Models.User user in response){

                        returnResponse.Add(user);
                    }

            }
             return returnResponse;

        }


    }
}