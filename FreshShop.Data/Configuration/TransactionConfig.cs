using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace FreshShop.Data.Configuration
{
    class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.TransactionDate).IsRequired();
          
            builder.Property(x => x.ExternalTransactionID).IsRequired();

            builder.Property(x => x.Amount).IsRequired();

            builder.Property(x => x.Fee).IsRequired();

            builder.Property(x => x.Result).IsRequired();

            builder.Property(x => x.Message).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Provider).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Transactions).HasForeignKey(x => x.UserId);           

        }
    }
}
