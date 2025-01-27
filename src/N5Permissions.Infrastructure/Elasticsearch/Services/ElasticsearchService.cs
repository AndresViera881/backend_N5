using Elastic.Clients.Elasticsearch;
using ErrorOr;
using Microsoft.Extensions.Logging;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Domain.Permissions;
using N5Permissions.Infrastructure.Elasticsearch.Models;


namespace N5Permissions.Infrastructure.Elasticsearch.Services
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly ElasticsearchClient client;
        private readonly ILogger<ElasticsearchService> logger;

        public ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        
        public async Task<ErrorOr<bool>> IndexPermissionAsync(Permiso permiso)
        {
            var permisoElastic = new PermisoElastic
            {
                Id = permiso.Id,
                NombreEmpleado = permiso.NombreEmpleado,
                ApellidoEmpleado = permiso.ApellidoEmpleado,
                TipoPermisoDescripcion = permiso.TipoPermiso.Descripcion,
                FechaPermiso = permiso.FechaPermiso
            };

            var response = await client.IndexAsync(permisoElastic);
            if (!response.IsValidResponse)
            {
                logger.LogWarning("Elasticsearch aún no iniciado, el índice no pudo ser guardado.");
                return Error.Failure("General.Failure","Error al guardar el permiso en Elasticsearch.");
            }
            logger.LogInformation("Indice guardado correctamente en Elasticsearch.");
            return true;
        }
    }
}
