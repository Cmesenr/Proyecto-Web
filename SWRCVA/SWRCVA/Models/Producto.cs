namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Producto")]
    public partial class Producto
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            ListaMatProducto = new HashSet<ListaMatProducto>();
            DetalleFactura = new HashSet<DetalleFactura>();
            ProductoCotizacion = new HashSet<ProductoCotizacion>();
        }

        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public int IdTipoProducto { get; set; }

        [Required]
        [StringLength(50)]
        public string Forma { get; set; }

        [Column(TypeName = "image")]
        public byte[] Imagen { get; set; }

        public int Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }
        [NotMapped]
        public decimal Subtotal { get; set; }
        [NotMapped]
        public int Cantidad { get; set; }

        [NotMapped]
        public decimal Ancho { get; set; }

        [NotMapped]
        public decimal Alto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaMatProducto> ListaMatProducto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }


        public virtual TipoProducto TipoProducto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion { get; set; }
    }
}
