using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Response
{
    public class UsuarioDtoOut
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Contrasenia { get; set; }
    }
}
