using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class BlogConfig : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {

            builder.ToTable("Blog");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();

            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Image).IsRequired().HasMaxLength(255);

            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired().HasDefaultValue(DateTime.Now);

            builder.Property(x => x.Status).IsRequired();           

        }

    }
}

