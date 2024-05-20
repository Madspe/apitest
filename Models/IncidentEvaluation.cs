using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class IncidentEvaluation
    {
        public Guid id { get; set;} = Guid.Empty;
        public string incidentEvaluationId { get; set; } = string.Empty;
        public string incidentId { get; set; } = string.Empty;
        public string startedById { get; set; } = string.Empty;
        public string endedById { get; set; } = string.Empty;
        public DateTime started = DateTime.Now;
        public DateTime ended { get; set; }
        public List<string> notes { get; set; } = new();
    }
}