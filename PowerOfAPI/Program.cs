
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PowerOf.Core.Entities;
using PowerOf.Core.IRepository;
using PowerOf.Core.IUnitOfWork;
using PowerOf.Persistence.Data;
using PowerOf.Persistence.Repository;
using PowerOf.Persistence.UnitOfWork;

namespace PowerOfAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("cs")));

            builder.Services.AddIdentity<User, IdentityRole>()
                           .AddEntityFrameworkStores<ApplicationDbContext>()
                           .AddDefaultTokenProviders();

            builder.Services.AddScoped<IGenericRepository<Product>, ProductRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                });
            });




            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            /*-----------------------------Swagger Part-----------------------------*/
            #region Swagger REgion

            // إضافة خدمات Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Laza API", Version = "v1" });

                // Add support for bearer token authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer' followed by a space and the token.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyPolicy");


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
