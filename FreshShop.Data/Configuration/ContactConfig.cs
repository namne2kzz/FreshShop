﻿using FreshShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Configuration
{
    class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).UseIdentityColumn();                   

            builder.Property(x => x.Message).IsRequired();

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Contacts).HasForeignKey(x => x.UserId);

        }
    }

}
