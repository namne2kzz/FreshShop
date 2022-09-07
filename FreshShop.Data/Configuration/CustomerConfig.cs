using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Dob).IsRequired();

            builder.Property(x => x.Email).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(x => x.Phone).IsRequired().HasMaxLength(15);

            builder.Property(x => x.Image).IsRequired().HasMaxLength(255);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Status).IsRequired();

        }
    }
}
