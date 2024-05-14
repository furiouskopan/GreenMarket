using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public int PhoneNumber { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public bool IsSeller { get; set; }

        // Navigation property for sales made by the user
        public virtual ICollection<Order> Sales { get; set; }

        //Navigation property to represent user's orders
        public virtual ICollection<Order> Orders { get; set; }
    }
}
