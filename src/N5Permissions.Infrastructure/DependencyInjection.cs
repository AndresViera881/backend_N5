using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Infrastructure.Common.Persistence;
using N5Permissions.Infrastructure.Elasticsearch.Services;
using N5Permissions.Infrastructure.Permisions.Persistence;
using N5Permissions.Infrastructure.TipoPermisos.Persistence;

namespace N5Permissions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("defaultConnection");
                options.UseSqlServer(connectionString);
            });
            services.AddScoped<IPermisosRepository, PermisosRepository>();
            services.AddScoped<ITipoPermisosRepository, TipoPermisosRepository>();
            services.AddScoped<IUnitOfWork>(serviceProvider =>
                serviceProvider.GetRequiredService<ApplicationDbContext>());

            //ejecutar las migraciones de forma automática
            services.AddHostedService<MigrationHostedService>();

            //Elastic search
            // Leer la configuración de Elasticsearch desde appsettings
            var elasticsearchUri = configuration["Elasticsearch:Uri"];
            var settings = new ElasticsearchClientSettings(new Uri(elasticsearchUri))
                .DefaultIndex("permissions");  //Nombre del índice

            //Creo el cliente de Elasticsearch y lo añado al contenedor de dependencias
            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
            // Inyecta el servicio de Elasticsearch
            services.AddSingleton<IElasticsearchService, ElasticsearchService>();

            return services;
        }
    }

    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
