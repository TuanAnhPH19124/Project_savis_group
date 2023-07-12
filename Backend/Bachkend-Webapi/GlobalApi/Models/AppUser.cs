using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GlobalApi.Models
{
    public class AppUser : IdentityUser
    {
        public List<Transaction> Transactions { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}