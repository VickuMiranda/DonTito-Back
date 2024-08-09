using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class PedidoDetalleDtoOut
    {
        public int Id { get; set; }

        public string? NombreProducto { get; set; }

        public int Cantidad { get; set; }

        public float SubTotal { get; set; }

        public long NumeroPedido { get; set; }
    }
}
