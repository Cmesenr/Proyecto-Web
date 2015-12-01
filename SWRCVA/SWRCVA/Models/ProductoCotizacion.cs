namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductoCotizacion")]
    public partial class ProductoCotizacion
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCotizacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }
        [NotMapped]
        public string Nombre { get; set; }
        public int CantProducto { get; set; }

        public int IdColorVidrio { get; set; }

        public int IdColorAluminio { get; set; }

        public int? IdColorPaleta { get; set; }

        public decimal? AnchoCelocia { get; set; }

        public decimal Instalacion { get; set; }

        public decimal Ancho { get; set; }

        public decimal Alto { get; set; }
        public decimal Subtotal { get; set; }

        public decimal? Largo { get; set; }

        public int? Divisiones { get; set; }
        [NotMapped]
        public decimal CantMat { get; set; }
        [NotMapped]
        public string Tipo { get; set; }
        [NotMapped]
        public string ColorVidrio { get; set; }
        [NotMapped]
        public string ColorPaleta { get; set; }

        [NotMapped]
        public string ColorAluminio { get; set; }
        [NotMapped]
        public int IdColor { get; set; }

        public virtual ColorMat ColorMat { get; set; }

        public virtual ColorMat ColorMat1 { get; set; }

        public virtual ColorMat ColorMat2 { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
