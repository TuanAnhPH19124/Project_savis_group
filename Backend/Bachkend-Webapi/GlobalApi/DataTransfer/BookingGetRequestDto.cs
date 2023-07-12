using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.DataTransfer
{
    public class BookingGetRequestDto
    {
        public string Id { get; set; }
        public double Price { get; set; }
        public string Photo { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Location { get; set; }
        public string RoomId { get; set; }
        public bool Selected { get; set; } = false;
    }
}