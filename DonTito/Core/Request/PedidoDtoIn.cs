using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Core.Request
{
    public class PedidoDtoIn
    {
        public int Id { get; set; }

        public long Numero { get; set; }

        public float Total { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
