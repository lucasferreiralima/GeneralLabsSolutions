using System.Text.Json.Serialization;
using GeneralLabSolutions.Domain.Configurations;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.IoC;
using GeneralLabSolutions.WebAPI.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeneralLabSolutions.WebAPI
{
    public class Program
    {
        public static async Task Main(string [] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Configurações de Configuration
            var configuration = builder.Configuration;
            configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                configuration.AddUserSecrets<Program>();
            }


            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Registra o MediatR e escaneia assemblies para handlers
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblies(
                    // assembly do Core
                    typeof(MediatRExtensions).Assembly,
                    // assembly da WebAPI
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            builder.Services
                .AddServicesAndDepencencyInjections()
                .RegisterApiServices();


            // Política de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentPolicy", builder =>
                {
                    builder
                        .WithOrigins("https://localhost:7193")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            var app = builder.Build();

            app.UseCors("DevelopmentPolicy");

            // Chamada para o DbInitializer 
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await DbInitializer.InitializeAsync(services);
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
