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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Factura()
        {
            DetalleFactura = new HashSet<DetalleFactura>();
        }

        [Key]
        public long IdFactura { get; set; }

        public int? IdCotizacion { get; set; }

        public DateTime FechaHora { get; set; }

        public decimal MontoTotal { get; set; }

        public decimal MontoPagar { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public int IdCliente { get; set; }

        public int Estado { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }

        public virtual Usuario Usuario1 { get; set; }
    }
}
