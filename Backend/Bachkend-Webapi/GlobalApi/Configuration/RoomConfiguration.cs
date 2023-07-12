using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalApi.Configuration
{
  public class RoomConfiguration : IEntityTypeConfiguration<Room>
  {
    public void Configure(EntityTypeBuilder<Room> builder)
    {
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Name).IsRequired();
      builder.Property(p => p.City).IsRequired();
      builder.Property(p => p.District).IsRequired();
      builder.Property(p => p.Ward).IsRequired();
      builder.Property(p => p.AvailableUnits).IsRequired();
      builder.Property(p => p.Wifi).IsRequired();
      builder.Property(p => p.Laundry).IsRequired();
      builder.Property(p => p.Price).IsRequired();

        builder.HasMany(p => p.TransactionDetails)
        .WithOne(p => p.Room)
        .HasForeignKey(p => p.RoomId);

      builder.HasMany(p => p.Bookings)
      .WithOne(p => p.Room)
      .HasForeignKey(p => p.RoomId);
    }
  }
}