using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Request
{
    public class UsuarioDtoIn
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Email { get; set; }

        public long Dni { get; set; }

        public long Telefono { get; set; }

        public string? Contrasenia { get; set; }

        public int IdDomicilio { get; set; }

        public int IdRol { get; set; }
    }
}
