using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class DomicilioDtoOut
    {
        public int Id { get; set; }

        public string? Calle { get; set; }

        public int Numero { get; set; }

        public string? Piso { get; set; }

        public string? Departamento { get; set; }

        public string? NombreProvincia { get; set; }
    }
}
