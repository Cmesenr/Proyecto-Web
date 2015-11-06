
namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoMaterial")]
    public partial class TipoMaterial
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoMaterial()
        {
            Material = new HashSet<Material>();
        }

        [Key]
        public int IdTipoMaterial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public int IdCatMat { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        public int Estado { get; set; }

        public virtual CategoriaMat CategoriaMat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Material { get; set; }
    }
}
