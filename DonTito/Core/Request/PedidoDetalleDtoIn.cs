﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class PedidoDetalleDtoIn
    {
        public int Id { get; set; }

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public float SubTotal { get; set; }

        public int IdPedido { get; set; }
    }
}