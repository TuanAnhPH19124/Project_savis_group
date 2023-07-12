using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalApi.Models
{
  public class Transaction : BaseEntity
  {
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime CheckInDate { get; set; }
    public double Total { get; set; }
    public string PayMethod { get; set; }
    public string Status { get; set; }

    public string CustomerId { get; set; }
    public AppUser AppUser { get; set; }

    public List<TransactionDetail> TransactionDetails { get; set; }
  }
}