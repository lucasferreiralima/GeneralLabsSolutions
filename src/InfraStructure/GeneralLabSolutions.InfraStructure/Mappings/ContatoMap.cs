using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralLabSolutions.InfraStructure.Mappings
{
    public class ContatoMap : IEntityTypeConfiguration<Contato>
    {
        public void Configure(EntityTypeBuilder<Contato> builder)
        {

            builder.HasKey(x => x.Id);


            // Configuração de índices para melhorar o desempenho de consultas em campos frequentemente utilizados
            builder.HasIndex(x => x.Email).HasDatabaseName("IX_Contato_Email");

            builder.HasIndex(x => x.Nome).HasDatabaseName("IX_Contato_Nome");


            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(254)");

            builder.Property(x => x.EmailAlternativo)
                .HasColumnType("varchar")
                .HasMaxLength(255);

            builder.Property(x => x.Telefone)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(15); // (21) 99999-9999

            builder.Property(x => x.TelefoneAlternativo)
                .HasColumnType("varchar")
                .HasMaxLength(15);


            builder.Property(x => x.Observacao)
                .HasColumnType("varchar(500)");

            builder.Property(x => x.TipoDeContato)
                .HasEnumConversion()
                .IsRequired();


            builder.ToTable("Contato");
        }
    }
}
