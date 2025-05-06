using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GeneralLabSolutions.InfraStructure.Data
{
    public static class SeedDataContato
    {
        // Função para retornar o tipo de contato baseado em pesos
        public static TipoDeContato GetTipoDeContatoByWeight(Random random)
        {
            var pesos = new Dictionary<TipoDeContato, int>
            {
                { TipoDeContato.Inativo, 5 },  // Peso 5
                { TipoDeContato.Comercial, 35 },  // Peso 35
                { TipoDeContato.Pessoal, 10 },  // Peso 10
                { TipoDeContato.ProspeccaoCliente, 25 },  // Peso 25
                { TipoDeContato.ProspeccaoVendedor, 10 },  // Peso 10
                { TipoDeContato.ProspeccaoFornecedor, 15 }  // Peso 15
            };

            int pesoTotal = pesos.Values.Sum();
            int randomValue = random.Next(0, pesoTotal);

            int acumulado = 0;
            foreach (var entry in pesos)
            {
                acumulado += entry.Value;
                if (randomValue < acumulado)
                {
                    return entry.Key;
                }
            }

            return TipoDeContato.Comercial;  // Fallback para Comercial (segurança)
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context == null || context.Contato == null || context.Pessoa == null || context.PessoaContato == null)
                {
                    throw new ArgumentNullException("Null AppDbContext");
                }

                if (!context.Contato.Any())
                {
                    var random = new Random();

                    // --- 1. Obter as Pessoas Existentes ---
                    var pessoas = new List<Pessoa>();
                    pessoas.AddRange(context.Cliente.Select(c => c.Pessoa));
                    pessoas.AddRange(context.Fornecedor.Select(f => f.Pessoa));
                    pessoas.AddRange(context.Vendedor.Select(v => v.Pessoa));
                    // ... (adicionar outras entidades que se relacionam com Pessoa, se houver) ...

                    // --- 2. Criar Contatos e Associá-los às Pessoas ---
                    var contatos = new List<Contato>();
                    for (int i = 0; i < 34; i++) // Manteremos o mesmo número de Contatos
                    {
                        var nome = $"Contato {i}";
                        var email = $"contato{i}@empresa.com";
                        var telefone = $"({random.Next(11, 99)}) 9{random.Next(1000, 9999)}-{random.Next(1000, 9999)}";

                        var contato = new Contato(nome, email, telefone)
                        {
                            EmailAlternativo = random.Next(0, 2) == 0 ? $"contato{i}.alt@empresa.com" : string.Empty,
                            TelefoneAlternativo = random.Next(0, 2) == 0 ? $"({random.Next(11, 99)}) 9{random.Next(1000, 9999)}-{random.Next(1000, 9999)}" : string.Empty,
                            Observacao = i % 2 == 0 ? "Contato gerado automaticamente para testes" : string.Empty,
                            TipoDeContato = GetTipoDeContatoByWeight(random)
                        };
                        contatos.Add(contato);

                        // Associar o Contato a uma Pessoa aleatória
                        var pessoaAleatoria = pessoas [random.Next(pessoas.Count)];
                        var pessoaContato = new PessoaContato
                        {
                            PessoaId = pessoaAleatoria.Id,
                            Contato = contato
                        };
                        context.PessoaContato.Add(pessoaContato);
                    }

                    context.SaveChanges();

                    Console.WriteLine("SeedData para Contato e PessoaContato gerados com sucesso!");
                } else
                {
                    Console.WriteLine("O SeedData para Contato já foi gerado!");
                }
            }
        }
    }
}