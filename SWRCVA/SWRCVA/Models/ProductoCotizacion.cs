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

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMaterial { get; set; }

        public decimal CantMaterial { get; set; }

        public int CantProducto { get; set; }

        public int IdColorVidrio { get; set; }

        public int IdColorAluminio { get; set; }

        public decimal Instalacion { get; set; }

        public decimal Ancho { get; set; }

        public decimal Alto { get; set; }
        [NotMapped]
        public decimal Subtotal { get; set; }

        public virtual ColorMat ColorMat { get; set; }

        public virtual ColorMat ColorMat1 { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual Material Material { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
