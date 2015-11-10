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
        [NotMapped]
        public decimal Subtotal { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        public virtual Material Material { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
