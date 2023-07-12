using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.Models
{
    public class Booking : BaseEntity
    {
        public string CustomerId { get; set; }
        public AppUser AppUser { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; }

        public int Amount { get; set; }
    }
}