
namespace SWRCVA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReciboDinero")]
    public partial class ReciboDinero
    {
        [Key]
        public long Consecutivo { get; set; }

        public int IdCotizacion { get; set; }

        public decimal Monto { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Fecha { get; set; }

        public virtual Cotizacion Cotizacion { get; set; }
    }
}