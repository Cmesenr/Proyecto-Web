namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Orden")]
    public partial class Orden
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orden()
        {
            DetalleFactura = new HashSet<DetalleFactura>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdCotizacion { get; set; }

        public int IdCliente { get; set; }

        public int CantidadProd { get; set; }

        public int Estado { get; set; }

        [StringLength(500)]
        public string Comentario { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaActualizacion { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
