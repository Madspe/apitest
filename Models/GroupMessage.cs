using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQUARE_API.Models
{
    public class GroupMessage
    {
        public Guid id { get; set;} = Guid.Empty;
        public DateTime Created = DateTime.Now;
        public string authorId { get; set; } = string.Empty;
        public string authorName { get; set; } = string.Empty; //trenger kanskje ikke ha dette med? Kan hente navn fra User via authorId (?)

        //Titel og innhold:
        public string title { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;

    }
}