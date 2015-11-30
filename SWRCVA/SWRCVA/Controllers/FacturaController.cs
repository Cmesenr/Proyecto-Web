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
using System.Globalization;

namespace SWRCVA.Controllers
{
    public class FacturaController : Controller
    {
        private DataContext db = new DataContext();
        List<ProductoCotizacion> ListaProductos = new List<ProductoCotizacion>();
        // GET: Factura
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

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
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            if (id != null)
            {
                Cotizacion cotizacion = db.Cotizacion.Find(id);
                if (cotizacion.Estado != "T") { cotizacion = null; }
                
                if (cotizacion == null)
                {
                    return HttpNotFound();
                }
                ViewData["Cliente"] = cotizacion.Cliente.Nombre;
                ViewData["IdCliente"] = cotizacion.Cliente.IdCliente;
                ViewData["IdCotizacion"] = cotizacion.IdCotizacion;
                var ListaProdu = (from s in db.ProductoCotizacion
                                  where s.IdCotizacion == id
                                  select s).ToList();
                var ListaPM = (from s in db.MaterialItemCotizacion
                               where s.IdCotizacion == id
                               select s).ToList();
                List<ProductoCotizacion> ListaP = new List<ProductoCotizacion>();
                foreach (var item in ListaProdu)
                {
                    ProductoCotizacion p = new ProductoCotizacion();
                    p.IdProducto = item.IdProducto;
                    p.Nombre = item.Producto.Nombre;
                    p.CantMat = item.CantProducto;
                    p.Subtotal = Math.Round((Decimal)item.Subtotal, 2);
                    ListaP.Add(p);
                }

                foreach (var item in ListaPM)
                {
                    ProductoCotizacion p = new ProductoCotizacion();
                    p.IdProducto = item.IdMaterial;
                    p.Nombre = item.Material.Nombre;
                    p.CantMat = item.Cantidad;
                    p.Subtotal = Math.Round((Decimal)item.Subtotal, 2);
                    ListaP.Add(p);
                }

                foreach (var itemProduct in ListaP)
                {
                    ListaProductos.Add(itemProduct);
                }
                TempData["ListaProductosFact"] = ListaProductos;
            }
            return View();
        }

