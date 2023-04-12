using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");       

            builder.Property(x => x.Firstname).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Lastname).IsRequired().HasMaxLength(255);

            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(255).IsUnicode(false);

            builder.Property(x => x.Dob).IsRequired();       

        }


    }
}
