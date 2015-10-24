using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public partial class UsuarioLogin
    {
        public UsuarioLogin()
        {

        }

        public string IdUsuario { get; set; }

        public string Contraseña { get; set; }
    }
}