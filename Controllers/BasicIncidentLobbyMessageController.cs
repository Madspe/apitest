
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentLobbyMessageController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("IncidentLobbyMessage"); //velger riktig container
        
        
        // Metode for å lage ny IncidentLobbyMessage--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentLobbyMessageCreate")]
        public async Task IncidentLobbyMessageCreate(IncidentLobbyMessage incidentLobbyMessage){

           await container.UpsertItemAsync<IncidentLobbyMessage>(
                item: incidentLobbyMessage,
                partitionKey: new PartitionKey(incidentLobbyMessage.incidentLobbyId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentLobbyMessage gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentLobbyMessageGetById")]
        public async Task<IncidentLobbyMessage> IncidentLobbyMessageGetById(string id, string partitionKey){
            IncidentLobbyMessage response = await container.ReadItemAsync<IncidentLobbyMessage>(
                id : id,
                partitionKey: new PartitionKey(partitionKey)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentLobbyMessage gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentLobbyMessageUpdateById")]
        public async Task IncidentLobbyMessageUpdateById(string id, string partitionKey, IncidentLobbyMessage incidentLobbyMessage){

            await container.ReplaceItemAsync<IncidentLobbyMessage>(
                item : incidentLobbyMessage,
                id: id,
                partitionKey: new PartitionKey(partitionKey)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
