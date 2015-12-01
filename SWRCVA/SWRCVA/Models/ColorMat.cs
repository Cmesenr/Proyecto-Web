namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ColorMat")]
    public partial class ColorMat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ColorMat()
        {
            ColorMaterial = new HashSet<ColorMaterial>();
            DetalleFactura = new HashSet<DetalleFactura>();
            MaterialItemCotizacion = new HashSet<MaterialItemCotizacion>();
            ProductoCotizacion = new HashSet<ProductoCotizacion>();
            ProductoCotizacion1 = new HashSet<ProductoCotizacion>();
            ProductoCotizacion2 = new HashSet<ProductoCotizacion>();
        }

        [Key]
        public int IdColor { get; set; }

        public int IdCatMaterial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public int Estado { get; set; }

        public virtual CategoriaMat CategoriaMat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ColorMaterial> ColorMaterial { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFactura> DetalleFactura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialItemCotizacion> MaterialItemCotizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductoCotizacion> ProductoCotizacion2 { get; set; }
    }
}
