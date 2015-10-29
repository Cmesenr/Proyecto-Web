namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Factura = new HashSet<Factura>();
        }

        [Key]
        [StringLength(15)]
        public string IdUsuario { get; set; }

        [Required]
        [StringLength(8)]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        public int IdRol { get; set; }

        public int Estado { get; set; }

        [Column("Usuario")]
        [Required]
        [StringLength(15)]
        public string Usuario1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Factura> Factura { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
