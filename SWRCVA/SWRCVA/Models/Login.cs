using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public partial class Login
    {
        public Login()
        {

        }

        [Required]
        public string IdUsuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [DataType(DataType.Password)]
        [Compare("Contraseña", ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmacionContraseña { get; set; }

    }
}