using GeneralLabSolutions.Identidade.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;

namespace GeneralLabSolutions.Identidade.Configuration
{
    public static class SeedDataUsersAndRoles
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); // Adiciona um logger

            var retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning(exception, $"Tentativa {retryCount} de popular o banco de dados após {timeSpan.TotalSeconds} segundos. Erro: {exception.Message}");
                });

            await retryPolicy.ExecuteAsync(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var scopedProvider = scope.ServiceProvider;
                await using var context = new ApplicationDbContext(scopedProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var seedUserSettings = configuration.GetSection(nameof(SeedUserSettings)).Get<SeedUserSettings>();

                // 1. Verificar se as tabelas existem
                if (!await context.Database.CanConnectAsync() || !context.Database.GetAppliedMigrations().Any())
                {
                    logger.LogInformation("Tabelas do Identity não encontradas. Execute as migrações primeiro.");
                    return; // Ou lançar uma exceção, dependendo da sua estratégia
                }

                // 2. Verificar se a população já foi feita (assumindo que se o role 'Admin' existir, os dados já foram populados)
                if (await roleManager.RoleExistsAsync("Admin"))
                {
                    logger.LogInformation("O banco de dados já foi populado anteriormente.");
                    return;
                }

                // 3. Criar Roles
                logger.LogInformation("Criando Roles...");
                await CriarRoleAsync(roleManager, "Admin", logger);
                await CriarRoleAsync(roleManager, "Default", logger);
                await CriarRoleAsync(roleManager, "SuperAdmin", logger);

                // 4. Criar Usuários
                logger.LogInformation("Criando Usuários...");
                await CriarUsuarioAsync(userManager, "Nome do Admin", "ApelidoAdmin", DateTime.Now.AddYears(-20), "imagemPadrao.png", seedUserSettings!.AdminEmail, seedUserSettings.AdminPassword, "Admin", logger);
                await CriarUsuarioAsync(userManager, "Nome do User Default", "UserDefault", DateTime.Now.AddYears(-20), "imagemPadrao.png", seedUserSettings!.DefaultUserEmail, seedUserSettings.DefaultUserPassword, "Default", logger);
                await CriarUsuarioAsync(userManager, "Nome do SuperAdmin", "SuperAdmin", DateTime.Now.AddYears(-20), "imagemPadrao.png", seedUserSettings!.SuperAdminEmail, seedUserSettings.SuperAdminPassword, "SuperAdmin", logger);

                await context.SaveChangesAsync();
                logger.LogInformation("População do banco de dados concluída com sucesso.");
            });
        }

        private static async Task CriarRoleAsync(RoleManager<IdentityRole> roleManager, string roleName, ILogger logger)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    logger.LogError($"Erro ao criar a role '{roleName}'. Erros: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    // Você pode lançar uma exceção aqui se a criação de um role for crítica
                } else
                {
                    logger.LogInformation($"Role '{roleName}' criada com sucesso.");
                }
            } else
            {
                logger.LogInformation($"Role '{roleName}' já existe.");
            }
        }

        private static async Task CriarUsuarioAsync(UserManager<ApplicationUser> userManager, string nomeCompleto, string apelido, DateTime dataNascimento, string imgProfilePath, string email, string password, string role, ILogger logger)
        {
            var user = await userManager.FindByEmailAsync(email); // Otimização: Buscar o usuário apenas uma vez
            if (user == null)
            {
                user = new ApplicationUser
                {
                    NomeCompleto = nomeCompleto,
                    Apelido = apelido,
                    DataNascimento = dataNascimento,
                    ImgProfilePath = imgProfilePath,
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    logger.LogInformation($"Usuário '{email}' criado e adicionado à role '{role}'.");
                } else
                {
                    logger.LogError($"Erro ao criar o usuário '{email}'. Erros: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    // Você pode lançar uma exceção aqui se a criação de um usuário for crítica
                }
            } else
            {
                logger.LogInformation($"Usuário '{email}' já existe.");
            }
        }
    }
}