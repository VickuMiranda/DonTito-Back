using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class ImagenDtoIn
    {
        
        public List<byte[]>? Url { get; set; }

        public int IdProducto { get; set; }
    }
}
