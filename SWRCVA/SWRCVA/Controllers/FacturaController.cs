using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWRCVA.Models;
using PagedList;

namespace SWRCVA.Controllers
{
    public class FacturaController : Controller
    {
        private DataContext db = new DataContext();
        List<ProductoCotizacion> ListaProductos = new List<ProductoCotizacion>();
        // GET: Factura
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
                               where s.Estado == "T" || s.Estado == "A"
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
            return View(cotizaciones.ToPagedList(pageNumber, pageSize));
        }

     

        // GET: Factura/Create
        public ActionResult Facturar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }

            var ListaProdu = (from s in db.ProductoCotizacion
                              where s.IdCotizacion == id
                              select s).ToList();
            List<ProductoCotizacion> ListaP = new List<ProductoCotizacion>();
            foreach (var item in ListaProdu)
            {
                ProductoCotizacion p = new ProductoCotizacion();
                p.IdProducto = item.IdProducto;
                p.Nombre = item.Producto.Nombre;
                p.CantProducto = item.CantProducto;
                p.IdColorPaleta = item.IdColorPaleta;
                p.AnchoCelocia = item.AnchoCelocia;
                p.IdColorVidrio = item.IdColorVidrio;
                p.IdColorAluminio = item.IdColorAluminio;
                p.Instalacion = item.Instalacion;
                p.Ancho = item.Ancho;
                p.Alto = item.Alto;
                p.Subtotal = item.Subtotal;
                ListaP.Add(p);
            }
 

            foreach (var itemProduct in ListaP)
            {
                ListaProductos.Add(itemProduct);
            }
            TempData["ListaProductosFact"] = ListaProductos;
            return View();
        }

        // POST: Factura/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Facturar([Bind(Include = "IdFactura,FechaHora,MontoTotal,MontoPagar,Usuario,IdCliente,Estado")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Factura.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", factura.IdCliente);
            ViewBag.Usuario = new SelectList(db.Usuario, "IdUsuario", "Contraseña", factura.Usuario);
            return View(factura);
        }

        // GET: Factura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", factura.IdCliente);
            ViewBag.Usuario = new SelectList(db.Usuario, "IdUsuario", "Contraseña", factura.Usuario);
            return View(factura);
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFactura,FechaHora,MontoTotal,MontoPagar,Usuario,IdCliente,Estado")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", factura.IdCliente);
            ViewBag.Usuario = new SelectList(db.Usuario, "IdUsuario", "Contraseña", factura.Usuario);
            return View(factura);
        }

        // GET: Factura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Factura.Find(id);
            db.Factura.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ConsultarClientes(string filtro)
        {
            var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro)
                            select new
                            {
                                s.IdCliente,
                                s.Nombre,
                                s.Telefono,
                                s.Correo
                            }).Take(5);

            return Json(Clientes,
             JsonRequestBehavior.AllowGet);

        }
        public JsonResult GuardarCliente(string Nombre, string Telefono, string Correo, string Direccion)
        {
            Cliente C1 = new Cliente();
            try
            {

                C1.Nombre = Nombre;
                C1.Telefono = Telefono;
                C1.Correo = Correo;
                C1.Direccion = Direccion;
                C1.Estado = 1;
                C1.Usuario = Session["UsuarioActual"].ToString();
                db.Cliente.Add(C1);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var resultado = "Un ocurrido un error Inesperado. Contacte al administrador del sistema \n\n" + e;
                return Json(resultado,
                  JsonRequestBehavior.AllowGet);
            }
            var Cliente = new { IdCliente = C1.IdCliente, Nombre = Nombre };
            return Json(Cliente,
              JsonRequestBehavior.AllowGet);
        }
        public JsonResult CalcularTotal()
        {
            decimal Total = 0;
            try
            {

                if (TempData["ListaProductosFact"] != null)
                {
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
                    foreach (var item in ListaProductos)
                    {
                        Total += item.Subtotal;
                    }

                }

            }
            catch (Exception e)
            {
                var resultado = "Un ocurrido un error Inesperado. Contacte al administrador del sistema \n\n" + e;
                return Json(resultado,
                  JsonRequestBehavior.AllowGet);
            }

            TempData["ListaProductosFact"] = ListaProductos;
            return Json(Total,
             JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarListaProductos()
        {
            if (TempData["ListaProductosFact"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
            }
            TempData["ListaProductosFact"] = ListaProductos;
            return Json(ListaProductos,
         JsonRequestBehavior.AllowGet);
        }
        public void LimpiarListas()
        {
            TempData["ListaProductosFact"] = null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
