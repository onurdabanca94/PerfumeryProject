using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PerfumeryProject.API.DTOs.Parfume;
using PerfumeryProject.Business.Abstraction;
using PerfumeryProject.Business.Concrete;
using PerfumeryProject.Core.Abstraction;
using PerfumeryProject.Core.Concrete;
using PerfumeryProject.Data.Domain;
using PerfumeryProject.Infrastructure.Context;
using PerfumeryProject.Infrastructure.Extensions;
using Serilog;

namespace PerfumeryProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddOData(options => options
                .AddRouteComponents("odata", GetEdmModel())
                .Select()
                .Filter()
                .OrderBy()
                .SetMaxTop(100)
                .Count()
                .Expand());

            // Add services to the container.
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IParfumService, ParfumService>();
            builder.Services.AddCors(opts => opts.AddDefaultPolicy(pol => pol.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<MainDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAutoMapper(typeof(Program));

            LoggingExtension.ConfigureLogging();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();
            app.UseCors();

            app.Run();
        }

        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<GetParfumeWithBrandDto>("Parfume");
            return builder.GetEdmModel();
        }
    }
}