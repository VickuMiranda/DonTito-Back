using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class ProductoUpdateDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
    }
}
