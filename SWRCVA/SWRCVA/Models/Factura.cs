namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Factura")]
    public partial class Factura
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdFactura { get; set; }

        public DateTime FechaHora { get; set; }

        public decimal MontoTotal { get; set; }

        public decimal MontoPagar { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public int IdCliente { get; set; }

        public int Estado { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual DetalleFactura DetalleFactura { get; set; }

        public virtual Usuario Usuario1 { get; set; }
    }
}
