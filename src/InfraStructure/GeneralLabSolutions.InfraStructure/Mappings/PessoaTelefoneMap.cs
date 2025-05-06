using GeneralLabSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralLabSolutions.InfraStructure.Mappings
{
    public class PessoaTelefoneMap : IEntityTypeConfiguration<PessoaTelefone>
    {
        public void Configure(EntityTypeBuilder<PessoaTelefone> builder)
        {
            builder.HasKey(pt => new { pt.PessoaId, pt.TelefoneId }); // Chave composta

            builder.HasOne(pt => pt.Pessoa)
                   .WithMany(p => p.PessoasTelefones)
                   .HasForeignKey(pt => pt.PessoaId)
                   .OnDelete(DeleteBehavior.Restrict); // Impedir exclusão de Pessoa se houver telefones associados

            builder.HasOne(pt => pt.Telefone)
                   .WithMany(t => t.PessoasTelefones)
                   .HasForeignKey(pt => pt.TelefoneId)
                   .OnDelete(DeleteBehavior.Restrict); // Impedir exclusão de Telefone se houver pessoas associadas

            // Não é necessário configurar .ToTable("PessoaTelefone")
            // porque o nome da tabela, por convenção, será o nome da classe
        }
    }
}
