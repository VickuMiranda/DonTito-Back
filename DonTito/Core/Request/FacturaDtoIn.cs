using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Request
{
    public class FacturaDtoIn
    {
        public int Id { get; set; }

        public float MontoTotal { get; set; }

        public DateTime Fecha { get; set; }
        public long Numero { get; set; }

        //public int IdPedido { get; set; }
    }
}
