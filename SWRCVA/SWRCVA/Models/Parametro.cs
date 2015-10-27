
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
            [Index]
            public int Id { get; set; }
            [Required]
            public int parametro { get; set; }
            [Required]
            [StringLength(100)]
            public string Nombre { get; set; }
            [Required]
            [StringLength(15)]
            public string Usuario { get; set; }
            [Required]
            public string Estado { get; set; }
            public string NombreCat { get; set; }
            public decimal Porcentaje { get; set; }
            public int CategoriaId { get; set; }

    }
}