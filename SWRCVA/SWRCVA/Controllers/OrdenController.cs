using PagedList;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class OrdenController : Controller
    {
        private DataContext db = new DataContext();
        List<ProductoCotizacion> ListaProductosOrden = new List<ProductoCotizacion>();
        List<MaterialCotizacion> ListaMaterialesOrden = new List<MaterialCotizacion>();

        // GET: Orden
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var cotizaciones = from s in db.Cotizacion
                               where s.Estado == "P"|| s.Estado == "F"|| s.Estado == "A"
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cotizaciones = cotizaciones.Where(s => s.Cliente.Nombre.Contains(searchString) ||
                                                    s.IdCotizacion.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    cotizaciones = cotizaciones.OrderByDescending(s => s.Cliente.Nombre);
                    break;
                default:  // Name ascending 
                    cotizaciones = cotizaciones.OrderByDescending(s => s.IdCotizacion);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(cotizaciones.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cliente/Editar/5
        public ActionResult Detalles(int? id)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Cotizacion cotizacion = db.Cotizacion.Find(id);

            var ListaProductoCotizacionOrden = db.ProductoCotizacion.Where(s => s.IdCotizacion == id).ToList();
            var ListaMaterialCotizacionOrden = db.MaterialItemCotizacion.Where(s => s.IdCotizacion == id).ToList();
            if (ListaProductosOrden.Count() == 0)
            {
                foreach(var item in ListaProductoCotizacionOrden)
                {
                    ProductoCotizacion Produ = new ProductoCotizacion();
                    Produ.IdProducto = item.IdProducto;
                    Produ.IdCotizacion = item.IdCotizacion;
                    if (item.ColorVidrio != null)
                    {
                        Produ.ColorVidrio = item.ColorMat.Nombre;
                    }
                    Produ.ColorAluminio = item.ColorMat1.Nombre;
                    Produ.AnchoCelocia = item.AnchoCelocia;
                    Produ.Nombre = item.Producto.Nombre;
                    Produ.Alto = item.Alto;
                    Produ.Ancho = item.Ancho;
                    Produ.CantMat = item.CantProducto;
                    if (item.IdColorPaleta != null)
                    {
                        Produ.ColorPaleta = item.ColorMat2.Nombre;
                    }                    
                    Produ.Subtotal = item.Subtotal;
                    ListaProductosOrden.Add(Produ);
                }
                
                foreach (var item in ListaMaterialCotizacionOrden)
                {
                    ProductoCotizacion Produ = new ProductoCotizacion();
                    Produ.IdProducto = item.IdMaterial;
                    Produ.IdCotizacion = item.IdCotizacion;
                    Produ.Nombre = item.Material.Nombre;
                    if (item.Material.IdCatMat == 2)
                    {
                        Produ.ColorAluminio = item.ColorMat.Nombre;
                    }
                    if (item.Material.IdCatMat == 3)
                    {
                        Produ.ColorVidrio = item.ColorMat.Nombre;
                    }
                    Produ.CantMat = item.Cantidad;
                    Produ.Alto = (decimal)item.Alto;
                    Produ.Ancho = (decimal)item.Ancho;
                    Produ.Subtotal=item.Subtotal;
                    ListaProductosOrden.Add(Produ);
                }
            }
            TempData["ListaProductosOrden"] = ListaProductosOrden;
            Session["IdCotizacion"] = id;

            return View(cotizacion);
        }

        public JsonResult ConsultarListaProductos()
        {
            if (TempData["ListaProductosOrden"] != null)
            {
                ListaProductosOrden = (List<ProductoCotizacion>)TempData["ListaProductosOrden"];
            }

            TempData["ListaProductosOrden"] = ListaProductosOrden;

            return Json(ListaProductosOrden,JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarMateriales(int idProducto)
        {
            int idCotizacion = int.Parse(Session["IdCotizacion"].ToString());

                var ListaMateriales = from s in db.MaterialCotizacion
                                           where (s.IdProducto == idProducto && s.IdCotizacion == idCotizacion)
                                           select s;         

            foreach(var item in ListaMateriales)
            {
                MaterialCotizacion materialCotizacion = new MaterialCotizacion();
                materialCotizacion.IdMaterial = item.IdMaterial;
                materialCotizacion.Nombre = item.Material.Nombre;
                materialCotizacion.CantMaterial = item.CantMaterial;

                ListaMaterialesOrden.Add(materialCotizacion);
            }

            return Json(ListaMaterialesOrden, JsonRequestBehavior.AllowGet);
        }
    }
}