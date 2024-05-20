using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class GroupAccessRequest
    {
        public Guid id { get; set;} = Guid.Empty;
        public DateTime created = DateTime.Now; //+-
        public DateTime requested = DateTime.Now;
        public string requestedUserId { get; set; } = string.Empty; //hvem vil ha tilgang
        public string requestMessage { get; set; } = string.Empty;


        //accessData (data om håndtering av Requesten):
        public string handeledByUserId { get; set; } = string.Empty;
        public DateTime handeledDate { get; set; }
        public bool handeledStatus { get; set; } = false;
        public string requestStatus { get; set; } = string.Empty; //Kanskje lage en enum eller lignende for (denied/ pending/ accepted)

    }
}