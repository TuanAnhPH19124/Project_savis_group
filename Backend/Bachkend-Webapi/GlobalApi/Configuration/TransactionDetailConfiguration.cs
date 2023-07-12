using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalApi.Configuration
{
  public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
  {
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
      builder.HasKey(p => new {p.Id, p.TransactionId});
      builder.Property(p => p.Amount).IsRequired();
      builder.Property(p => p.Price).IsRequired();
      builder.Property(p => p.RoomId).IsRequired();


        builder.HasOne(p => p.Transaction)
        .WithMany(p => p.TransactionDetails)
        .HasForeignKey(p => p.TransactionId);
        
        builder.HasOne(p => p.Room)
        .WithMany(p => p.TransactionDetails)
        .HasForeignKey(p => p.RoomId);
    }
  }
}