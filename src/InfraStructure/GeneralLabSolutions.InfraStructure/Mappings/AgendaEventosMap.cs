using GeneralLabSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralLabSolutions.InfraStructure.Mappings
{
    public class AgendaEventosMap : IEntityTypeConfiguration<AgendaEventos>
    {
        public void Configure(EntityTypeBuilder<AgendaEventos> builder)
        {

            builder.HasIndex(x => x.Title).HasDatabaseName("IX_AgendaEventos_Title");

            builder.HasKey(e => e.Id);
            builder.Property(p => p.ParticipanteId).IsRequired();
            builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Color).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.Participante)
                .WithMany(x => x.AgendaEventos)
                .HasForeignKey(x => x.ParticipanteId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
