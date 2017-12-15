using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace zadatak2.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public Guid GuidId { get; set; }

        public ApplicationUser()
        {
            GuidId = Guid.NewGuid();
        }
    }
}
