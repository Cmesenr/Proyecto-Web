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
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdConsecutivo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long IdFactura { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdColor { get; set; }

        public decimal MontoParcial { get; set; }

        public decimal Cantidad { get; set; }

        public virtual ColorMat ColorMat { get; set; }

        public virtual Factura Factura { get; set; }

        public virtual Material Material { get; set; }

        public virtual Producto Producto { get; set; }


    }
}
