using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IntelviaStoreAPI.Models
{
    public class UserData : IdentityUser
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }


        [Display(Name = "Profile Photo")]
        public byte[] ProfilePhoto { get; set; }
    }
}
