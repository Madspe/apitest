using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.GeoJson; 

namespace SQUARE_API.Models
{
    public class IncidentMarker
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentMarkerId { get; set; } = string.Empty;
        public string incidentId { get; set; } = string.Empty;
        public string authorId { get; set; } = string.Empty;
        public DateTime created = DateTime.Now;
        public bool showInChat { get; set; }
        public bool dontShow { get; set; }
        public string message { get; set; } =string.Empty;
        public string markerUrl { get; set; } = string.Empty;
        public GeoPoint position { get;  } = new(0,0);
        public string type { get; set; } = string.Empty;
              
        
        public List<KeyValuePair<DateTime, List<KeyValuePair<string, string>>>> Changes { get; set; } = new(); //liste for endringer: Dato: (authorId, Endring)




    }
}