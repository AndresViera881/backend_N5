using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Permissions.Application.Common.Interfaces;
using N5Permissions.Application.Permissions.Commands.ModifyPermission;
using N5Permissions.Contracts.Permissions;
using N5Permissions.Application.Permissions.Commands.RequestPermission;
using N5Permissions.Application.Permissions.Queries.GetPermissions;

namespace N5Permissions.Api.Controllers
{
    [Route("api/permisos")]
    public class PermisosController : BaseController
    {
        private readonly ISender mediator;
        private readonly IElasticsearchService elasticsearchService;

        public PermisosController(ISender mediator, IElasticsearchService elasticsearchService)
        {
            this.mediator = mediator;
            this.elasticsearchService = elasticsearchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var query = new GetPermissionsQuery();
            var getPermissionsResult = await mediator.Send(query);

            return getPermissionsResult.Match(
                x => Ok(x.ConvertAll(permiso => new PermissionResponse(
                    permiso.Id,
                    permiso.NombreEmpleado,
                    permiso.ApellidoEmpleado,
                    new TipoPermisoResponse(
                        permiso.TipoPermiso.Id,
                        permiso.TipoPermiso.Descripcion
                    )))),
                Problem
                );
        }

        [HttpPost]
        public async Task<IActionResult> RequestPermission(RequestPermissionRequest request)
        {
            var command = new RequestPermissionCommand(request.NombreEmpleado, request.ApellidoEmpleado, request.TipoPermiso);
            var requestPermissionResult = await mediator.Send(command);

            if (!requestPermissionResult.IsError)
            {
                // Guardar el permiso en Elasticsearch, ver log para resultados.
                await elasticsearchService.IndexPermissionAsync(requestPermissionResult.Value);
            }

            return requestPermissionResult.Match(
                x => Created(
                    $"api/permisos/{requestPermissionResult.Value.Id}",
                    new PermissionResponse(
                        requestPermissionResult.Value.Id,
                        requestPermissionResult.Value.NombreEmpleado,
                        requestPermissionResult.Value.ApellidoEmpleado,
                        new TipoPermisoResponse(
                            requestPermissionResult.Value.TipoPermiso.Id,
                            requestPermissionResult.Value.TipoPermiso.Descripcion
                        )
                    )
                ),
                Problem
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> ModifyPermission(int id, ModifyPermissionRequest request)
        {
            var command =
                new ModifyPermissionCommand(id, request.NombreEmpleado, request.ApellidoEmpleado, request.TipoPermiso);
            var modifyPermissionCommandResult = await mediator.Send(command);

            return modifyPermissionCommandResult.MatchFirst(
                permiso => Ok(new PermissionResponse(
                    id,
                    request.NombreEmpleado,
                    request.ApellidoEmpleado,
                    new TipoPermisoResponse(permiso.TipoPermiso.Id, permiso.TipoPermiso.Descripcion))),
                Problem
            );
        }
    }
}
