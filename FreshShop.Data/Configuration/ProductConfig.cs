using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryID);           

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Description).IsRequired();

            builder.Property(x => x.Stock).IsRequired();

            builder.Property(x => x.Sold).IsRequired();

            builder.Property(x => x.Unit).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.ViewCount).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.Status).IsRequired();
        }
    }
}
