using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class IncidentLobby //funker egt. som accessRequest for incidents.
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentLobbyId { get; set; } = string.Empty;
        public string incidentId { get; set; } = string.Empty;
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime requestTime { get; set; } = DateTime.Now;
        public string requestedUserId { get; set; } = string.Empty;
        public List<IncidentLobbyMessage> incidentLobbyMessages { get; set; } = new();
    }
}