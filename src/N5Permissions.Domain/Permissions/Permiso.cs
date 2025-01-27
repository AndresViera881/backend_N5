namespace N5Permissions.Domain.Permissions
{
    public class Permiso
    {
        public int Id { get; private set; }
        public string NombreEmpleado { get; private set; }
        public string ApellidoEmpleado { get; private set; }
        public TipoPermiso TipoPermiso { get; private set; }
        public DateTime FechaPermiso { get; }

        private Permiso(){}

        public Permiso(string nombreEmpleado, string apellidoEmpleado, TipoPermiso tipoPermiso)
        {
            NombreEmpleado = nombreEmpleado;
            ApellidoEmpleado = apellidoEmpleado;
            TipoPermiso = tipoPermiso ?? throw new ArgumentNullException(nameof(tipoPermiso));
            FechaPermiso = DateTime.UtcNow;
        }
        public void UpdatePermiso(string nombreEmpleado, string apellidoEmpleado)
        {
            if (string.IsNullOrWhiteSpace(nombreEmpleado) || string.IsNullOrWhiteSpace(apellidoEmpleado))
                throw new ArgumentException("El nombre y apellido del empleado son obligatorios.");

            NombreEmpleado = nombreEmpleado;
            ApellidoEmpleado = apellidoEmpleado;
        }
        public void SetTipoPermiso(TipoPermiso tipoPermiso)
        {
            TipoPermiso = tipoPermiso ?? throw new ArgumentNullException(nameof(tipoPermiso));
        }
    }
}
