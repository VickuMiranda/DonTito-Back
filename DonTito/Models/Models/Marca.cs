﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Marca
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<Modelo> Modelo { get; set; } = new List<Modelo>();
}