using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class WishlistConfig : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("Wishlist");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.HasOne(x => x.Customer).WithMany(x => x.Wishlists).HasForeignKey(x => x.CustomerID);

            builder.HasOne(x => x.Product).WithMany(x => x.Wishlists).HasForeignKey(x => x.ProductID);

            builder.Property(x => x.CreatedDate).IsRequired();

           

        }
    }
}
