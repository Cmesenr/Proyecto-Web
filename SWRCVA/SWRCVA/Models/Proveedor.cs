namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Proveedor")]
    public partial class Proveedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proveedor()
        {
            Material = new HashSet<Material>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProveedor { get; set; }

        [Required]
        [StringLength(15)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string Correo { get; set; }

        [Required]
        [StringLength(30)]
        public string Direccion { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Telefono { get; set; }

        public int Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Material { get; set; }
    }
}
