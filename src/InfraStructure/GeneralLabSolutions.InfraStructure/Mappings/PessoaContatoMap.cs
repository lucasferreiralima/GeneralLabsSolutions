using GeneralLabSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralLabSolutions.InfraStructure.Mappings
{
    public class PessoaContatoMap : IEntityTypeConfiguration<PessoaContato>
    {
        public void Configure(EntityTypeBuilder<PessoaContato> builder)
        {
            builder.HasKey(pt => new { pt.PessoaId, pt.ContatoId }); // Chave composta

            builder.HasOne(pt => pt.Pessoa)
                   .WithMany(p => p.PessoasContatos)
                   .HasForeignKey(pt => pt.PessoaId);

            builder.HasOne(pt => pt.Contato)
                   .WithMany(t => t.PessoasContatos)
                   .HasForeignKey(pt => pt.ContatoId);
        }
    }
}
