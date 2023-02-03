using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();         

            builder.Property(x => x.ShipName).IsRequired().HasMaxLength(255);

            builder.Property(x => x.ShipEmail).IsRequired().HasMaxLength(50).IsUnicode(false);

            builder.Property(x => x.ShipPhone).IsRequired().HasMaxLength(15);

            builder.Property(x => x.ShipAddress).IsRequired();

            builder.Property(x => x.ShippingCost).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(x => x.Total).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(x => x.Coupon).WithMany(x => x.Orders).HasForeignKey(x => x.CouponID);

            builder.Property(x => x.CreatedDate).IsRequired();      

            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

        }
    }
}
