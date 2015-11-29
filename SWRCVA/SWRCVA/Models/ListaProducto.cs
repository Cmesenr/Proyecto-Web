using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public class ListaProducto
    {
        public string Nombre { set; get; }
        public decimal Cantidad { set; get; }
        public decimal SubTotal { set; get; }
    }
}