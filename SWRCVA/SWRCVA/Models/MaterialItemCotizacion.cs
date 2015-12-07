
namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterialItemCotizacion")]
    public partial class MaterialItemCotizacion
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdConsecutivo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCotizacion { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMaterial { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdColor { get; set; }

        public decimal Cantidad { get; set; }
        public decimal? Ancho { get; set; }

        public decimal? Alto { get; set; }

        public decimal Subtotal { get; set; }
        public virtual ColorMat ColorMat { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual Material Material { get; set; }
    }
}