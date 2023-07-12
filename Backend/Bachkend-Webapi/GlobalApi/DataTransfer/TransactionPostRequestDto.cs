using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;

namespace GlobalApi.DataTransfer
{
  public class TransactionPostRequestDto
  {
    public DateTime CheckInDate { get; set; }
    public double Total { get; set; }
    public string PayMethod { get; set; }
    public string Status { get; set; }
    public string CustomerId { get; set; }
    public List<TransactionDetailPostRequestDto> TransactionDetails { get; set; }
  }
}