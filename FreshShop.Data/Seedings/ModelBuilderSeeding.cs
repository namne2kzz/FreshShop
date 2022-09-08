using FreshShop.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Data.Seedings
{
    public static class ModelBuilderSeeding
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "English", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    ID = 1,
                    Name="Loai 1",
                    IsShowOnHome = true,
                    ParentID = null,  
                    ImagePath="cate1.jpg"
                },
                 new Category()
                 {
                     ID = 2,
                     IsShowOnHome = true,
                     ParentID = null,                    
                    Name="Loai 2",
                    ImagePath="cate2.jpg"
                 });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                  new CategoryTranslation() { Id = 1, CategoryId = 1, Name = "Loai 1", LanguageId = "vi", SeoAlias = "loai-1",  SeoTitle = "San pham cho loai 1 ne" },
                  new CategoryTranslation() { Id = 2, CategoryId = 1, Name = "Category 1", LanguageId = "en", SeoAlias = "category-1",  SeoTitle = "Products for category 1" },
                  new CategoryTranslation() { Id = 3, CategoryId = 2, Name = "Loai 2", LanguageId = "vi", SeoAlias = "loai-2",  SeoTitle = "San pham cho loai 2 ne" },
                  new CategoryTranslation() { Id = 4, CategoryId = 2, Name = "Category 2", LanguageId = "en", SeoAlias = "category-2",  SeoTitle = "Products for category 2" }
                    );

            modelBuilder.Entity<Product>().HasData(
           new Product()
           {
               ID = 1,
               CategoryID=1,
               Name="San pham 1",
               Stock=10,
               Sold=0,
               Unit="kg",
               OriginalPrice=8000,
               Price=10000,
               ViewCount=0,
               CreatedDate=DateTime.Now,
               Status=true,
           });
            modelBuilder.Entity<ProductTranslation>().HasData(
                 new ProductTranslation()
                 {
                     Id = 1,
                     ProductId = 1,
                     Name = "San pham 1",
                     LanguageId = "vi",
                     SeoAlias = "san-pham-1",                    
                     SeoTitle = "San pham 1",                    
                     Description = "San pham 1"
                 },
                    new ProductTranslation()
                    {
                        Id = 2,
                        ProductId = 1,
                        Name = "Product 2",
                        LanguageId = "en",
                        SeoAlias = "product-2",                      
                        SeoTitle = "Product 2",                      
                        Description = "Product 2"
                    });
           

            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "tedu.international@gmail.com",
                NormalizedEmail = "tedu.international@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Abcd1234$"),
                SecurityStamp = string.Empty,
                Firstname = "Nam",
                Lastname = "Huynh Phuong",
                ImagePath="avatar.jpg",
                Dob = new DateTime(2000, 01, 01)
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

           
        }
    }
}
