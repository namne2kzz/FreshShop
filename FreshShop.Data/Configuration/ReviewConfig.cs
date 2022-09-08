using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Review");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.ReplyID).IsRequired(false);

            builder.HasOne(x => x.Customer).WithMany(x => x.Reviews).HasForeignKey(x => x.CustomerID);

            builder.HasOne(x => x.Product).WithMany(x => x.Reviews).HasForeignKey(x => x.ProductID);

            builder.Property(x => x.Message).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.Status).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Reviews).HasForeignKey(x => x.UserId);

        }
    }
}
