
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace SWRCVA.Models
    {
        public class Parametro
        {
            [Required]
            public int parametro { get; set; }
            [Required]
            [StringLength(15)]
            public string Nombre { get; set; }

            [Required]
            [StringLength(15)]
            public string Usuario { get; set; }

            public int Estado { get; set; }
        }
    }