
namespace SQUARE_API.Models
{
    public class IncidentArchive
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentId { get; set; } = string.Empty;
        public Incident incidentEnded{ get; } = new();
        public List<(DateTime, string, IncidentPolygon)> changedPolygons { get; set;} = new(); //dato når endringen skjedde / userId til den som endret/ objektet som ble endret
        public List<(DateTime, string, IncidentPolyline)> changedPolylines { get; set;} = new();
        public List<(DateTime, string, IncidentPhoto)> changedPhotos { get; set;} = new();
        public List<(DateTime, string, IncidentMessage)> changedMessages { get; set;} = new();
        public List<(DateTime, string, IncidentMarker)> changedMarkers { get; set;} = new();

        public List<(string, string)> evaluationNotes { get; set;} = new(); //kan for eksmpel være tema/ note

    }
}