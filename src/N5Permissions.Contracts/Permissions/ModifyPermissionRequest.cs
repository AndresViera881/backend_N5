namespace N5Permissions.Contracts.Permissions;

public record ModifyPermissionRequest(
    string NombreEmpleado,
    string ApellidoEmpleado,
    int TipoPermiso
    );