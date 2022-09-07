using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class OrderDetailConfig : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderID);

            builder.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductID);

            builder.HasOne(x => x.Promotion).WithMany(x => x.OrderDetails).HasForeignKey(x => x.PromotionID);

            builder.Property(x => x.Quantity).IsRequired();

            builder.Property(x => x.Amount).IsRequired();

        }
    }
}
