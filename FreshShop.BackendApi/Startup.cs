using FluentValidation;
using FluentValidation.AspNetCore;
using FreshShop.Business.Catalog.Addresses;
using FreshShop.Business.Catalog.Blogs;
using FreshShop.Business.Catalog.Categories;
using FreshShop.Business.Catalog.Coupons;
using FreshShop.Business.Catalog.Products;
using FreshShop.Business.Catalog.Promotions;
using FreshShop.Business.Common;
using FreshShop.Business.System.Languages;
using FreshShop.Business.System.Roles;
using FreshShop.Business.System.Users;
using FreshShop.Data.EF;
using FreshShop.Data.Entities;
using FreshShop.ViewModels.Catalog.Address;
using FreshShop.ViewModels.Catalog.Blog;
using FreshShop.ViewModels.Catalog.Category;
using FreshShop.ViewModels.Catalog.Product;
using FreshShop.ViewModels.Catalog.Promotion;
using FreshShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshShop.BackendApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          

            services.AddDbContext<FreshShopDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("FreshShopSolutionDb")));
            
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<FreshShopDbContext>()
                .AddDefaultTokenProviders();

            //Declear DI                   
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IStorageService, FileStorageService>();
            services.AddTransient<IProductService, ProductService>();           
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IAddressService, AddressService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<ICouponService, CouponService>();

            //Fluent validator
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
            services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();
            services.AddTransient<IValidator<UserUpdateRequest>, UserUpdateRequestValidator>();

            services.AddTransient<IValidator<ProductCreateRequest>, ProductCreateRequestValidator>();
            services.AddTransient<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();

            services.AddTransient<IValidator<PromotionCreateRequest>, PromotionCreateRequestValidator>();
            services.AddTransient<IValidator<PromotionUpdateRequest>, PromotionUpdateRequestValidator>();

            services.AddTransient<IValidator<BlogCreateRequest>, BlogCreateRequestValidator>();
            services.AddTransient<IValidator<BlogUpdateRequest>, BlogUpdateRequestValidator>();

            services.AddTransient<IValidator<CategoryCreateRequest>, CategoryCreateRequestValidator>();
            services.AddTransient<IValidator<CategoryUpdateRequest>, CategoryUpdateRequestValidator>();

            services.AddTransient<IValidator<AddressCreateRequest>, AddressCreateRequestValidator>();
            services.AddTransient<IValidator<AddressUpdateRequest>, AddressUpdateRequestValidator>();

            services.AddControllers()
                .AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            services.AddSwaggerGen(c=>
                {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger FreshShop Solution", Version = "v1" });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description=@"JWT Authorization header using Bearer scheme. \r\n\r\n
                            Enter 'Bearer' [space] and thrn token in this text input below. \r\n\r\n
                            Example: 'Bearer 123456789abcdef'",
                        Name="Authorization",
                        In=ParameterLocation.Header,
                        Type=SecuritySchemeType.ApiKey,
                        Scheme="Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                },
                                Scheme="oauth2",
                                Name="Bearer",
                                In=ParameterLocation.Header,
                                
                            },
                            new List<string>()
                        }
                    });
            });

            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer=true,
                        ValidIssuer=issuer,
                        ValidateAudience=true,
                        ValidAudience=issuer,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        ClockSkew=System.TimeSpan.Zero,
                        IssuerSigningKey=new SymmetricSecurityKey(signingKeyBytes)
                    };
                });
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger FreshShop v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
