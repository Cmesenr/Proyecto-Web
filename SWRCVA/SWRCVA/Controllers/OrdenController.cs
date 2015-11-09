using PagedList;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class OrdenController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Orden
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Cotizacion" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var ordenes = from s in db.Orden
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                ordenes = ordenes.Where(s => s.IdCotizacion.Equals(int.Parse(searchString)));
            }
            switch (sortOrder)
            {
                case "Cotizacion":
                    ordenes = ordenes.OrderByDescending(s => s.IdCotizacion);
                    break;
                default:  // Name ascending 
                    ordenes = ordenes.OrderBy(s => s.IdCotizacion);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(ordenes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Orden/Procesar
        public ActionResult Procesar()
        {
            return View();
        }

    }
}