using GeneralLabSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralLabSolutions.InfraStructure.Data
{
    public static class SeedDataCategoriaProduto
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context == null || context.CategoriaProduto == null)
                {
                    throw new ArgumentNullException("Null AppDbContext");
                }

                // Look for any categories.
                if (!context.CategoriaProduto.Any())
                {
                    context.CategoriaProduto.AddRange(
                        new CategoriaProduto("Eletrônicos"),
                        new CategoriaProduto("Instrumentos de Medição"),
                        new CategoriaProduto("Materiais de Construção"),
                        new CategoriaProduto("Ferramentas Manuais"),
                        new CategoriaProduto("Equipamentos de Proteção Individual (EPI)"),
                        new CategoriaProduto("Máquinas e Equipamentos"),
                        new CategoriaProduto("Automação Industrial"),
                        new CategoriaProduto("Peças e Componentes"),
                        new CategoriaProduto("Lubrificantes e Produtos Químicos"),
                        new CategoriaProduto("Materiais Elétricos"),
                        new CategoriaProduto("Iluminação"),
                        new CategoriaProduto("Hidráulica"),
                        new CategoriaProduto("Pneumática"),
                        new CategoriaProduto("Sinalização e Segurança"),
                        new CategoriaProduto("Móveis e Armazenagem")
                    );
                    context.SaveChanges();
                    Console.WriteLine("SeedData para Categoria de Produto gerado com sucesso!");
                } else
                {
                    Console.WriteLine("O SeedData para Categoria de Produto Já foi gerado!");
                }
            }
        }
    }
}
