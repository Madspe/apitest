using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.GeoJson;

namespace SQUARE_API.Models
{
    public class IncidentPolyline
    {
        public Guid id { get; set;} = Guid.Empty;
        public string authorId { get; set; } = string.Empty;
        public string color { get; set; } = string.Empty;
        public bool isDashed { get; set; }
        public bool isLine { get; set; } //Skulle denn egt. ikke være med??
        public DateTime created = DateTime.Now;
        public bool showInChat { get; set; }
        public bool dontShow { get; set; }
        public string message { get; set; } =string.Empty;
        public List<GeoPoint> positions { get; } = new();
        public List<KeyValuePair<DateTime, List<KeyValuePair<string, string>>>> changes { get; set; } = new(); //liste for endringer: Dato: (authorId, Endring)
    }
}