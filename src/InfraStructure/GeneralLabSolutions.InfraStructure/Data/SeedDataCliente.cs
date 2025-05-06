using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralLabSolutions.InfraStructure.Data
{
    public static class SeedDataCliente
    {
        // Função para retornar o tipo de cliente baseado em pesos
        public static TipoDeCliente GetTipoDeClienteByWeight(Random random)
        {
            var pesos = new Dictionary<TipoDeCliente, int>
            {
                { TipoDeCliente.Especial, 9 },   // Peso 9
                { TipoDeCliente.Comum, 32 },     // Peso 32
                { TipoDeCliente.Inadimplente, 4 } // Peso 4
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

            return TipoDeCliente.Comum;  // Fallback (segurança)
        }

        // Função para retornar o status do cliente baseado em pesos
        public static StatusDoCliente GetStatusDoClienteByWeight(Random random)
        {
            var pesos = new Dictionary<StatusDoCliente, int>
            {
                { StatusDoCliente.Ativo, 30 },  // Peso 30
                { StatusDoCliente.Inativo, 5 }  // Peso 5
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

            return StatusDoCliente.Ativo;  // Fallback (segurança)
        }

        // Função para retornar o tipo de pessoa baseado em pesos
        public static TipoDePessoa GetTipoDePessoaByWeight(Random random)
        {
            var pesos = new Dictionary<TipoDePessoa, int>
            {
                { TipoDePessoa.Fisica, 5 },  // Peso 5
                { TipoDePessoa.Juridica, 25 } // Peso 25
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

            return TipoDePessoa.Fisica;  // Fallback (segurança)
        }

        // Função para gerar o número de documento baseado no tipo de pessoa
        public static string GerarDocumento(TipoDePessoa tipoPessoa, Random random)
        {
            if (tipoPessoa == TipoDePessoa.Fisica)
            {
                return string.Join("", Enumerable.Range(0, 11).Select(_ => random.Next(0, 10).ToString())); // 11 dígitos para Pessoa Física
            } else
            {
                return string.Join("", Enumerable.Range(0, 14).Select(_ => random.Next(0, 10).ToString())); // 14 dígitos para Pessoa Jurídica
            }
        }

        // Função para gerar email único baseado no nome do cliente
        public static string GerarEmail(string nomeCliente, Random random)
        {
            var nomeTratado = nomeCliente.ToLower().Replace(" ", ".");
            var dominios = new List<string> { "empresa.com", "cliente.com", "mail.com", "exemplo.com" };
            var dominioAleatorio = dominios [random.Next(dominios.Count)];
            return $"{nomeTratado}@{dominioAleatorio}";
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context == null || context.Cliente == null || context.Pessoa == null)
                {
                    throw new ArgumentNullException("Null AppDbContext");
                }

                if (!context.Cliente.Any())
                {
                    var random = new Random();

                    var clientesData = new List<(string Nome, string Email)>
                    {
                        ("Ana Paula", "ana.paula@email.com"),
                        ("Empresa X Ltda", "empresax@email.com"),
                        ("Carlos Eduardo", "carlos.eduardo@email.com"),
                        ("Construtora ABC", "construtoraabc@email.com"),
                        ("Arnaldo Santiago", "arnaldo.santiago@email.com"),
                        ("Clarice Borges", "clarice.borges@email.com"),
                        ("ABC Med Inc", "abcmedinc@email.com"),
                        ("Start Cor", "startcor@email.com"),
                        ("Medical Group Ltda", "medicalgroupltda@email.com"),
                        ("Ricardo Amaral", "ricardo.amaral@email.com"),
                        ("Emp. Leonardo, Ass", "empleonardoass@email.com"),
                        ("Cooperchip CPES Ltda", "cooperchipcpesltda@email.com"),
                        ("Ana Beatriz Souza", "ana.beatriz.souza@email.com"),
                        ("Lumac Labs SA", "lumaclabssa@email.com"),
                        ("Ana Clara", "ana.clara@email.com")
                        // Adicione mais clientes conforme necessário...
                    };

                    foreach (var (Nome, Email) in clientesData)
                    {
                        var tipoPessoa = GetTipoDePessoaByWeight(random);
                        var documento = GerarDocumento(tipoPessoa, random);

                        var cliente = new Cliente(
                            nome: Nome,
                            documento: documento,
                            tipoDePessoa: tipoPessoa,
                            email: Email
                        )
                        {
                            TipoDeCliente = GetTipoDeClienteByWeight(random),
                            StatusDoCliente = GetStatusDoClienteByWeight(random)
                        };

                        context.Cliente.Add(cliente);
                    }

                    context.SaveChanges();

                    Console.WriteLine("SeedData para Cliente gerado com sucesso!");
                } else
                {
                    Console.WriteLine("O SeedData para Cliente já foi gerado!");
                }
            }
        }
    }
}
