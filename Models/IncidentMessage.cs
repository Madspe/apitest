using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class IncidentMessage
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentId { get; set; } = string.Empty;
        public string authorId { get; set; } = string.Empty;
        public DateTime created { get; set; } = DateTime.Now;
        public bool isHidden { get; set; } = false;
        public bool isImportant { get; set; } = false;
        public string message { get; set; } = string.Empty;
         // public List<KeyValuePair<DateTime, List<KeyValuePair<string, string>>>> changes { get; set; } = new(); //liste for endringer: Dato: (authorId, Endring)
    }
}