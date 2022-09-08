using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(x => x.ID);
            
            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.Province).IsRequired().HasMaxLength(255);

            builder.Property(x => x.District).IsRequired().HasMaxLength(255);

            builder.Property(x => x.AddressDetail).IsRequired();

            builder.HasOne(x => x.Customer).WithMany(x => x.Addresses).HasForeignKey(x => x.CustomerID);

            builder.HasOne(x => x.AppUser).WithMany(x => x.Addresses).HasForeignKey(x => x.UserId);

        }

       
    }
}
