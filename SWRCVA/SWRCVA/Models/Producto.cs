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
            Orden = new HashSet<Orden>();
            ProductoCotizacion = new HashSet<ProductoCotizacion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public int IdTipoProducto { get; set; }

        [Required]
        public byte[] Imagen { get; set; }

        public int Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaMatProducto> ListaMatProducto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orden> Orden { get; set; }

        public virtual TipoProducto TipoProducto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion { get; set; }
    }
}
