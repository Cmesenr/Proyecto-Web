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

        // GET: Cotizacion/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);

            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Cotizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdCotizacion,IdCliente,CantProducto,Estado,Fecha,Usuario,MontoParcial")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orden);
        }
    }
}