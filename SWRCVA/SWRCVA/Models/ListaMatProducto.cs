namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ListaMatProducto")]
    public partial class ListaMatProducto
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProducto { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdMaterial { get; set; }

        [Required]
        [StringLength(15)]
        public string Usuario { get; set; }

        [NotMapped]
        public string NombreMaterial { get; set; }

        public virtual Material Material { get; set; }

        public virtual Producto Producto { get; set; }
    }
}
