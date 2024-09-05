using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class ImagenDtoOut
    {
        public int Id { get; set; }
        public List<byte[]>? Url { get; set; }
        public  string? NombreProducto { get; set; }
    }
}
