using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntelviaStoreAPI.Models
{
    public class UserToRole
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}
