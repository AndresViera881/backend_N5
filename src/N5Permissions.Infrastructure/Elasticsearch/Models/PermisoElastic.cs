namespace N5Permissions.Infrastructure.Elasticsearch.Models
{
    public class PermisoElastic
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public string TipoPermisoDescripcion { get; set; }
        public DateTime FechaPermiso { get; set; }
    }
}
