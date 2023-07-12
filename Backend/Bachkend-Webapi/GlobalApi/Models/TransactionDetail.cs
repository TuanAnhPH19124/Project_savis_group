using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.Models
{
    public class TransactionDetail : BaseEntity
    {
        public string TransactionId { get; set; }
        public Transaction Transaction { get; set; }

        public string RoomId { get; set; }
        public Room Room { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
        
    }
}