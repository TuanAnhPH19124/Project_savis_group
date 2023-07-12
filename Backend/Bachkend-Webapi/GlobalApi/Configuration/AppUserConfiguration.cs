using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalApi.Configuration
{
  public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
  {
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
      builder.HasMany(p => p.Transactions)
      .WithOne(p => p.AppUser)
      .HasForeignKey(p => p.CustomerId);

      builder.HasMany(p => p.Bookings)
      .WithOne(p => p.AppUser)
      .HasForeignKey(p => p.CustomerId);

      
    }
  }
}