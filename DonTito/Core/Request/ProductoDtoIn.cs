using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class ProductoDtoIn
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public float Precio { get; set; }

        public string? Codigo { get; set; }

        public string? Descripcion { get; set; }

        public int IdModelo { get; set; }
    }
}
