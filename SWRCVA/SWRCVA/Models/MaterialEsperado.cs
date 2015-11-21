
namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MaterialEsperado")]
    public partial class MaterialEsperado
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string IdForma { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTipoMaterial { get; set; }

        public virtual TipoMaterial TipoMaterial { get; set; }
    }
}