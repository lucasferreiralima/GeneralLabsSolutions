using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralLabSolutions.InfraStructure.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasIndex(x => x.Nome).HasDatabaseName("IX_Cliente_Nome");
            builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("IX_Cliente_Email");
            builder.HasIndex(x => x.Documento).IsUnique().HasDatabaseName("IX_Cliente_Documento");

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(x => x.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)"); // CPF ou CNPJ

            builder.Property(x => x.TipoDePessoa)
                .HasEnumConversion()
                .IsRequired();


            builder.Property(x => x.Email)
                .IsRequired()
                .HasColumnType("varchar(254)");

            builder.Property(x => x.StatusDoCliente)
                .HasEnumConversion()
                .IsRequired();

            builder.Property(x => x.TipoDeCliente)
                .HasEnumConversion()
                .IsRequired();


            builder.HasMany(x => x.Pedidos)
                .WithOne(x => x.Cliente)
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Pessoa)
                   .WithOne() //  Cliente  tem  uma  Pessoa,  mas  Pessoa  não  tem  Cliente  diretamente
                   .HasForeignKey<Cliente>(c => c.PessoaId);

            builder.ToTable("Cliente");
        }
    }
}
