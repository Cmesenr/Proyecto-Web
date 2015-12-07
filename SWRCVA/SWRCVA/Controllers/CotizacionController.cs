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
    public class CotizacionController : Controller
    {
        private DataContext db = new DataContext();
        List<ProductoCotizacion> ListaProductos = new List<ProductoCotizacion>();
        List<MaterialCotizacion> ListaMateriales = new List<MaterialCotizacion>();

        // GET: Cotizacion
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
                               where s.Estado=="C"|| s.Estado=="A"
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
                    cotizaciones = cotizaciones.OrderBy(s => s.Cliente.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(cotizaciones.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Recibo(string sortOrder, string currentFilter, string searchString, int? page)
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
                               where s.Estado == "P"
                               select s;
            foreach(var item in cotizaciones)
            {
                var recibo = db.ReciboDinero.Where(s => s.IdCotizacion == item.IdCotizacion);
                if (recibo!=null)
                {
                    foreach (var item2 in recibo){
                        item.MontoParcial -= item2.Monto;
                    }
                   
                }
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                cotizaciones = cotizaciones.Where(s => s.Cliente.Nombre.Contains(searchString)||
                                                    s.IdCotizacion.ToString().Contains(searchString)
                                                   );
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
        // GET: Cotizacion/Create
        public ActionResult Cotizar()
        {
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.IdTipoProducto = new SelectList((from s in db.TipoProducto
                                                     where s.Estado == 1
                                                     select new
                                                     {
                                                         s.IdTipoProducto,
                                                         s.Nombre
                                                     }), "IdTipoProducto", "Nombre");
            ViewBag.Paletas=new SelectList((from s in db.Material
                                            where s.IdCatMat == 3 && s.IdTipoMaterial == 55&& s.Estado==1
                                            select new
                                            {
                                                s.IdMaterial,
                                                s.Nombre
                                            }), "IdMaterial", "Nombre");
            ViewBag.ColoresVidrio=new SelectList((from s in db.ColorMat
                                                  where s.IdCatMaterial == 3&& s.Estado==1
                                                  select new
                                                  {
                                                      IdColor = s.IdColor,
                                                      Nombre = s.Nombre
                                                  }), "IdColor", "Nombre");
            ViewData["ColoresPaleta"] = db.ColorMat.Where(s => s.IdCatMaterial == 3&&s.Estado==1).ToList();
            ViewBag.ColoresAluminio = new SelectList((from s in db.ColorMat
                                                    where s.IdCatMaterial == 2 &&s.Estado==1 
                                                    select new
                                                    {
                                                        IdColor = s.IdColor,
                                                        Nombre = s.Nombre
                                                    }), "IdColor", "Nombre");
            ViewBag.Instalacion = new SelectList((from s in db.Valor
                                                  where s.Tipo == "I" && s.Estado==1
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
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            return View(cotizacion);
        }

        // GET: Cotizacion/Edit/5
        public ActionResult Editar(int? id)
        {
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.IdTipoProducto = new SelectList((from s in db.TipoProducto
                                                     where s.Estado == 1
                                                     select new
                                                     {
                                                         s.IdTipoProducto,
                                                         s.Nombre
                                                     }), "IdTipoProducto", "Nombre");
            ViewBag.Paletas = new SelectList((from s in db.Material
                                              where s.IdCatMat == 3 && s.IdTipoMaterial == 55 && s.Estado == 1
                                              select new
                                              {
                                                  s.IdMaterial,
                                                  s.Nombre
                                              }), "IdMaterial", "Nombre");
            ViewBag.ColoresVidrio = new SelectList((from s in db.ColorMat
                                                    where s.IdCatMaterial == 3 && s.Estado == 1
                                                    select new
                                                    {
                                                        IdColor = s.IdColor,
                                                        Nombre = s.Nombre
                                                    }), "IdColor", "Nombre");
            ViewData["ColoresPaleta"] = db.ColorMat.Where(s => s.IdCatMaterial == 3 && s.Estado == 1).ToList();
            ViewBag.ColoresAluminio = new SelectList((from s in db.ColorMat
                                                      where s.IdCatMaterial == 2 && s.Estado == 1
                                                      select new
                                                      {
                                                          IdColor = s.IdColor,
                                                          Nombre = s.Nombre
                                                      }), "IdColor", "Nombre");
            ViewBag.Instalacion = new SelectList((from s in db.Valor
                                                  where s.Tipo == "I" && s.Estado == 1
                                                  select new
                                                  {
                                                      IdValor = s.IdValor,
                                                      Nombre = s.Nombre
                                                  }), "IdValor", "Nombre");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }

           var ListaProdu=(from s in db.ProductoCotizacion
                           where s.IdCotizacion==id
                            select s).ToList();
            var ListaProduMat = (from s in db.MaterialItemCotizacion
                              where s.IdCotizacion == id
                              select s).ToList();
            var listaMate= (from s in db.MaterialCotizacion
                            where s.IdCotizacion == id
                            select s).ToList();
            List<ProductoCotizacion> ListaP = new List<ProductoCotizacion>();
            foreach (var item in ListaProduMat)
            {
                ProductoCotizacion p = new ProductoCotizacion();
                p.IdConsecutivo = item.IdConsecutivo;
                p.IdProducto = item.IdMaterial;
                p.Nombre = item.Material.Nombre;
                p.CantMat = item.Cantidad;
                if (item.Ancho != null)
                {
                    p.Ancho = (decimal)item.Ancho;
                }
                if (item.Alto != null)
                {
                    p.Alto = (decimal)item.Alto;
                }
                p.Tipo = "Mat";
                p.IdColor = item.IdColor;
                p.Subtotal = item.Subtotal;

                ListaP.Add(p);
            }
            foreach (var item in ListaProdu)
            {
                ProductoCotizacion p = new ProductoCotizacion();
                p.IdConsecutivo = item.IdConsecutivo;
                p.IdProducto = item.IdProducto;
                p.Nombre = item.Producto.Nombre;
                p.CantMat = item.CantProducto;
                p.IdColorPaleta = item.IdColorPaleta;
                p.AnchoCelocia = item.AnchoCelocia;
                p.IdColorVidrio = item.IdColorVidrio;
                p.IdColorAluminio = item.IdColorAluminio;
                p.Instalacion = item.Instalacion;
                p.Ancho = item.Ancho;
                p.Alto = item.Alto;
                p.Tipo = "Prod";
                p.Subtotal = item.Subtotal;
                ListaP.Add(p);
            }
            List<MaterialCotizacion> ListaM = new List<MaterialCotizacion>();
            foreach (var item in listaMate)
            {
                MaterialCotizacion p = new MaterialCotizacion();
                p.IdProducto = item.IdProducto;
                p.IdMaterial = item.IdMaterial;
                p.CantMaterial = item.CantMaterial;
                p.Subtotal = item.Subtotal;
                ListaM.Add(p);
            }

            foreach (var itemProduct in ListaP)
            {
                ListaProductos.Add(itemProduct);
            }
            foreach (var itemMat in ListaM)
            {
                ListaMateriales.Add(itemMat);
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

            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

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
    
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ListaP = (from s in db.ProductoCotizacion
                          where s.IdCotizacion == id
                          select s).ToList();
            var ListaPM = (from s in db.MaterialItemCotizacion
                           where s.IdCotizacion == id
                           select s).ToList();
            var ListaM = (from s in db.MaterialCotizacion
                          where s.IdCotizacion == id
                          select s).ToList();

            foreach (var item in ListaM)
            {
                db.MaterialCotizacion.Attach(item);
                db.MaterialCotizacion.Remove(item);
                db.SaveChanges();
            }

            foreach (var item in ListaP)
            {

                db.ProductoCotizacion.Attach(item);
                db.ProductoCotizacion.Remove(item);
                db.SaveChanges();

            }
            foreach (var item in ListaPM)
            {
                db.MaterialItemCotizacion.Attach(item);
                db.MaterialItemCotizacion.Remove(item);
                db.SaveChanges();

            }
            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            db.Cotizacion.Remove(cotizacion);
            db.SaveChanges();
           
            return RedirectToAction("Index");
        }

        public ActionResult Plaforma(int id)
        {
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            ViewData["fecha"] = cotizacion.Fecha;
            ViewData["IdCot"] = cotizacion.IdCotizacion;
            ViewData["Cliente"] = cotizacion.Cliente.Nombre;
            var ListPro = (from s in db.ProductoCotizacion
                           where s.IdCotizacion == id
                           select new ListaProducto
                           {
                               Nombre = s.Producto.Nombre,
                               Cantidad = (decimal)s.CantProducto,
                               SubTotal = s.Subtotal
                           }).ToList();
            var ListProMat = (from s in db.MaterialItemCotizacion
                           where s.IdCotizacion == id
                           select new ListaProducto
                           {
                               Nombre = s.Material.Nombre,
                               Cantidad = s.Cantidad,
                               SubTotal = s.Subtotal
                           }).ToList();
            var productos = ListPro;
            productos.AddRange(ListProMat);
            ViewData["Total"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", Calculos.round5(cotizacion.MontoParcial));
            ViewData["ListaPro"] = productos;
            return View();
        }
        public ActionResult ReciboDinero(int id, decimal monto)
        {
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Cotizacion cotizacion = db.Cotizacion.Find(id);
            if (cotizacion == null)
            {
                return HttpNotFound();
            }
            ViewData["fecha"] = DateTime.Now;
            ViewData["IdCot"] = cotizacion.IdCotizacion;
            ViewData["IdRecibo"] = TempData["IdRecibo"];
            ViewData["Cliente"] = cotizacion.Cliente.Nombre;
            ViewData["Abono"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", monto);
            ViewData["MontoOriginal"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", Calculos.round5(cotizacion.MontoParcial)); 
            var montoCot = cotizacion.MontoParcial;
            var recibo = db.ReciboDinero.Where(s => s.IdCotizacion == id);
            foreach (var item in recibo)
            {
                montoCot -= item.Monto;
            }
            ViewData["Saldo"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", Calculos.round5(montoCot));
            ViewData["Total"] = string.Format(CultureInfo.InvariantCulture,
                                 "{0:0,0.00}", (Calculos.round5(montoCot) + monto));
            return View();
        }
        public JsonResult AgregarProducto(int Idpro, int Cvidrio,decimal? anchocelocia, int CAluminio, int Insta, int Cant, decimal Ancho, decimal Alto, int vidrio, int? ColorPaleta, int? IdPaleta)
        {
            if(TempData["MateralCotizacion"]!= null)
            {
                ListaMateriales=(List<MaterialCotizacion>)TempData["MateralCotizacion"];
            }
           
            Calculos C = new Calculos();

            var resultado = "No se pudo agregar el Producto";
            var ValidarMat = C.ValidarMateriales(Idpro, Cvidrio, CAluminio, vidrio,ColorPaleta, IdPaleta);
            decimal instalacion = db.Valor.Find(Insta).Porcentaje;
            if (ValidarMat.Count()==0)
            {
                TempData["MateralCotizacion"] = C.calcularMonto(Idpro, Cvidrio, anchocelocia, CAluminio, instalacion, Cant, Ancho, Alto, vidrio, ColorPaleta, IdPaleta);
                ListaMateriales.AddRange((List<MaterialCotizacion>)TempData["MateralCotizacion"]);
            }
            else
            {
                return Json(ValidarMat,
                JsonRequestBehavior.AllowGet);
            }
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];

            }
            try
            {
                Producto ListPro = db.Producto.Find(Idpro);
                ProductoCotizacion Produ = new ProductoCotizacion();
                Produ.IdProducto = Idpro;
                Produ.Nombre = ListPro.Nombre;
                Produ.Tipo = "Prod";
                Produ.CantMat = Cant;
                Produ.IdColorPaleta =ColorPaleta;
                Produ.AnchoCelocia = anchocelocia;
                Produ.IdColorVidrio =Cvidrio;
                Produ.IdColorAluminio = CAluminio;
                Produ.Instalacion = instalacion;
                Produ.Ancho =Ancho;
                Produ.Alto = Alto;
                foreach (var item in ListaMateriales)
                {
                    if (Idpro == item.IdProducto)
                        Produ.Subtotal += (item.Subtotal* (1 + instalacion));
                }
                Produ.Subtotal = Produ.Subtotal * Cant;
                ListaProductos.Add(Produ);
               
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
        public JsonResult AgregarMaterial(int Idpro, int IdColor, decimal Cant, decimal costo, decimal? Extra, decimal? Ancho, decimal? Alto)
        {
            var resultado = "Error al intentar agregar el material";
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                ListaMateriales = (List<MaterialCotizacion>)TempData["MateralCotizacion"];
            }
            try
            {
                decimal IV = 1 + db.Valor.Find(2).Porcentaje;
                decimal Cargo = 1 + db.Valor.Find(5).Porcentaje;
                Material ListMat = db.Material.Find(Idpro);
                ProductoCotizacion Produ = new ProductoCotizacion();
                Produ.IdProducto = Idpro;
                Produ.Tipo = "Mat";
                Produ.Nombre = ListMat.Nombre;
                Produ.CantMat = Cant;
                Produ.IdColor = IdColor;
                if (Ancho != null)
                {
                    Produ.Ancho = (decimal)Ancho;
                }
                if (Alto != null)
                {
                    Produ.Alto = (decimal)Alto;
                }
               
                switch (ListMat.IdCatMat)
                {
                    case 1:
                        {
                            Produ.Subtotal = ((costo * IV) * Cargo);
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
                                Produ.Subtotal = ((decimal)Ancho * (decimal)Alto)*((costo * IV) * Cargo);
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
                    Extra = (Extra / 100m) + 1;
                    Produ.Subtotal = (Produ.Subtotal * Cant) * (decimal)Extra;
                }
                else
                {
                    Produ.Subtotal = Produ.Subtotal * Cant;
                }
                Produ.Subtotal = Math.Round((Decimal)Produ.Subtotal, 2);
                ListaProductos.Add(Produ);
            }
            catch (Exception e)
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
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                ListaMateriales = (List<MaterialCotizacion>) TempData["MateralCotizacion"];
            }
            try
            {
                foreach (ProductoCotizacion listProduct in ListaProductos)
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
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
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
                                where s.IdTipoProducto == id && s.Estado==1
                                select new
                                {
                                    IdProducto = s.IdProducto,
                                    Nombre = s.Nombre
                                }).ToList();


            return Json(Productos,
               JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult ColsultarVidrio(int? id, string tipo)
        {
             
            if (id!=null && tipo =="Categoria")
            {
                var vidrio = (from s in db.Material
                              where s.IdCatMat == 3 && s.IdSubCatMat == id && s.IdTipoMaterial != 55 && s.IdTipoMaterial != 61 && s.Estado == 1
                              select new
                              {
                                  s.IdMaterial,
                                  s.Nombre
                              }).ToList();
                return Json(vidrio,
              JsonRequestBehavior.AllowGet);
            }
            else if(id==null && tipo == "Paleta")
            {
                var vidrio = (from s in db.Material
                              where s.IdCatMat == 3 && s.IdTipoMaterial == 55 && s.Estado == 1
                              select new
                              {
                                  s.IdMaterial,
                                  s.Nombre
                              }).ToList();
                return Json(vidrio,
              JsonRequestBehavior.AllowGet);
            }
            else if (id == null && tipo == "Lamina")
            {
                var vidrio = (from s in db.Material
                              where s.IdCatMat == 3 && s.IdTipoMaterial == 61 && s.Estado == 1
                              select new
                              {
                                  s.IdMaterial,
                                  s.Nombre
                              }).ToList();
                return Json(vidrio,
              JsonRequestBehavior.AllowGet);
            }
            return Json(null,
             JsonRequestBehavior.AllowGet);


        }
        public JsonResult ConsultarClientes(string filtro)
        {
                var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro) && s.Estado == 1
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
                resultado = "CV";
            }
            if (product.Forma == "CE")
            {
                resultado = "C";
            }
            if (product.Forma == "PB1"|| product.Forma == "PB2")
            {
                resultado = "PB";
            }
            return Json(resultado,
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
                }else if (mat.IdTipoMaterial == 55)
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
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
            }
            TempData["ListaProductos"] = ListaProductos;
            return Json(ListaProductos,
         JsonRequestBehavior.AllowGet);
        }
        public JsonResult Abonar(int Id, decimal monto)
        {
            var resultado = "Error, Consulte su administrador del sistema";
            ReciboDinero R1 = new ReciboDinero();
            try
            {
                var montoCot = db.Cotizacion.Find(Id).MontoParcial;
                var recibo = db.ReciboDinero.Where(s => s.IdCotizacion == Id);
                foreach (var item in recibo)
                {
                    montoCot -= item.Monto;
                }
                
                if (monto > montoCot)
                {
                    resultado = "El monto no puede ser mayor al saldo.";
                }
                else
                {
                    R1.IdCotizacion = Id;
                    R1.Monto = monto;
                    R1.Fecha = DateTime.Now.Date;
                    R1.Usuario = Session["UsuarioActual"].ToString();
                    db.ReciboDinero.Add(R1);
                    db.SaveChanges();
                    resultado = "ok";
                    TempData["IdRecibo"] = R1.Consecutivo;
                }
              

            }
            catch (Exception e)
            {
                resultado = "Un ocurrido un error Inesperado. Contacte al administrador del sistema \n\n" + e;
                return Json(resultado,
                  JsonRequestBehavior.AllowGet);
            }
            return Json(resultado,
              JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarMateriales()
        {
            var Aluminios = (from s in db.Material
                             join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                             where s.IdCatMat == 2 && s.Estado == 1
                             select new
                             {
                                 Id = s.IdMaterial,
                                 Nombre = s.Nombre,
                                 Categoria = s.CategoriaMat.Nombre,
                                 IdColor = c.ColorMat.IdColor,
                                 Color = c.ColorMat.Nombre,
                                 Costo = c.Costo
                             }).ToList();

            var Vidrio = (from s in db.Material
                          join c in db.ColorMaterial on s.IdMaterial equals c.IdMaterial
                          where s.IdCatMat == 3 && s.Estado == 1
                          select new
                          {
                              Id = s.IdMaterial,
                              Nombre = s.Nombre,
                              Categoria = s.CategoriaMat.Nombre,
                              IdColor = c.ColorMat.IdColor,
                              Color = c.ColorMat.Nombre,
                              Costo = c.Costo
                          }).ToList();
            var Acesorios = (from s in db.Material
                             where s.IdCatMat == 1 && s.Estado == 1
                             select new
                             {
                                 Id = s.IdMaterial,
                                 Nombre = s.Nombre,
                                 Categoria = s.CategoriaMat.Nombre,
                                 IdColor = 21,
                                 Color = "n/a",
                                 Costo = s.Costo
                             }).ToList();

            var materiales = Aluminios;
            materiales.AddRange(Vidrio);
            materiales.AddRange(Acesorios);
            return Json(materiales,
             JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProcesarCotizacion(int IdCliente, string Comentario)
        {
            var respuesta = "Cotizacion Procesada!";
            try { 
           
            if (TempData["ListaProductos"] != null)
            {
                ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                ListaMateriales= (List<MaterialCotizacion>)TempData["MateralCotizacion"];
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
                    db.MaterialCotizacion.Add(item);
                     }
                    foreach (var item in ListaProductos)
                    {
                        item.IdCotizacion = Cot.IdCotizacion;
                        if (item.Tipo == "Mat")
                        {
                            MaterialItemCotizacion M = new MaterialItemCotizacion();
                            M.IdCotizacion= Cot.IdCotizacion;
                            M.IdMaterial = item.IdProducto;
                            M.Cantidad = item.CantMat;
                            M.Ancho = item.Ancho;
                            M.Alto = item.Alto;
                            M.Subtotal = item.Subtotal;
                            M.IdColor = item.IdColor;
                            db.MaterialItemCotizacion.Add(M);
                        }
                        else
                        {
                            item.CantProducto = (int)item.CantMat;
                            db.ProductoCotizacion.Add(item);
                        }
                        
                       
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
          

                if (TempData["ListaProductos"] != null)
                {
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                    ListaMateriales = (List<MaterialCotizacion>)TempData["MateralCotizacion"];
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
                        db.MaterialCotizacion.Add(item);
                }
                db.SaveChanges();
                foreach (var item in ListaProductos)
                {
                    item.IdCotizacion = Cot.IdCotizacion;
                    if (item.Tipo == "Mat")
                    {
                        MaterialItemCotizacion M = new MaterialItemCotizacion();
                        M.IdCotizacion = Cot.IdCotizacion;
                        M.IdMaterial = item.IdProducto;
                        M.Cantidad = item.CantMat;
                        M.Ancho = item.Ancho;
                        M.Alto = item.Alto;
                        M.IdColor = item.IdColor;
                        M.Subtotal = item.Subtotal;
                        db.MaterialItemCotizacion.Add(M);
                    }
                    else
                    {
                        item.CantProducto = (int)item.CantMat;
                        db.ProductoCotizacion.Add(item);
                    }

                }
                db.SaveChanges();
                LimpiarListas();
                }
                else
                {
                    respuesta = "Debe Agregar Productos a la Cotizacion!";
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
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                    ListaMateriales = (List<MaterialCotizacion>)TempData["MateralCotizacion"];

                    var ListaP = (from s in db.ProductoCotizacion
                                  where s.IdCotizacion == id
                                  select s).ToList();
                    var ListaPM = (from s in db.MaterialItemCotizacion
                                   where s.IdCotizacion == id
                                   select s).ToList();
                    var ListaM = (from s in db.MaterialCotizacion
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

                    foreach (var item in ListaM)
                    {
                        db.MaterialCotizacion.Attach(item);
                        db.MaterialCotizacion.Remove(item);
                        db.SaveChanges();
                    }
                   
                    foreach (var item in ListaP)
                    {

                            db.ProductoCotizacion.Attach(item);
                            db.ProductoCotizacion.Remove(item);
                            db.SaveChanges();
                     
                    }
                    foreach (var item in ListaPM)
                    {
                        db.MaterialItemCotizacion.Attach(item);
                        db.MaterialItemCotizacion.Remove(item);
                        db.SaveChanges();

                    }
                    foreach (var item in ListaMateriales)
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        db.MaterialCotizacion.Add(item);
                    }
                    foreach (var item in ListaProductos)
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        if (item.Tipo == "Mat")
                        {
                            MaterialItemCotizacion M = new MaterialItemCotizacion();
                            M.IdCotizacion = Cotiz.IdCotizacion;
                            M.IdMaterial = item.IdProducto;
                            M.Cantidad = item.CantMat;
                            M.Ancho = item.Ancho;
                            M.Alto = item.Alto;
                            M.IdColor = item.IdColor;
                            M.Subtotal = item.Subtotal;
                            db.MaterialItemCotizacion.Add(M);
                        }
                        else
                        {
                            item.CantProducto = (int)item.CantMat;
                            db.ProductoCotizacion.Add(item);
                        }
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
                    ListaProductos = (List<ProductoCotizacion>)TempData["ListaProductos"];
                    ListaMateriales= (List<MaterialCotizacion>)TempData["MateralCotizacion"];

                   var  ListaP = (from s in db.ProductoCotizacion
                                      where s.IdCotizacion == id
                                      select s).ToList();
                    var ListaPM = (from s in db.MaterialItemCotizacion
                                  where s.IdCotizacion == id
                                  select s).ToList();
                    var ListaM = (from s in db.MaterialCotizacion
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

                    foreach (var item in ListaM)
                    {
                        db.MaterialCotizacion.Attach(item);
                        db.MaterialCotizacion.Remove(item);
                        db.SaveChanges();
                    }

                    foreach (var item in ListaP)
                    {      
                            db.ProductoCotizacion.Attach(item);
                            db.ProductoCotizacion.Remove(item);
                            db.SaveChanges();

                    }
                        foreach (var item in ListaPM)
                        {                       
                                db.MaterialItemCotizacion.Attach(item);
                                db.MaterialItemCotizacion.Remove(item);
                                db.SaveChanges();                           

                        }
                        foreach (var item in ListaMateriales)
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        db.MaterialCotizacion.Add(item);
                    }
                    foreach (var item in ListaProductos)
                    {
                        item.IdCotizacion = Cotiz.IdCotizacion;
                        if (item.Tipo == "Mat")
                        {
                            MaterialItemCotizacion M = new MaterialItemCotizacion();
                            M.IdCotizacion = Cotiz.IdCotizacion;
                            M.IdMaterial = item.IdProducto;
                            M.Cantidad = item.CantMat;
                            M.Ancho = item.Ancho;
                            M.Alto = item.Alto;
                            M.IdColor = item.IdColor;
                            M.Subtotal = item.Subtotal;
                            db.MaterialItemCotizacion.Add(M);
                        }
                        else
                        {
                            item.CantProducto = (int)item.CantMat;
                            db.ProductoCotizacion.Add(item);
                        }
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
