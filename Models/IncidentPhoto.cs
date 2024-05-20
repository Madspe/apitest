using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core.GeoJson; 

namespace SQUARE_API.Models
{
    public class IncidentPhoto
    {
        public Guid id { get; set;} = Guid.Empty;
        public string authorId { get; set; } = string.Empty;
        public DateTime created { get; set; } = DateTime.Now;
        public bool dontShow { get; set; }
        public bool showInChat { get; set; }
        public GeoPoint position { get;  } = new(0,0);
        public string photoUrl { get; set; } = string.Empty;
        public List<KeyValuePair<DateTime, List<KeyValuePair<string, string>>>> changes { get; set; } = new(); //liste for endringer: Dato: (authorId, Endring)
    }
}