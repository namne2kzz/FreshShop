using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime Dob { get; set; }

        public string ImagePath { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
