
using GeneralLabSolutions.Identidade.Configuration;
using GeneralLabSolutions.Identidade.Services;
using GeneralLabSolutions.WebApiCore.Identidade;

namespace GeneralLabSolutions.Identidade
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Carregar configurações
            var configuration = builder.Configuration;
            configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (builder.Environment.IsDevelopment())
            {
                configuration.AddUserSecrets<Program>();
            }

            // Adicionar serviços
            builder.Services.AddIdentityConfiguration(configuration);

            builder.Services.AddApiConfiguration();

            builder.Services.AddSwaggerConfiguration();

            // JwtConfig
            builder.Services.AddJwtConfiguration(configuration);

            
            

            //builder.Services.AddScoped<IAuthenticationIdentityService, AuthenticationIdentityService>();
            builder.Services.AddScoped<AuthenticationIdentityService>();
            
           
            var allowedOrigins = "_totalAllowedOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(allowedOrigins,
                                      builder =>
                                      {
                                          //builder.WithOrigins("http://localhost:4503")
                                          builder.AllowAnyOrigin()
                                                 .AllowAnyHeader()
                                                 .AllowAnyMethod();
                                      });
            });

            var app = builder.Build();

            app.UseCors(allowedOrigins);

            // Create a service scope to get an AppDbContext instance using DI and seed the database.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedDataUsersAndRoles.InitializeAsync(services, configuration); // Aguardar a conclusão    
            }

            // Configuração do middleware
            app.UseSwaggerConfiguration();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

    }
}
