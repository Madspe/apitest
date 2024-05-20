using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.GeoJson; 

namespace SQUARE_API.Models
{
    public class Incident
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentId { get; set; } = string.Empty;
        public string authorId { get; set; } = string.Empty;
        public string groupId { get; set; } = string.Empty;
        public DateTime created { get; set; }
        public DateTime endedTimeStamp {get; set; }
        public string evalState { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool isShared { get; set; } = false;
        public bool isTraining { get; set; } = false;
        public GeoPoint position { get;  } = new(0,0); //setter 0,0 som standard for å ikke få irriterende feilmelding ved runtime :)
        public string title { get; set; } = string.Empty;
        public string location { get; set; } = string.Empty;

        public List<IncidentMarker> incidentMarkers {get; set; } = new();
        public List<IncidentPhoto> incidentPhotos {get; set; } = new();
        public List<IncidentPolygon> incidentPolygons {get; set; } = new();
        public List<IncidentPolyline> incidentPolylines {get; set; } = new();
        public List<IncidentMessage> incidentMessages {get; set; } = new();

        public List<IncidentMember> incidentMembers {get; set; } = new();
        
        
    }
}