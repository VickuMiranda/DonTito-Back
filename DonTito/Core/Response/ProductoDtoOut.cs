﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class ProductoDtoOut
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public float Precio { get; set; }

        public string? Codigo { get; set; }

        public string? Descripcion { get; set; }

        public byte[]? Imagen { get; set; }

        public string? NombreModelo { get; set; }
    }
}
