namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cotizacion")]
    public partial class Cotizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cotizacion()
        {
            ProductoCotizacion = new HashSet<ProductoCotizacion>();
        }

        [Key]
        public int IdCotizacion { get; set; }

        public int IdCliente { get; set; }

        [Required]
        [StringLength(1)]
        public string Estado { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaActualizacion { get; set; }

        [StringLength(500)]
        public string Comentario { get; set; }

        public decimal MontoParcial { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion { get; set; }
    }
}
