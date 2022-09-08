using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class CouponConfig : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("Coupon");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();         

            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);

            builder.Property(x => x.Discount).IsRequired();

            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.FromDate).IsRequired();

            builder.Property(x => x.ExpiredDate).IsRequired();

            builder.Property(x => x.Status).IsRequired();

        }
    }
}

