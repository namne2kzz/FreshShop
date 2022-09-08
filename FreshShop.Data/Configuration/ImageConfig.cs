using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class ImageConfig : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.HasOne(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductID);

            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(255).IsUnicode(false);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.IsDefault).IsRequired();

        }
    }
}
