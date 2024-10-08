﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public float Precio { get; set; }

    public string Codigo { get; set; }

    public string Descripcion { get; set; }

    public int IdModelo { get; set; }

    public byte[] Imagen { get; set; }

    public virtual Modelo IdModeloNavigation { get; set; }

    public virtual ICollection<PedidoDetalle> PedidoDetalle { get; set; } = new List<PedidoDetalle>();
}