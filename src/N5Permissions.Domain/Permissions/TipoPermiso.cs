namespace N5Permissions.Domain.Permissions
{
    public class TipoPermiso
    {
        public int Id { get; private set; }
        public string Descripcion { get; }

        private TipoPermiso(){}

        public TipoPermiso(string descripcion)
        {
            Descripcion = descripcion;
        }
        public TipoPermiso(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }
    }
}
