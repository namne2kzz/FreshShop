using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class PromotionConfig : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable("Promotion");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.HasOne(x => x.Product).WithMany(x => x.Promotions).HasForeignKey(x => x.ProductID);         

            builder.Property(x => x.FromDate).IsRequired();

            builder.Property(x => x.ExpiredDate).IsRequired();

            builder.Property(x => x.Discount).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(x => x.Status).IsRequired();

        }
    }
}
