
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentLobbyController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("IncidentLobby"); //velger riktig container
        
        
        // Metode for å lage ny IncidentLobby--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentLobbyCreate")]
        public async Task IncidentLobbyCreate(IncidentLobby incidentLobby){

           await container.UpsertItemAsync<IncidentLobby>(
                item: incidentLobby,
                partitionKey: new PartitionKey(incidentLobby.incidentId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentLobby gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentLobbyGetById")]
        public async Task<IncidentLobby> IncidentLobbyGetById(string id, string partitionKey){
            IncidentLobby response = await container.ReadItemAsync<IncidentLobby>(
                id : id,
                partitionKey: new PartitionKey(partitionKey)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentLobby gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentLobbyUpdateById")]
        public async Task IncidentLobbyUpdateById(string id, string partitionKey, IncidentLobby incidentLobby){

            await container.ReplaceItemAsync<IncidentLobby>(
                item : incidentLobby,
                id: id,
                partitionKey: new PartitionKey(partitionKey)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
