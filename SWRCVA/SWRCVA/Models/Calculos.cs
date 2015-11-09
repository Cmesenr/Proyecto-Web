using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public class Calculos
    {
        private DataContext db = new DataContext();
        public static decimal calcularMonto(int Idpro, int Cvidrio, int CAluminio, int Insta, int Cant, decimal Ancho, decimal Alto)
        {




            return 0;
        }
        public string ValidarMateriales(int Idpro, int Cvidrio, int CAluminio)
        {
            string resultado = "";
            var MatAlumino = (from s in db.ListaMatProducto
                              join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                              where s.IdProducto == Idpro && s.Material.IdCatMat == 2
                              select new
                              {
                                  s.IdMaterial,
                                  c.IdColorMat,
                                  c.Costo
                              }).ToList();

            foreach(var item in MatAlumino)
            {
                if(item.IdColorMat== CAluminio)
                {
                    resultado = "Ready";
                    
                }
            }

            return resultado;
        }
    }
}