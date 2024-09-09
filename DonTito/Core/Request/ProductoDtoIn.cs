namespace Core.Request
{
    public class ProductoDtoIn
    {
        public string? Nombre { get; set; }

        public float Precio { get; set; }

        public string? Codigo { get; set; }

        public string? Descripcion { get; set; }

        public byte[]? Imagen { get; set; }

        public int IdModelo { get; set; }
    }
}
