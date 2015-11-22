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
        List<Producto> ListaProductosOrden = new List<Producto>();
        List<ProductoCotizacion> ListaProductoCotizacionOrden = new List<ProductoCotizacion>();
        List<MaterialCotizacion> ListaMaterialesOrden = new List<MaterialCotizacion>();

        // GET: Orden
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

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
                               where s.Estado == "P"
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cotizaciones = cotizaciones.Where(s => s.Cliente.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    cotizaciones = cotizaciones.OrderByDescending(s => s.Cliente.Nombre);
                    break;
                default:  // Name ascending 
                    cotizaciones = cotizaciones.OrderBy(s => s.Cliente.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            Session["IdCotizacion"] = null;

            return View(cotizaciones.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cliente/Editar/5
        public ActionResult Detalles(int? id)
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            Cotizacion cotizacion = db.Cotizacion.Find(id);

            var ListaProductoCotizacionOrden = db.ProductoCotizacion.Where(s => s.IdCotizacion == id).GroupBy(s => s.IdProducto);

            if (ListaProductosOrden.Count() == 0)
            {
                foreach (var prodcot in ListaProductoCotizacionOrden)
                {
                    Producto ProAlmac = db.Producto.Find(prodcot.Key);
                    Producto Produ = new Producto();
                    Produ.IdProducto = ProAlmac.IdProducto;
                    Produ.Nombre = ProAlmac.Nombre;
                    foreach (var item in ListaProductoCotizacionOrden)
                    {
                        foreach (var g in item)
                        {
                            if (g.IdProducto == ProAlmac.IdProducto)
                            {
                                Produ.Cantidad = g.CantProducto;
                                Produ.Alto = g.Alto;
                                Produ.Ancho = g.Ancho;
                                Produ.ColorVidrio = g.ColorMat.Nombre;
                                Produ.ColorAluminio = g.ColorMat1.Nombre;
                                if (g.AnchoCelocia == null)
                                {
                                    Produ.AnchoCelocia = 0;
                                }
                                else
                                {
                                    Produ.AnchoCelocia = g.AnchoCelocia;
                                }
                            }
                        }
                    }
                    ListaProductosOrden.Add(Produ);
                }
            }

            TempData["ListaProductoCotizacionOrden"] = ListaProductoCotizacionOrden;
            TempData["ListaProductosOrden"] = ListaProductosOrden;
            Session["IdCotizacion"] = id;

            return View(cotizacion);
        }

        public JsonResult ConsultarListaProductos()
        {
            if (TempData["ListaProductosOrden"] != null)
            {
                ListaProductosOrden = (List<Producto>)TempData["ListaProductosOrden"];
            }

            TempData["ListaProductosOrden"] = ListaProductosOrden;

            return Json(ListaProductosOrden,JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarOrden()
        {
            var respuesta = "Orden Terminada!";

            if (Session["IdCotizacion"] != null)
            {
                Cotizacion cotizacion = db.Cotizacion.Find(int.Parse(Session["IdCotizacion"].ToString()));
                cotizacion.Estado = "T";
                db.Entry(cotizacion).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
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