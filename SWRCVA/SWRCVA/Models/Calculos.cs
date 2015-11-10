using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public class Calculos
    {
        private DataContext db = new DataContext();
        List<ProductoCotizacion> ListaCosto = new List<ProductoCotizacion>();
        public List<ProductoCotizacion> calcularMonto(int Idpro, int Cvidrio, int CAluminio,int Insta, int Cant, decimal Ancho, decimal Alto)
        {
            var producto = db.Producto.Find(Idpro);
            var Aluminios= (from s in db.ListaMatProducto
                             join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                             where s.IdProducto == Idpro && s.Material.IdCatMat == 2 && c.IdColorMat == CAluminio
                             select new
                             {
                                 s.IdProducto,
                                 s.IdMaterial,
                                 s.Material.IdTipoMaterial,
                                 c.Costo
                             }).ToList();
            var Vidrios= (from s in db.ListaMatProducto
                          join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                          where s.IdProducto == Idpro && s.Material.IdCatMat == 3 && c.IdColorMat == Cvidrio
                          select new
                          {
                              s.IdProducto,
                              s.IdMaterial,
                              s.Material.IdTipoMaterial,
                              c.Costo
                          }).ToList();
            var Acesorios= (from s in db.ListaMatProducto
                            where s.IdProducto == Idpro && s.Material.IdCatMat == 1
                            select new
                            {
                                s.IdProducto,
                                s.IdMaterial,
                                s.Material.IdTipoMaterial,
                                s.Material.Costo
                            }).ToList();

            var materiales = Aluminios;
            materiales.AddRange(Vidrios);
            materiales.AddRange(Acesorios);

            //Ventana 5020
            if (producto.IdTipoProducto == 1)
            {
                decimal SupH = 0;
                decimal InfH = 0;
                var Movil = 0;
                var fijo = 0;
                decimal VertCent = 0;
                decimal VertLlav = 0;
                decimal cierre = 0;
                decimal empaq = 0;
                decimal felpa = 0;
                decimal IV =1+db.Valor.Find(2).Porcentaje;
                

                foreach (var item in producto.Forma.ToString())
                {
                    if (item == 'M')
                    {
                        Movil++;
                    }
                    if(item == 'F')
                    {
                        fijo++;
                    }
                }
                
                if (producto.Forma.Count()==2)
                {
                    VertCent = Alto * 2;
                    VertLlav= Alto * 2;
                    empaq = (Ancho * 2) + (Alto * 2);
                    felpa= (Ancho * 2) + (Alto * 2);
                    if (Movil == 2)
                    {
                        SupH = Ancho;
                        InfH = Ancho;
                        cierre = 2;
                    }
                    else
                    {
                        SupH = Ancho*1.5m;
                        InfH = Ancho*0.5m;
                        cierre = 1;
                    }
                }
                else if (producto.Forma.Count() == 3)
                {
                    cierre = 2;
                    empaq = (Ancho * 2) + (Alto * 6);
                    if (Movil == 2)
                    {
                        SupH = Ancho*1.5m;
                        InfH = Ancho*0.5m;
                        VertCent = Alto * 4;
                        VertLlav = Alto * 2;
                        felpa = Alto*6;

                    }
                    else
                    {
                        SupH = Ancho/3*5;
                        InfH = Ancho/3;
                        VertCent = Alto * 2;
                        VertLlav = Alto * 4;
                        felpa = (Ancho / 3) + (Alto * 3);
                    }
                }
                else
                {
                    SupH = Ancho * 1.5m;
                    InfH = Ancho * 0.5m;
                    VertCent = Alto * 4;
                    VertLlav = Alto * 4;
                    cierre = 1;
                    empaq = (Ancho * 2) + (Alto * 6);
                    felpa = Alto * 6;
                }
                foreach (var item in materiales)
                    {
                    ProductoCotizacion PC = new ProductoCotizacion();
                        switch (item.IdTipoMaterial)
                        {
                            case 22://Cargador
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Ancho;
                            PC.Subtotal= PC.CantMaterial *((decimal)item.Costo*IV);
                            ListaCosto.Add(PC);
                            break;
                            case 20://Umbral
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Ancho;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 21://Jamba
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Alto*2;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 24://Sup hoja
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial =SupH;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 23://Inf Hoja
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = InfH;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 26://Vertical Centro
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = VertCent;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 25://Vertical
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = VertLlav;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 16://Rodin
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Movil*2;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 5://Cierre
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = cierre;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 8://Empaque
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = empaq;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 53://Vidrio
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Ancho*Alto;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 10://Felpa
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = Ancho * Alto;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                            case 17://Tornillo
                            PC.IdMaterial = item.IdMaterial;
                            PC.IdProducto = Idpro;
                            PC.CantMaterial = 1;
                            PC.Subtotal = PC.CantMaterial * ((decimal)item.Costo * IV);
                            ListaCosto.Add(PC);
                            break;
                        }
                   
                   }
            }

            return ListaCosto;
        }
        public List<string>  ValidarMateriales(int Idpro, int Cvidrio, int CAluminio)
        {

            var MatAlumino = (from s in db.ListaMatProducto
                              where s.IdProducto == Idpro && s.Material.IdCatMat == 2
                              select s.Material.Nombre
                              ).ToList();
            var MatAlumino2 = (from s in db.ListaMatProducto
                               join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                               where s.IdProducto == Idpro && s.Material.IdCatMat == 2 && c.IdColorMat == CAluminio
                               select s.Material.Nombre).ToList();

            var Diferentes = MatAlumino.Except(MatAlumino2).ToList();
            var MatVidrio = (from s in db.ListaMatProducto
                              where s.IdProducto == Idpro && s.Material.IdCatMat == 3
                              select s.Material.Nombre
                             ).ToList();
            var MatVidrio2 = (from s in db.ListaMatProducto
                               join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                               where s.IdProducto == Idpro && s.Material.IdCatMat == 3 && c.IdColorMat == Cvidrio
                              select s.Material.Nombre).ToList();
            Diferentes.AddRange(MatVidrio.Except(MatVidrio2).ToList());

            return Diferentes;
        }
    }
}