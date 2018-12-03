using System;
using Microsoft.AspNetCore.Identity;

namespace _101mngr.AuthorizationServer.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<long>
    {
        public string CountryCode { get; set; } 
        public DateTime BirthDate { get; set; } 
    }

    public class ApplicationRole : IdentityRole<long>
    {
        public ApplicationRole() { }

        public ApplicationRole(string name) : base(name) { }
    }
}