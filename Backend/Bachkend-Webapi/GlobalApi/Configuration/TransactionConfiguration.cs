using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalApi.Configuration
{
  public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
  {
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
      builder.HasKey(p => p.Id);
      builder.Property(p => p.CheckInDate).IsRequired();
      builder.Property(p => p.Total).IsRequired();
      builder.Property(p => p.PayMethod).IsRequired();
      builder.Property(p => p.Status).IsRequired();
      builder.Property(p => p.CustomerId).IsRequired();
        
        builder.HasOne(p => p.AppUser)
        .WithMany(p => p.Transactions)
        .HasForeignKey(p => p.CustomerId);
         
         builder.HasMany(p => p.TransactionDetails)
         .WithOne(p => p.Transaction)
         .HasForeignKey(p => p.TransactionId);
    }
  }
}