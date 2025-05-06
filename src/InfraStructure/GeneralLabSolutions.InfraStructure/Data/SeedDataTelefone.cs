using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralLabSolutions.InfraStructure.Data
{
    public static class SeedDataTelefone
    {
        // Função para retornar o tipo de telefone baseado em pesos
        public static TipoDeTelefone GetTipoDeTelefoneByWeight(Random random)
        {
            // Dicionário com pesos para cada tipo de telefone
            var pesos = new Dictionary<TipoDeTelefone, int>
            {
                { TipoDeTelefone.Celular, 10 },  // Peso 10
                { TipoDeTelefone.Residencial, 5 },  // Peso 5
                { TipoDeTelefone.Comercial, 15 },  // Peso 15
                { TipoDeTelefone.Recado, 5 },  // Peso 5
                { TipoDeTelefone.Outro, 3 }  // Peso 3
            };

            int pesoTotal = pesos.Values.Sum(); // Soma dos pesos totais
            int randomValue = random.Next(0, pesoTotal); // Valor aleatório entre 0 e o peso total

            int acumulado = 0;
            // Itera sobre os pesos para determinar o tipo de telefone
            foreach (var entry in pesos)
            {
                acumulado += entry.Value;
                if (randomValue < acumulado)
                {
                    return entry.Key; // Retorna o tipo de telefone correspondente ao valor aleatório
                }
            }

            return TipoDeTelefone.Celular;  // Fallback (segurança) caso algo dê errado
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context == null || context.Telefone == null || context.Pessoa == null || context.PessoaTelefone == null)
                {
                    throw new ArgumentNullException("Null AppDbContext");
                }

                if (!context.Telefone.Any())
                {
                    var random = new Random();

                    // --- 1. Obter as Pessoas Existentes ---
                    var pessoas = new List<Pessoa>();
                    pessoas.AddRange(context.Cliente.Select(c => c.Pessoa));
                    pessoas.AddRange(context.Fornecedor.Select(f => f.Pessoa));
                    pessoas.AddRange(context.Vendedor.Select(v => v.Pessoa));
                    // ... (adicionar outras entidades que se relacionam com Pessoa, se houver) ...

                    // --- 2. Gerar Telefones Aleatórios ---
                    var telefones = new List<Telefone>();
                    for (int i = 0; i < 100; i++) // Gerar 100 telefones (ajuste conforme necessário)
                    {
                        var ddd = random.Next(11, 99).ToString();
                        var numero = random.Next(900000000, 999999999).ToString();
                        var tipoDeTelefone = GetTipoDeTelefoneByWeight(random);

                        telefones.Add(new Telefone(ddd, numero, tipoDeTelefone));
                    }
                    context.Telefone.AddRange(telefones);
                    context.SaveChanges();

                    // --- 3. Associar Pessoas e Telefones ---
                    for (int i = 0; i < telefones.Count; i++)
                    {
                        // Associar cada Telefone a uma Pessoa aleatória
                        var pessoaAleatoria = pessoas [random.Next(pessoas.Count)];
                        var pessoaTelefone = new PessoaTelefone
                        {
                            PessoaId = pessoaAleatoria.Id,
                            TelefoneId = telefones [i].Id
                        };
                        context.PessoaTelefone.Add(pessoaTelefone);
                    }

                    context.SaveChanges();

                    Console.WriteLine("SeedData para Telefone e PessoaTelefone gerados com sucesso!");
                } else
                {
                    Console.WriteLine("O SeedData para Telefone já foi gerado!");
                }
            }
        }
    }
}
