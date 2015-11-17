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
        List<ProductoCotizacion> ListaMateriales = new List<ProductoCotizacion>();

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
                               where s.Estado=="C"|| s.Estado=="A"
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
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.ColoresVidrio=new SelectList((from s in db.ColorMat
                                                  where s.IdCatMaterial == 3
                                                  select new
                                                  {
                                                      IdColor = s.IdColor,
                                                      Nombre = s.Nombre
                                                  }), "IdColor", "Nombre");
            ViewData["ColoresPaleta"] = db.ColorMat.Where(s => s.IdCatMaterial == 3).ToList();
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
         
            return View(cotizacion);
        }

        // GET: Cotizacion/Edit/5
        public ActionResult Editar(int? id)
        {
            
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
           
            ViewBag.ColoresVidrio = new SelectList((from s in db.ColorMat
                                                    where s.IdCatMaterial == 3
                                                    select new
                                                    {
                                                        IdColor = s.IdColor,
                                                        Nombre = s.Nombre
                                                    }), "IdColor", "Nombre");
            ViewData["ColoresPaleta"] = db.ColorMat.Where(s => s.IdCatMaterial == 3).ToList();

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
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }

            Calculos C = new Calculos();
           var ListaProdu=(from s in db.ProductoCotizacion
                           where s.IdCotizacion==id
                            select new 
                            { 
                                IdProducto=s.IdProducto,
                                Nombre=s.Producto.Nombre,
                                Cantidad=s.CantProducto                       
                                
                            }).ToList();
            List<Producto> Lista = new List<Producto>();
            foreach (var item in ListaProdu)
            {
                Producto p = new Producto();
                p.IdProducto = item.IdProducto;
                p.Nombre = item.Nombre;
                p.Cantidad = item.Cantidad;
                if (!Lista.Any(s=>s.IdProducto==p.IdProducto)) {
                    Lista.Add(p);
                }
               
            }
            decimal? anchocelocia = null;
            int? ColorPaleta = null;
            int vidrio = 0;
            int Idpro = 0;
            int Cant = 0;
            int ColorVidrio = 0;
            int ColorAluminio = 0;
            decimal instalacion = 0;
            decimal ancho = 0;
            decimal alto = 0;
            foreach(var itemProduct in Lista)
            {
                var ListaMat = (from s in db.ProductoCotizacion
                                   where s.IdCotizacion == id&& s.IdProducto==itemProduct.IdProducto
                                   select s).ToList();
                foreach (var item in ListaMat)
                {
                    
                    if (item.Material.IdCatMat == 3 && item.Material.IdTipoMaterial == 55)
                    {
                        ColorPaleta = item.IdColorPaleta;
                        anchocelocia = item.AnchoCelocia;
                    }
                        if (item.Material.IdCatMat == 3 && item.Material.IdTipoMaterial == 53)
                    {
                        vidrio = item.IdMaterial;                  
                    }
                    Idpro = item.IdProducto;
                    Cant = item.CantProducto;
                    ColorVidrio = item.IdColorVidrio;
                    ColorAluminio = item.IdColorAluminio;
                    instalacion = item.Instalacion;
                    ancho = item.Ancho;
                    alto = item.Alto;
                }
                
                TempData["MateralCotizacion"] = C.calcularMonto(Idpro, ColorVidrio, anchocelocia, ColorAluminio, instalacion, Cant, ancho, alto, vidrio, ColorPaleta);
                ListaMateriales.AddRange((List<ProductoCotizacion>)TempData["MateralCotizacion"]);
                foreach(var item in ListaMateriales)
                {
                    if(itemProduct.IdProducto==item.IdProducto)
                    itemProduct.Subtotal += (item.Subtotal * (1 + item.Instalacion));
                }
                itemProduct.Subtotal = (itemProduct.Subtotal * itemProduct.Cantidad);
                ListaProductos.Add(itemProduct);

            }
           
            TempData["MateralCotizacion"] = ListaMateriales;
            TempData["ListaProductos"] = ListaProductos;
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
        public JsonResult AgregarProducto(int Idpro, int Cvidrio,decimal? anchocelocia, int CAluminio, int Insta, int Cant, decimal Ancho, decimal Alto, int vidrio, int? ColorPaleta)
        {
            if(TempData["MateralCotizacion"]!= null)
            {
                ListaMateriales=(List<ProductoCotizacion>)TempData["MateralCotizacion"];
            }
           
            Calculos C = new Calculos();

            var resultado = "No se pudo agregar el Producto";
            var ValidarMat = C.ValidarMateriales(Idpro, Cvidrio, CAluminio, vidrio,ColorPaleta);
            decimal instalacion = db.Valor.Find(Insta).Porcentaje;
            if (ValidarMat.Count()==0)
            {
                TempData["MateralCotizacion"] = C.calcularMonto(Idpro, Cvidrio, anchocelocia, CAluminio, instalacion, Cant, Ancho, Alto, vidrio, ColorPaleta);
                ListaMateriales.AddRange((List<ProductoCotizacion>)TempData["MateralCotizacion"]);
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

                foreach (var item in ListaMateriales)
                {
                    if (Idpro == item.IdProducto)
                        Produ.Subtotal += (item.Subtotal* (1 + item.Instalacion));
                }
                Produ.Subtotal = Produ.Subtotal * Cant;
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
            TempData["MateralCotizacion"] = ListaMateriales;
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult EliminarProducto(int id)
        {
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
                ListaMateriales = (List<ProductoCotizacion>) TempData["MateralCotizacion"];
            }
            try
            {
                foreach (Producto listProduct in ListaProductos)
                {
                    if (listProduct.IdProducto == id)
                    {
                        ListaProductos.Remove(listProduct);
                        for(int i = 0; i < ListaMateriales.Count(); i++)
                        {
                            if (ListaMateriales[i].IdProducto == id)
                            {
                                ListaMateriales.RemoveAt(i);
                                i--;
                            }
                        }
                        break;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo borrar la linea, y si el problema persiste contacte el administrador del sistema.");
            }
            TempData["MateralCotizacion"] = ListaMateriales;
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult CalcularTotal()
        {
            decimal Total = 0;
            try
            {
                
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
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

            TempData["ListaProductos"] = ListaProductos;
            return Json(Total,
             JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarImagen(int id)
        {
            string imagen = "";
            try { 
             imagen =Convert.ToBase64String(db.Producto.Find(id).Imagen);
   
            }
            catch (Exception e)
            {
                var resultado = "Un ocurrido un error Inesperado. Contacte al administrador del sistema \n\n" + e;
                return Json(resultado,
                  JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult ColsultarVidrio(int id)
        {
           var vidrio=(from s in db.Material
                                              where s.IdCatMat == 3 && s.IdSubCatMat== id
                                      select new
                                              {
                                                  s.IdMaterial,
                                                  s.Nombre
                                              }).ToList();
            return Json(vidrio,
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarClientes(string filtro)
        {
                var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro)
                            select new
                            { s.IdCliente,
                              s.Nombre,
                              s.Telefono,
                              s.Correo
                            }).Take(5);

                return Json(Clientes,
                 JsonRequestBehavior.AllowGet);        
       
        }
        public JsonResult VerificarAtributos(int id)
        {
            string resultado = "";
            var product = db.Producto.Find(id);
            if (product.Forma == "CF" || product.Forma == "FCF"||product.Forma=="COF")
            {
                resultado = "Celocia";
            }
            return Json(resultado,
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult GuardarCliente(string Nombre, string Telefono, string Correo, string Direccion)
        {
            Cliente C1 = new Cliente();
            try {
            
            C1.Nombre = Nombre;
            C1.Telefono = Telefono;
            C1.Correo = Correo;
            C1.Direccion = Direccion;
            C1.Estado = 1;
            C1.Usuario = Session["UsuarioActual"].ToString();
                db.Cliente.Add(C1);
            db.SaveChanges();
            }
            catch(Exception e)
            {
                var resultado = "Un ocurrido un error Inesperado. Contacte al administrador del sistema \n\n" + e;
                return Json(resultado,
                  JsonRequestBehavior.AllowGet);
            }
            var Cliente = new { IdCliente = C1.IdCliente, Nombre = Nombre };
            return Json(Cliente,
              JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarListaProductos()
        {
            if(TempData["ListaProductos"] !=null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
            }
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos,
         JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProcesarCotizacion(int IdCliente, string Comentario)
        {
            var respuesta = "Cotizacion Procesada!";
            try { 
           
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<Producto>)TempData["ListaProductos"];
                ListaMateriales= (List<ProductoCotizacion>)TempData["MateralCotizacion"];
                Cotizacion Cot = new Cotizacion();
                Cot.IdCliente = IdCliente;
                Cot.Estado = "P";
                Cot.Fecha = DateTime.Now;
                Cot.FechaActualizacion = DateTime.Now;
                Cot.Comentario = Comentario;
                
                foreach (var item in ListaProductos)
                {
                    Cot.MontoParcial += item.Subtotal;
                }
                Cot.Usuario= Session["UsuarioActual"].ToString();
                    db.Cotizacion.Add(Cot);
                    db.SaveChanges();

                    foreach (var item in ListaMateriales)
                {
                    item.IdCotizacion = Cot.IdCotizacion;
                    db.ProductoCotizacion.Add(item);
                }
                    db.SaveChanges();
                    LimpiarListas();
                }
            else
            {
                respuesta = "Debe Agregar Productos a la Cotizacion!";
            }
            }
            catch(Exception e)
            {
                respuesta = "Un error inesperado a ocurrido, contacte al administrador del sistema\n\n" + e;
                return Json(respuesta,
          JsonRequestBehavior.AllowGet);
            }

            return Json(respuesta,
          JsonRequestBehavior.AllowGet);
        }
        public JsonResult GuardarCotizacion(int IdCliente, string Comentario)
        {
            var respuesta = "Cotizacion Guardada!";
            try
            {

                if (TempData["ListaProductos"] != null)
                {
                    ListaProductos = (List<Producto>)TempData["ListaProductos"];
                    ListaMateriales = (List<ProductoCotizacion>)TempData["MateralCotizacion"];
                    Cotizacion Cot = new Cotizacion();
                    Cot.IdCliente = IdCliente;
                    Cot.Estado = "C";
                    Cot.Fecha = DateTime.Now;
                    Cot.FechaActualizacion = DateTime.Now;
                    Cot.Comentario = Comentario;

                    foreach (var item in ListaProductos)
                    {
                        Cot.MontoParcial += item.Subtotal;
                    }
                    Cot.Usuario = Session["UsuarioActual"].ToString();
                    db.Cotizacion.Add(Cot);
                    db.SaveChanges();

                    foreach (var item in ListaMateriales)
                    {
                        item.IdCotizacion = Cot.IdCotizacion;
                        db.ProductoCotizacion.Add(item);
                    }
                    db.SaveChanges();
                    LimpiarListas();
                }
                else
                {
                    respuesta = "Debe Agregar Productos a la Cotizacion!";
                }
            }
            catch(Exception e)
            {
                respuesta = "Un error inesperado a ocurrido, contacte al administrador del sistema\n\n" + e;
                return Json(respuesta,
          JsonRequestBehavior.AllowGet);
            }

            return Json(respuesta,
          JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProcesarCotizacionEdit(int id,int IdCliente, string Comentario)
        {
            var respuesta = "Cotizacion Procesada!";
            try
            {

                if (TempData["ListaProductos"] != null)
                {
                    ListaProductos = (List<Producto>)TempData["ListaProductos"];
                    ListaMateriales = (from s in db.ProductoCotizacion
                                       where s.IdCotizacion == id
                                       select s).ToList();
                    Cotizacion Cotiz = db.Cotizacion.Find(id);
                    Cotiz.IdCliente = IdCliente;
                    Cotiz.Estado = "P";
                    Cotiz.FechaActualizacion = DateTime.Now;
                    Cotiz.Comentario = Comentario;
                    Cotiz.MontoParcial = 0;
                    foreach (var item in ListaProductos)
                    {
                        Cotiz.MontoParcial += item.Subtotal;
                    }
                    Cotiz.Usuario = Session["UsuarioActual"].ToString();
                    db.Entry(Cotiz).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (var item in ListaMateriales)
                    {
                        db.ProductoCotizacion.Attach(item);
                        db.ProductoCotizacion.Remove(item);
                        db.SaveChanges();
                    }

                    foreach (var item in (List<ProductoCotizacion>)TempData["MateralCotizacion"])
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        db.ProductoCotizacion.Add(item);
                    }
                    db.SaveChanges();
                    LimpiarListas();
                }
                else
                {
                    respuesta = "Debe Agregar Productos a la Cotizacion!";
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
        public JsonResult GuardarCotizacionEdit(int id,int IdCliente, string Comentario)
        {
            var respuesta = "Cotizacion Guardada!";
            try
            {

                if (TempData["ListaProductos"] != null)
                {
                    ListaProductos = (List<Producto>)TempData["ListaProductos"];
                    ListaMateriales = (from s in db.ProductoCotizacion
                                       where s.IdCotizacion == id
                                       select s).ToList();
                    Cotizacion Cotiz = db.Cotizacion.Find(id);
                    Cotiz.IdCliente = IdCliente;
                    Cotiz.Estado = "C";
                    Cotiz.FechaActualizacion = DateTime.Now;
                    Cotiz.Comentario = Comentario;
                    Cotiz.MontoParcial = 0;
                    foreach (var item in ListaProductos)
                    {
                        Cotiz.MontoParcial += item.Subtotal;
                    }
                    Cotiz.Usuario = Session["UsuarioActual"].ToString();
                    db.Entry(Cotiz).State = EntityState.Modified;
                    db.SaveChanges();

                    foreach (var item in ListaMateriales)
                    {
                        db.ProductoCotizacion.Attach(item);
                        db.ProductoCotizacion.Remove(item);
                        db.SaveChanges();
                    }

                    foreach (var item in (List<ProductoCotizacion>)TempData["MateralCotizacion"])
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        db.ProductoCotizacion.Add(item);
                    }
                    db.SaveChanges();
                    LimpiarListas();
                }
                else
                {
                    respuesta = "Debe Agregar Productos a la Cotizacion!";
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
        public void LimpiarListas()
        {
            TempData["MateralCotizacion"] = null;
            TempData["ListaProductos"] = null;
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
