using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalApi.Configuration
{
  public class BookingConfiguration : IEntityTypeConfiguration<Booking>
  {
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
      builder.HasKey(p => new {p.Id,p.CustomerId});
      builder.Property(p => p.Amount).IsRequired();
      builder.Property(p => p.CustomerId).IsRequired();
      builder.Property(p => p.RoomId).IsRequired();

      builder.HasOne(p => p.AppUser)
      .WithMany(p => p.Bookings)
      .HasForeignKey(p => p.CustomerId);

      builder.HasOne(p => p.Room)
      .WithMany(p => p.Bookings)
      .HasForeignKey(p => p.RoomId);

    }
  }
}