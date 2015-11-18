namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            ColorMaterial = new HashSet<ColorMaterial>();
            ListaMatProducto = new HashSet<ListaMatProducto>();
            MaterialCotizacion = new HashSet<MaterialCotizacion>();
            Producto = new HashSet<Producto>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        public int IdCatMat { get; set; }

        public int? IdSubCatMat { get; set; }
        [Required]
        public int? IdTipoMaterial { get; set; }
        [Required]
        public int IdProveedor { get; set; }
        [Required]
        public int Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public decimal? Costo { get; set; }

        public virtual CategoriaMat CategoriaMat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ColorMaterial> ColorMaterial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaMatProducto> ListaMatProducto { get; set; }

        public virtual Proveedor Proveedor { get; set; }

        public virtual SubCategoria SubCategoria { get; set; }

        public virtual TipoMaterial TipoMaterial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaterialCotizacion> MaterialCotizacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Producto> Producto { get; set; }
    }
}
