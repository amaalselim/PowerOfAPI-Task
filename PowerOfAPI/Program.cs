
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PowerOf.Application.IServices;
using PowerOf.Application.Mapping;
using PowerOf.Application.Services;
using PowerOf.Core.Entities;
using PowerOf.Core.Enums;
using PowerOf.Core.IRepository;
using PowerOf.Core.IUnitOfWork;
using PowerOf.Persistence.Data;
using PowerOf.Persistence.Repository;
using PowerOf.Persistence.UnitOfWork;
using System.ComponentModel;
using System.Text;

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

            var smtpSettings = builder.Configuration.GetSection("SMTP");
            builder.Services.AddSingleton<IEmailService>(new EmailService(
                smtpSettings["Server"],
                int.Parse(smtpSettings["Port"]),
                smtpSettings["User"],
                smtpSettings["Password"]
            ));


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<AuthService>();    
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IImageService,ImageService>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();    


            builder.Services.AddAutoMapper(typeof(MappingProfile));


            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("MyPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
                });
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Roles.Admin.ToString(), policy => policy.RequireRole(Roles.Admin.ToString()));
                options.AddPolicy(Roles.Client.ToString(), policy => policy.RequireRole(Roles.Client.ToString()));
                options.AddPolicy(Roles.Employee.ToString(), policy => policy.RequireRole(Roles.Employee.ToString()));
            });



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
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
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Laza API V1");

            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("MyPolicy");


            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
