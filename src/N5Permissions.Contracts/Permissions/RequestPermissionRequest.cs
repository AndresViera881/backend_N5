namespace N5Permissions.Contracts.Permissions;

public record RequestPermissionRequest(
    string NombreEmpleado,
    string ApellidoEmpleado,
    int TipoPermiso
    );