using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SMS.Models;

// Add profile data for application users by adding properties to the SMSUser class
public class SMSUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}