namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Valor")]
    public partial class Valor
    {
        [Key]
        public int IdValor { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public decimal Porcentaje { get; set; }

        public int Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }
    }
}
