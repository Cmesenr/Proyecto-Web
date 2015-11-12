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
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.IdVidrio = new SelectList(from s in db.Material
                                              where s.IdCatMat == 3
                                              select new
                                              {
                                                  s.IdMaterial,
                                                  s.Nombre
                                              }, "IdMaterial", "Nombre");
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
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.IdVidrio = new SelectList(from s in db.Material
                                              where s.IdCatMat == 3
                                              select new
                                              {
                                                  s.IdMaterial,
                                                  s.Nombre
                                              },"IdMaterial","Nombre");
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
        public JsonResult AgregarProducto(int Idpro, int Cvidrio, int CAluminio, int Insta, int Cant, decimal Ancho, decimal Alto, int vidrio)
        {
            Calculos C = new Calculos();
            var resultado = "No se pudo agregar el Producto";
            decimal instalacion = db.Valor.Find(Insta).Porcentaje;
            var ValidarMat = C.ValidarMateriales(Idpro, Cvidrio, CAluminio, vidrio);
            if (ValidarMat.Count()==0)
            {
                TempData["MateralCotizacion"] = C.calcularMonto(Idpro, Cvidrio, CAluminio, Insta, Cant, Ancho, Alto, vidrio);
            }
            else
            {
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
                Producto Produ = new Producto();
                Produ.IdProducto = Idpro;
                Produ.Nombre = ListPro.Nombre;
                Produ.Cantidad = Cant;

                foreach (var item in (List<ProductoCotizacion>)TempData["MateralCotizacion"])
                {
                    if (Idpro == item.IdProducto)
                        Produ.Subtotal += item.Subtotal;
                }
                Produ.Subtotal = ((Produ.Subtotal) * (1 + instalacion)) * Cant;
                if (ListaProductos.Count() == 0) { ListaProductos.Add(Produ); }
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
                    ListaProductos.Add(Produ);
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
        public JsonResult EliminarProducto(int id)
        {
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
            }
            try
            {
                foreach (Producto listProduct in ListaProductos)
                {
                    if (listProduct.IdProducto == id)
                    {
                        ListaProductos.Remove(listProduct);
                        break;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo borrar la linea, y si el problema persiste contacte el administrador del sistema.");
            }
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult CalcularTotal()
        {
            decimal Total=0;
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
                foreach (var item in ListaProductos)
                {
                    Total += item.Subtotal;
                }

            }

            TempData["ListaProductos"] = ListaProductos;
            return Json(Total,
             JsonRequestBehavior.AllowGet);

        }
        public JsonResult ConsultarImagen(int id)
        {
            string imagen =Convert.ToBase64String(db.Producto.Find(id).Imagen);
            return Json(imagen,
             JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarProductos(int id)
        {
            var Productos = (from s in db.Producto
                                where s.IdTipoProducto == id
                                select new
                                {
                                    IdProducto = s.IdProducto,
                                    Nombre = s.Nombre
                                }).ToList();


            return Json(Productos,
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarClientes(string filtro)
        {
            var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro)
                            select s).Take(5);
            
            return Json(Clientes,
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
