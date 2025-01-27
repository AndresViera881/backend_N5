using Microsoft.EntityFrameworkCore;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;
using System.Reflection;

namespace N5Permissions.Infrastructure.Common.Persistence
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<TipoPermiso> TipoPermiso { get; set; }
        public async Task CommitChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
