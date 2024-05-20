
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    public class BasicIncidentController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        static IncidentArchive incidentArchive = new IncidentArchive();
        
        
        // Metode for å lage ny Incident--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentCreate")]
        public async Task IncidentCreate(Incident incident){
            incident.id = Guid.NewGuid();
            incident.incidentId = incident.id.ToString();

           await containerI.UpsertItemAsync<Incident>(
                item: incident,
                partitionKey: new PartitionKey(incident.incidentId)
                );

            incidentArchive.id = Guid.NewGuid();
            incidentArchive.incidentId = incident.id.ToString();

            await containerIA.UpsertItemAsync<IncidentArchive>(
                item: incidentArchive,
                partitionKey: new PartitionKey(incidentArchive.incidentId)
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en Incident gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentGetById")]
        public async Task<Incident> IncidentGetById(string id){
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : id,
                partitionKey: new PartitionKey(id)
            );
            return response;


        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en Incident gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentUpdateById")]
        public async Task IncidentUpdateById(string id, Incident incident){

            await containerI.ReplaceItemAsync<Incident>(
                item : incident,
                id: id,
                partitionKey: new PartitionKey(id)
            );

        }
         //------------------------------------------------------------------------------------------------------------------|


// Metode for å hente alle Incidents --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentGetAll")]
        public async Task<List<Incident>> GetAllIncidents(){


        string q = "SELECT * from Incident";
      
       
            QueryDefinition query = new(q); //definerer spørring               
            using FeedIterator<Incident> feedIterator = containerI.GetItemQueryIterator<Incident>(query);

                List<Incident> returnResponse= new(); 
                while (feedIterator.HasMoreResults){

                    FeedResponse<Incident> response = await feedIterator.ReadNextAsync();
                    foreach (Incident incident in response){
                        returnResponse.Add(incident);
                    }
                }
             return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        
    }
}
