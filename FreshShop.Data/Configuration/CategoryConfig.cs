using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();
           
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Image).IsRequired().HasMaxLength(255);

            builder.Property(x => x.ParentID).IsRequired(false);

            builder.Property(x => x.IsShowOnHome).IsRequired();


        }
    }
}
