using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5Permissions.Domain.Permissions;

namespace N5Permissions.Infrastructure.Permisions.Persistence
{
    public class PermisosConfiguration : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NombreEmpleado).HasMaxLength(75);
            builder.Property(x => x.ApellidoEmpleado).HasMaxLength(75);
            builder.Property(x => x.FechaPermiso).HasColumnType("datetime2");

            //Relacion entre Permiso y TipoPermisoId
            builder.HasOne(p => p.TipoPermiso)
                .WithMany()
                .HasForeignKey("TipoPermisoId")
                .IsRequired();

            //Seed Data
            //builder.HasData(
            //    new
            //    {
            //        Id = 1,
            //        NombreEmpleado = "Juan",
            //        ApellidoEmpleado = "Pérez",
            //        TipoPermisoId = 1, // Relación con Administrador
            //        FechaPermiso = new DateTime(2025, 1, 25)
            //    },
            //    new
            //    {
            //        Id = 2,
            //        NombreEmpleado = "Ana",
            //        ApellidoEmpleado = "López",
            //        TipoPermisoId = 2, // Relación con Usuario
            //        FechaPermiso = new DateTime(2025, 1, 25)
            //    }
            //);
        }
    }
}
