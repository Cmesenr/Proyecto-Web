namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ColorMaterial")]
    public partial class ColorMaterial
    {
        [Key]
        [Column(Order = 0)]
        public int IdMaterial { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IdColorMat { get; set; }

        public decimal Costo { get; set; }
        [NotMapped]
        public string NombreMaterial { get; set; }

        public virtual ColorMat ColorMat { get; set; }

        public virtual Material Material { get; set; }
    }
}
