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
    public class CotizacionController : Controller
    {
        private DataContext db = new DataContext();
        List<Producto> ListaProductos = new List<Producto>();
        // GET: Cotizacion
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

        // GET: Cotizacion/Create
        public ActionResult Cotizar()
        {
            ViewBag.Productos = new SelectList(db.Producto, "IdProducto", "Nombre");
            ViewBag.ColoresVidrio=new SelectList((from s in db.ColorMat
                                                  where s.IdCatMaterial == 3
                                                  select new
                                                  {
                                                      IdColor = s.IdColor,
                                                      Nombre = s.Nombre
                                                  }), "IdColor", "Nombre");
            ViewBag.ColoresAluminio = new SelectList((from s in db.ColorMat
                                                    where s.IdCatMaterial == 2
                                                    select new
                                                    {
                                                        IdColor = s.IdColor,
                                                        Nombre = s.Nombre
                                                    }), "IdColor", "Nombre");
            ViewBag.Instalacion = new SelectList((from s in db.Valor
                                                  where s.Tipo == "I"
                                                  select new
                                                  {
                                                      IdValor = s.IdValor,
                                                      Nombre = s.Nombre
                                                  }), "IdValor", "Nombre");
            return View();
        }

        // POST: Cotizacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cotizar([Bind(Include = "IdCotizacion,IdCliente,CantProducto,Estado,Fecha,Usuario,MontoParcial")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                db.Cotizacion.Add(cotizacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Productos = new SelectList(db.Producto, "IdProducto", "Nombre");
            ViewBag.ColoresVidrio = new SelectList((from s in db.ColorMat
                                                    where s.IdCatMaterial == 3
                                                    select new
                                                    {
                                                        IdColor = s.IdColor,
                                                        Nombre = s.Nombre
                                                    }), "IdColor", "Nombre");
            ViewBag.ColoresAluminio = new SelectList((from s in db.ColorMat
                                                      where s.IdCatMaterial == 2
                                                      select new
                                                      {
                                                          IdColor = s.IdColor,
                                                          Nombre = s.Nombre
                                                      }), "IdColor", "Nombre");
            ViewBag.Instalacion = new SelectList((from s in db.Valor
                                                  where s.Tipo == "V"
                                                  select new
                                                  {
                                                      IdValor = s.IdValor,
                                                      Nombre = s.Nombre
                                                  }), "IdValor", "Nombre");
            return View(cotizacion);
        }

        // GET: Cotizacion/Edit/5
        public ActionResult Editar(int? id)
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
            return View(cotizacion);
        }

        // POST: Cotizacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdCotizacion,IdCliente,CantProducto,Estado,Fecha,Usuario,MontoParcial")] Cotizacion cotizacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cotizacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cotizacion);
        }

        // GET: Cotizacion/Delete/5
        public ActionResult Borrar(int? id)
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
            return View(cotizacion);
        }

        // POST: Cotizacion/Delete/5
        [HttpPost, ActionName("Borrar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cotizacion cotizacion = db.Cotizacion.Find(id);
            db.Cotizacion.Remove(cotizacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult AgregarProducto(int Idpro, int Cvidrio, int CAluminio, int Insta, int Cant, decimal Ancho, decimal Alto)
        {
            Calculos C = new Calculos();
            var resultado = "No se pudo agregar el Producto";
            var ValidarMat = C.ValidarMateriales(Idpro, Cvidrio, CAluminio);
            if (ValidarMat.Count()==0)
            {
                var resultCalculo = C.calcularMonto(Idpro, Cvidrio, CAluminio, Insta, Cant, Ancho, Alto);
            

            }
            else
            {
                TempData["ListaProductos"] = ListaProductos;
                return Json(ValidarMat,
                JsonRequestBehavior.AllowGet);
            }
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];

            }
            try
            {

                Producto ListPro = db.Producto.Find(Idpro);
                ListPro.IdProducto = Idpro;
                ListPro.Nombre = ListPro.Nombre;
                if (ListaProductos.Count() == 0) { ListaProductos.Add(ListPro); }
                else
                {
                    foreach (Producto listProduct in ListaProductos)
                    {
                        if (listProduct.IdProducto == Idpro)
                        {
                            TempData["ListaProductos"] = ListaProductos;
                            resultado = "No se puede duplicar el El producto!";
                            return Json(resultado,
                            JsonRequestBehavior.AllowGet);
                        }

                    }
                    ListaProductos.Add(ListPro);
                }
            }
            catch
            {
                return Json(resultado,
              JsonRequestBehavior.AllowGet);
            }
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

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
