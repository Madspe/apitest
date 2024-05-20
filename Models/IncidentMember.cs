using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class IncidentMember
    {
        public Guid id { get; set;} = Guid.Empty;
        public string userId { get; set; } = string.Empty;
        public string groupId { get; set; } = string.Empty;
    }
}