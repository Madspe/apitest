using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class IncidentLobbyMessage
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentLobbyMessageId { get; set; } = string.Empty;
        public string incidentLobbyId { get; set;} = string.Empty;
        public string authorId { get; set; } = string.Empty;
        public DateTime created { get; set; } = DateTime.Now;
        public string message { get; set; } = string.Empty;
        
    }
}