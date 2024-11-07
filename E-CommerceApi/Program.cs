
using E_CommerceApi.Data;
using E_CommerceApi.Models;
using E_CommerceApi.Repositery.InterfaceCategory;
using E_CommerceApi.Repositery;
using E_CommerceApi.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E_CommerceApi.Utility;
using Stripe;

namespace E_CommerceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(
option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
            builder.Services.AddIdentity<
                ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<ICategoryRepositery, CategoryRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
