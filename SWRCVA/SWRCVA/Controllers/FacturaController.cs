﻿using System;
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
            if (id != null)
            {
                Cotizacion cotizacion = db.Cotizacion.Find(id);
                if (cotizacion == null)
                {
                    return HttpNotFound();
                }
                ViewData["Cliente"] = cotizacion.Cliente.Nombre;
                var ListaProdu = (from s in db.ProductoCotizacion
                                  where s.IdCotizacion == id
                                  select s).ToList();
                List<ProductoCotizacion> ListaP = new List<ProductoCotizacion>();
                foreach (var item in ListaProdu)
                {
                    ProductoCotizacion p = new ProductoCotizacion();
                    p.IdProducto = item.IdProducto;
                    p.Nombre = item.Producto.Nombre;
                    p.CantMat = item.CantProducto;
                    p.Subtotal = item.Subtotal;
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
                Material ListMat = db.Material.Find(Idpro);
                ProductoCotizacion Produ = new ProductoCotizacion();
                Produ.IdProducto = Idpro;
                Produ.Nombre = ListMat.Nombre;
                Produ.CantMat = Cant;
                switch (ListMat.IdCatMat)
                {
                    case 1:
                        {
                            Produ.Subtotal =((costo * IV) * 1.5m);
                            break;
                        }
                    case 2:
                        {
                            Produ.Subtotal = ((costo * IV) * 1.5m);
                            break;
                        }
                    case 3:
                        {
                            if(ListMat.IdTipoMaterial== 55)
                            {
                                Produ.Subtotal = (decimal)Ancho *((costo * IV) * 1.5m);
                            }
                            else
                            {
                                Produ.Subtotal = ((decimal)Ancho * (decimal)Alto) * ((costo * IV) * 1.5m);
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
            if (TempData["ListaProductosFact"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductosFact"];
            }
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
        public JsonResult ProcesarFactura(int IdCliente, int IdCotizacion, decimal MontoPagar, decimal Montototal)
        {
            var respuesta = "Factura Registrada!";
            try
            {

                if (TempData["ListaProductosFact"] != null)
                {
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                    Factura Fact = new Factura();
                    Fact.IdCliente = IdCliente;
                    Fact.IdCotizacion = IdCotizacion;
                    Fact.FechaHora = DateTime.Now;
                    Fact.Usuario = Session["UsuarioActual"].ToString();
                    Fact.Estado=1;
                    Fact.MontoTotal = Montototal;
                    Fact.MontoPagar = MontoPagar;
                    db.Factura.Add(Fact);
                    db.SaveChanges();

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
                    LimpiarListas();
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
