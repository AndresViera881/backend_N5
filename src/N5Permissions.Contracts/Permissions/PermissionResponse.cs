namespace N5Permissions.Contracts.Permissions;

public record PermissionResponse(
    int Id,
    string NombreEmpleado,
    string ApellidoEmpleado,
    TipoPermisoResponse TipoPermiso
);