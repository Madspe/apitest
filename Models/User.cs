using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;

namespace SQUARE_API.Models
{
    public class User
    {
        [Required]
        public Guid id { get; set; } = Guid.Empty;
        [Required]
        public string userId { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        [Required]
        public string lastName { get; set; } = string.Empty;
        [Required]
        public string phoneNr { get; set; } = string.Empty;
        [Required]
        public bool isAdmin { get; set; }
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public DateTime created = DateTime.Now;
        [Required]
        public string profilePictureUrl { get; set; } = string.Empty;
        public List<string> groupMembership {get ; set; } = new(); //liste for gruppetilhørlighet dersom en User kan være medlem i flere grupper

    }
}