        public ActionResult Ticket()
        {
            if (TempData["ListaProductosFact"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
            }
            ViewData["IdFactura"] = TempData["IdFactura"];
            ViewData["Total"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", TempData["Total"]);
            ViewData["ListaPro"] = ListaProductos;
            TempData["ListaProductosFact"] = ListaProductos;
            ViewData["fecha"] = TempData["Fecha"];
            ViewData["Cliente"] = TempData["Cliente"];
            LimpiarListas();
            return View();
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFactura,FechaHora,MontoTotal,MontoPagar,Usuario,IdCliente,Estado")] Factura factura)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

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
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

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
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            Factura factura = db.Factura.Find(id);
            db.Factura.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult AgregarProducto(int Idpro, decimal Cant,decimal costo, decimal? Extra,decimal? Ancho, decimal? Alto)
        {
            var resultado = "Error al intentar agregar el producto";
            if (TempData["ListaProductosFact"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
            }
            try
            {               
                decimal IV = 1 + db.Valor.Find(2).Porcentaje;
                decimal Cargo = 1 + db.Valor.Find(5).Porcentaje;
                Material ListMat = db.Material.Find(Idpro);
                ProductoCotizacion Produ = new ProductoCotizacion();
                Produ.IdProducto = Idpro;
                Produ.Nombre = ListMat.Nombre;
                Produ.CantMat = Cant;
                switch (ListMat.IdCatMat)
                {
                    case 1:
                        {
                            Produ.Subtotal =((costo * IV) * Cargo);
                            break;
                        }
                    case 2:
                        {
                            Produ.Subtotal = ((costo * IV) * Cargo);
                            break;
                        }
                    case 3:
                        {
                            if (ListMat.IdTipoMaterial == 55)
                            {
                                Produ.Subtotal = (decimal)Ancho * ((costo * IV) * Cargo);
                            }
                            else if (ListMat.IdTipoMaterial == 53)
                            {
                                Produ.Subtotal = ((decimal)Ancho * (decimal)Alto) * ((costo * IV) * Cargo);
                            }
                            else
                            {
                                Produ.Subtotal = ((costo * IV) * Cargo);
                            }
                            break;
                        }
                }

                if (Extra != null)
                {
                    Extra = (Extra / 100m)+1;
                    Produ.Subtotal = (Produ.Subtotal * Cant) *(decimal)Extra;
                }
                else
                {
                    Produ.Subtotal = Produ.Subtotal* Cant;
                }
                Produ.Subtotal = Math.Round((Decimal)Produ.Subtotal, 2);
                if (ListaProductos.Count() == 0) { ListaProductos.Add(Produ); }
                else
                {
                    foreach (ProductoCotizacion listProduct in ListaProductos)
                    {
                        if (listProduct.IdProducto == Idpro)
                        {
                            TempData["ListaProductosFact"] = ListaProductos;
                            resultado = "No se puede duplicar el El producto!";
                            return Json(resultado,
                            JsonRequestBehavior.AllowGet);
                        }

                    }
                    ListaProductos.Add(Produ);
                }
            }
            catch(Exception e)
            {
                return Json(resultado,
              JsonRequestBehavior.AllowGet);
            }
            TempData["ListaProductosFact"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult EliminarProducto(int id)
        {

                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
            try
            {
                foreach (ProductoCotizacion listProduct in ListaProductos)
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
            TempData["ListaProductosFact"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult ProcesarFactura(int IdCliente, int? IdCotizacion, decimal MontoPagar, decimal Montototal)
        {
            var respuesta = "Factura Registrada!";
            try
            {
                if (TempData["ListaProductosFact"] != null)
                {
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
                }
               
                if (ListaProductos.Count()>0)
                {                   
                    Factura Fact = new Factura();
                    Fact.IdCliente = IdCliente;
                    if (IdCotizacion != null)
                    {
                        Cotizacion Cotiz = db.Cotizacion.Find(IdCotizacion);
                        Cotiz.Estado = "F";
                        Cotiz.FechaActualizacion = DateTime.Now;
                        db.Entry(Cotiz).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Fact.IdCotizacion = null;
                    }

                    Fact.FechaHora = DateTime.Now;
                    Fact.Usuario = Session["UsuarioActual"].ToString();
                    Fact.Estado=1;
                    Fact.MontoTotal = Montototal;
                    Fact.MontoPagar = MontoPagar;
                    db.Factura.Add(Fact);
                    db.SaveChanges();
                    TempData["Fecha"] = Fact.FechaHora;
                    TempData["Cliente"] =db.Cliente.Find(Fact.IdCliente).Nombre;
                    foreach (var item in ListaProductos)
                    {
                        DetalleFactura D1 = new DetalleFactura();
                        D1.IdFactura = Fact.IdFactura;
                        D1.IdProducto = item.IdProducto;
                        D1.MontoParcial = item.Subtotal;
                        D1.Cantidad = item.CantMat;
                        db.DetalleFactura.Add(D1);
                    }
                    db.SaveChanges();
                    TempData["IdFactura"] = Fact.IdFactura;
                }
                else
                {
                    respuesta = "Debe Agregar Productos a la Factura!";
                }
            }
            catch (Exception e)
            {
                respuesta = "Un error inesperado a ocurrido, contacte al administrador del sistema\n\n" + e;
                return Json(respuesta,
          JsonRequestBehavior.AllowGet);
            }
            TempData["ListaProductosFact"] = ListaProductos;
            return Json(respuesta,
         JsonRequestBehavior.AllowGet);
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
        public JsonResult ConsultarMateriales()
        {
            var Aluminios = (from s in db.Material
                             join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                             where s.IdCatMat == 2 
                             select new
                             {
                                 Id = s.IdMaterial,
                                 Nombre = s.Nombre,
                                 Categoria = s.CategoriaMat.Nombre,
                                 Color = c.ColorMat.Nombre,
                                 Costo = c.Costo
                             }).ToList();

            var Vidrio = (from s in db.Material
                          join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                          where s.IdCatMat == 3
                          select new
                          {
                             Id= s.IdMaterial,
                             Nombre= s.Nombre,
                              Categoria = s.CategoriaMat.Nombre,
                              Color = c.ColorMat.Nombre,
                              Costo=c.Costo
                          }).ToList();
            var Acesorios = (from s in db.Material
                             where s.IdCatMat == 1
                             select new
                             {
                                 Id = s.IdMaterial,
                                 Nombre = s.Nombre,
                                 Categoria = s.CategoriaMat.Nombre,
                                 Color = "n/a",
                                 Costo = s.Costo
                             }).ToList();

            var materiales = Aluminios;
            materiales.AddRange(Vidrio);
            materiales.AddRange(Acesorios);
            return Json(materiales,
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
            TempData["Total"] = Total;
            TempData["ListaProductosFact"] = ListaProductos;
            return Json(Total,
             JsonRequestBehavior.AllowGet);
        }
        public JsonResult VerificarMaterial(int id)
        {
            string resultado = "";
            var mat = db.Material.Find(id);
            if (mat.IdCatMat == 3)
            {
                if (mat.IdTipoMaterial == 53)
                {
                    resultado = "Vidrio";
                }
                else if (mat.IdTipoMaterial == 55)
                {
                    resultado = "Paleta";
                }
                else
                {
                    resultado = "Material";
                }
            }
            else
            {
                resultado = "Material";
            }
            return Json(resultado,
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
        
        public JsonResult ConsultarListaProductosTicket()
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
            TempData["IdFactura"] = null;
            TempData["Total"] = null;
            TempData["Fecha"] = null;
            TempData["Cliente"] = null;
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
