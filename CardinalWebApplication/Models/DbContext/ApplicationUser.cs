using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardinalLibrary;
using Microsoft.AspNetCore.Identity;

namespace CardinalWebApplication.Models.DbContext
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AccountGender Gender { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime? TermsAndConditionsDate { get; set; }
    }
}
