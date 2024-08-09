using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class DomicilioDtoIn
    {
        public int Id { get; set; }

        public string? Calle { get; set; }

        public int Numero { get; set; }

        public string? Piso { get; set; }

        public string? Departamento { get; set; }

        public int IdProvincia { get; set; }
    }
}
