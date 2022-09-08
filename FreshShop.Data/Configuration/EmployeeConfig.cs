using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Dob).IsRequired();

            builder.Property(x => x.Email).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(x => x.Phone).IsRequired().HasMaxLength(15);

            builder.Property(x => x.Image).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Address).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Password).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Status).IsRequired();

        }
    }
}
