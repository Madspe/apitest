
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using SQUARE_API.Models;

namespace SQUARE_API.Controllers
{

    [ApiController]
    public class BasicIncidentPhotoController : ControllerBase
    {
         // Database connection og tilkobling til riktig container:
        static DB_Connection db_conn = new DB_Connection(); //lager ny instanse av DB_connection
        static Database db = db_conn.Database; // henter database fra db_conn
        static Container containerI = db.GetContainer("Incident"); //velger riktig container
        static Container containerIA = db.GetContainer("IncidentArchive");
        
        
        // Metode for å lage ny IncidentPhoto--------------------------------------------------------------------------->
        [HttpPost]
        [Route("/IncidentPhotoCreate")]
        public async Task IncidentPhotoCreate(string incidentId, IncidentPhoto incidentPhoto){
            incidentPhoto.id = Guid.NewGuid(); //autogenererer ID for objektet

           await containerI.PatchItemAsync<Group>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/incidentPhotos/-", incidentPhoto)
                ]
                );
            
        }
        //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentPhoto gjennom id --------------------------------------------------------------------------->

        [HttpGet]
        [Route("/IncidentPhotoGetById")]
       public async Task<IncidentPhoto> IncidentPhotoGetById(string incidentId, string incidentPhotoId){

            List<IncidentPhoto> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );
            //looper gjennom alle groupAccessRequests i gruppen og finner den med riktig id:
            foreach (IncidentPhoto item in response.incidentPhotos){
                if (item.id.ToString() == incidentPhotoId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            return returnResponse;
        }
         //------------------------------------------------------------------------------------------------------------------|

        // Metode for å hente en IncidentPhoto gjennom id --------------------------------------------------------------------------->

        [HttpPost]
        [Route("/IncidentPhotoUpdateById")]
         public async Task IncidentPhotoUpdateById(string incidentId, string incidentPhotoId, string incidentArchive, IncidentPhoto newIncidentPhoto){

            List<IncidentPhoto> returnResponseList = new();
            //henter riktig gruppe:
            Incident response = await containerI.ReadItemAsync<Incident>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId) 
            );

             foreach (IncidentPhoto item in response.incidentPhotos){
                if (item.id.ToString() == incidentPhotoId){
                    returnResponseList.Add(item);
                    break;
                }
            }
            var returnResponse = returnResponseList[0];
            int index = response.incidentPhotos.IndexOf(returnResponse);
            Guid id = returnResponse.id;

            //adder gammel incidentMarker til IncidentArchive
            await containerIA.PatchItemAsync<IncidentPhoto>(
                id : incidentArchive,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Add("/changedPhotos/-", returnResponse)
                ]
            );
            //Replacer gammel med ny Photo
            newIncidentPhoto.id = id;
            await containerI.PatchItemAsync<IncidentPhoto>(
                id : incidentId,
                partitionKey: new PartitionKey(incidentId),
                patchOperations: [
                    PatchOperation.Replace ($"/incidentPhotos/{index}", newIncidentPhoto)
                ]
                );        
        }
         //------------------------------------------------------------------------------------------------------------------|
    }
}
