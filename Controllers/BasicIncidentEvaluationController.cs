
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentEvaluationController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container container = db.GetContainer("IncidentEvaluation"); //velger riktig container
        
        
        // Metode for å lage ny IncidentEvaluation--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentEvaluationCreate")]
        public async Task IncidentEvaluationCreate(IncidentEvaluation incidentEvaluation){

           await container.UpsertItemAsync<IncidentEvaluation>(
                item: incidentEvaluation,
                partitionKey: new PartitionKey(incidentEvaluation.incidentId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentEvaluation gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentEvaluationGetById")]
        public async Task<IncidentEvaluation> IncidentEvaluationGetById(string id, string partitionKey){
            IncidentEvaluation response = await container.ReadItemAsync<IncidentEvaluation>(
                id : id,
                partitionKey: new PartitionKey(partitionKey)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentEvaluation gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentEvaluationUpdateById")]
        public async Task IncidentEvaluationUpdateById(string id, string partitionKey, IncidentEvaluation incidentEvaluation){

            await container.ReplaceItemAsync<IncidentEvaluation>(
                item : incidentEvaluation,
                id: id,
                partitionKey: new PartitionKey(partitionKey)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
