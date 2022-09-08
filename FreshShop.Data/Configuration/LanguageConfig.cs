using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class LanguageConfig : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable("Language");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired().IsUnicode(false).HasMaxLength(5);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
