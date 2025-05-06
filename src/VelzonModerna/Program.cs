using GeneralLabSolutions.Identidade.Services;
using GeneralLabSolutions.InfraStructure.Data;
using GeneralLabSolutions.InfraStructure.IoC;
using Microsoft.EntityFrameworkCore;
using VelzonModerna.Configuration.Mappings;
using VelzonModerna.Extensions;
using VelzonModerna.Services;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddMvcConfiguration();

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

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddServicesAndDepencencyInjections()
            .AddAutoMapper(typeof(AutoMapperConfig));


        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", builder =>
                builder.WithOrigins("https://localhost:7015") // Permite requisições desse domínio
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });

        // Habilitar User Secrets no ambiente de desenvolvimento
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        // Registrar o HttpClient para consumo da API
        builder.Services.AddHttpClient<UserAdminService>();
        // Registrar o HttpClient para consumo da API
        builder.Services.AddHttpClient<RoleAdminService>();
         // Registrar o HttpClient para consumo da API
        builder.Services.AddHttpClient<AutenticacaoService>();
        builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
        //builder.Services.AddScoped<AuthenticationIdentityService>();

        var app = builder.Build();

        

        app.UseCors("AllowSpecificOrigin"); // Ativa a política de CORS

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            //pattern: "{controller=GalLabs}/{action=GlDashboard}/{id?}");
            pattern: "{controller=Authentication}/{action=Login}/{id?}");
        app.Run();

    }
}