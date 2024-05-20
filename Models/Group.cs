using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SQUARE_API.Models
{
    public class Group
    {
        public Guid id { get; set;} = Guid.Empty;
        public string groupId { get; set; } = string.Empty;
        public DateTime created = DateTime.Now;
        public string groupName { get; set; } = string.Empty;

        public string logoUrl { get; set; } = string.Empty ;
        public List<string> members= new();
        //rapporter:
        public List<KeyValuePair<string, string>> reports { get; set; } = new(); //Tilhørlige rrapporter / hendelser innenfor gruppen :: Skal inneholde Navn på hendelse og tilhørende raport (link)

        //kontakt infro:
        public string contactName { get; set; } = string.Empty;
        public string contactEmail { get; set;} = string.Empty;
        public string contactPhone { get; set; } = string.Empty;
        public List<GroupMessage> groupMessages{ get; set; } = new List<GroupMessage>();
        public List<GroupAccessRequest> groupAccessRequests { get; set; } = new List<GroupAccessRequest>();
        

    }
}