using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Response
{
    public class PedidoDtoOut
    {
        public int Id { get; set; }

        public long Numero { get; set; }

        public float Total { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
