using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.DataTransfer
{
    public class TransactionDetailPostRequestDto
    {
        public string BookingId { get; set; }
        public string RoomId { get; set; }
        public int Amount { get; set; }
    }
}