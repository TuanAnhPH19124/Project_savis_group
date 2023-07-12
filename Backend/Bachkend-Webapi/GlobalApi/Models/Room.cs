using GlobalApi.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalApi.Models;

[Table("Rooms")]
public class Room : BaseEntity
{

    public string Name { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string Ward { get; set; } = string.Empty;
 
    public int AvailableUnits { get; set; }

    public bool Wifi { get; set; }

    public bool Laundry { get; set; }
    
    public double Price { get; set; }
    public string Photo { get; set; }

    public List<TransactionDetail> TransactionDetails { get; set; }
    public List<Booking> Bookings { get; set; }
}
