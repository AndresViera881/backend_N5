using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Infrastructure.TipoPermisos.Persistence
{
    public class TipoPermisosConfiguration : IEntityTypeConfiguration<TipoPermiso>
    {
        public void Configure(EntityTypeBuilder<TipoPermiso> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Descripcion).HasMaxLength(50);
            // Seed Data
            builder.HasData(
                new TipoPermiso(1, "Vacaciones"),
                new TipoPermiso(2, "Enfermedad"),
                new TipoPermiso(3, "Maternidad")
            );
        }
    }
}
