namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetalleFactura")]
    public partial class DetalleFactura
    {
        public int IdProducto { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdFactura { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public decimal MontoParcial { get; set; }

        public int IdCotizacion { get; set; }

        public virtual Factura Factura { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
