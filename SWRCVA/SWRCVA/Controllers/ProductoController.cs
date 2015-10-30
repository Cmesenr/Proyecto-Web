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
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Text.RegularExpressions;

namespace SWRCVA.Controllers
{
    public class ProductoController : Controller
    {
        private DataContext db = new DataContext();
        private List<ListaMatProducto> Listamateriales = new List<ListaMatProducto>();
        // GET: Productoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            var producto = db.Producto.Include(p => p.TipoProducto);
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
            producto = from s in db.Producto
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                producto = producto.Where(s => s.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    producto = producto.OrderByDescending(s => s.Nombre);
                    break;
                default:  // Name ascending 
                    producto = producto.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(producto.ToPagedList(pageNumber, pageSize));
        }


        // GET: Productoes/Create
        public ActionResult Registrar()
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Imagen,Estado,Usuario")] Producto producto, HttpPostedFileBase ImageFile)
        {
            try
            {
                ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
                ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");

                ModelState.Remove("Usuario");
                producto.Usuario = Session["UsuarioActual"].ToString();

                if (ModelState.IsValid)
                {     
                        if (ImageFile != null)
                        {
                        byte[] array;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(ms);
                            array = ms.GetBuffer();
                        }
                        if (HttpPostedFileBaseExtensions.GetImageFormat(array) != HttpPostedFileBaseExtensions.ImageFormat.Desconocido)
                        {
                            producto.Imagen = array;
                        }
                        else
                        {
                            ModelState.AddModelError("Imagen", "La imagen debe de ser de un formato correcto(jpg,png,gif,jpeg,tiff,bmp).");
                            return View();
                        }
                    }
                        
             

                        if (TempData["ListaMateriales"] != null)
                        {
                            db.Producto.Add(producto);
                            db.SaveChanges();
                            foreach (ListaMatProducto item in (List<ListaMatProducto>)TempData["ListaMateriales"])
                            {
                                item.IdProducto = producto.IdProducto;
                                db.ListaMatProducto.Add(item);
                            }
                            db.SaveChanges();
                            TempData["ListaMateriales"] = null;
                        }
                    else {
                        db.Producto.Add(producto);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(producto);
        }

        // GET: Productoes/Edit/5
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(int? id)
        {
            if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            foreach (var item in (List<ListaMatProducto>)producto.ListaMatProducto.ToList())
            {
                ListaMatProducto MatProduct = new ListaMatProducto();
                MatProduct.IdProducto = item.IdProducto;
                MatProduct.IdMaterial = item.IdMaterial;
                MatProduct.Usuario = item.Usuario;
                MatProduct.NombreMaterial = item.Material.Nombre;
                Listamateriales.Add(MatProduct);
            }
            TempData["ListaMateriales"] = Listamateriales;
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre", producto.IdTipoProducto);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Imagen,Estado,Usuario")] Producto producto, HttpPostedFileBase ImageFile)
        {
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");

            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                Producto productoToUpdate = db.Producto.Find(producto.IdProducto);
                if (TryUpdateModel(productoToUpdate, "",
                   new string[] { "Nombre", "IdTipoProducto", "Imagen", "Estado", "Usuario" }))
                {
                    productoToUpdate.Usuario = Session["UsuarioActual"].ToString();
                    
                        if (ImageFile != null)
                    {
                        byte[] array;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            ImageFile.InputStream.CopyTo(ms);
                            array = ms.GetBuffer();
                        }
                        if (HttpPostedFileBaseExtensions.GetImageFormat(array)!= HttpPostedFileBaseExtensions.ImageFormat.Desconocido)
                        {                         
                                productoToUpdate.Imagen = array;                         
                        }
                        else
                        {
                            ModelState.AddModelError("Imagen", "La imagen debe de ser de un formato correcto(jpg,png,gif,jpeg,tiff,bmp).");
                            return View(productoToUpdate);
                        }
                    }
                    db.Entry(productoToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (TempData["ListaMateriales"] != null)
                {
                    var materiales = (from s in db.ListaMatProducto
                                      where s.IdProducto == producto.IdProducto
                                      select s).ToList();

                    foreach (ListaMatProducto item in materiales)
                    {
                        db.ListaMatProducto.Attach(item);
                        db.ListaMatProducto.Remove(item);
                        db.SaveChanges();
                    }
                    foreach (ListaMatProducto item in (List<ListaMatProducto>)TempData["ListaMateriales"])
                    {
                        item.IdProducto = producto.IdProducto;
                        db.ListaMatProducto.Add(item);
                    }
                    db.SaveChanges();
                    TempData["ListaMateriales"] = null;
                }

                return RedirectToAction("Index");
            }
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre", producto.IdTipoProducto);
            return View(producto);
        }

        // GET: Productoes/Borrar/5
        public ActionResult Borrar(int? id)
        {
            Producto productotoUpdate = db.Producto.Find(id);
            try
            {
                productotoUpdate.Usuario = Session["UsuarioActual"].ToString();
                productotoUpdate.Estado = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible eliminar el registro. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
            }

            return RedirectToAction("Index");
        }
        public JsonResult AgregarMaterial(int IdMat)
        {
            var resultado = "No se pudo agregar el color";
            if (TempData["ListaMateriales"] != null)
            {
                Listamateriales = (List<ListaMatProducto>)TempData["ListaMateriales"];

            }
            try
            {
                ListaMatProducto ListMat = new ListaMatProducto();
                ListMat.IdMaterial = IdMat;
                ListMat.Usuario = "Charlie";
                ListMat.NombreMaterial = db.Material.Find(IdMat).Nombre;
                if (Listamateriales.Count() == 0) { Listamateriales.Add(ListMat); }
                else
                {

                    foreach (ListaMatProducto listMatProduct in Listamateriales)
                    {
                        if (listMatProduct.IdMaterial == IdMat)
                        {
                            TempData["ListaMateriales"] = Listamateriales;
                            resultado = "No se puede duplicar el Material!";
                            return Json(resultado,
                            JsonRequestBehavior.AllowGet);
                        }

                    }
                    Listamateriales.Add(ListMat);
                }
            }
            catch
            {
                return Json(resultado,
              JsonRequestBehavior.AllowGet);
            }
            TempData["ListaMateriales"] = Listamateriales;
            return Json(Listamateriales.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult EliminarMaterial(int id)
        {
            if (TempData["ListaMateriales"] != null)
            {
                Listamateriales = (List<ListaMatProducto>)TempData["ListaMateriales"];
            }
            try
            {
                foreach (ListaMatProducto listMatProduct in Listamateriales)
                {
                    if (listMatProduct.IdMaterial == id)
                    {
                        Listamateriales.Remove(listMatProduct);
                        break;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo borrar la linea, y si el problema persiste contacte el administrador del sistema.");
            }
            TempData["ListaMateriales"] = Listamateriales;
            return Json(Listamateriales.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult CargarSubcategoria(int id)
        {
           var SubCategoria = (from s in db.SubCategoria
                              where s.IdCatMat==id
                       select new
                       {
                           IdSubCatMat = s.IdSubCatMat,
                           Nombre=s.Nombre
                       }).ToList();
           
            
            return Json(SubCategoria,
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarMateriales(int IdCat, int? IdSubcat)
        {
            if (IdCat == 1)
            {
                var Materiales = (from s in db.Material
                                  where s.IdCatMat== IdCat
                                  select new
                                  {
                                      IdMaterial = s.IdMaterial,
                                      Nombre=s.Nombre
                                  
                                  });
                

                return Json(Materiales,
                   JsonRequestBehavior.AllowGet);

            }
            else
            {
                var Materiales = (from s in db.Material
                                  where s.IdCatMat == IdCat && s.IdSubCatMat == IdSubcat
                                  select new
                                  {
                                      IdMaterial = s.IdMaterial,
                                      Nombre = s.Nombre
                                  }).ToList();

                return Json(Materiales,
                   JsonRequestBehavior.AllowGet);
            }
           
        }

        public void RefrescarLista()
        {
            TempData["ListaMateriales"] = null;
            Session["Image"] = null;
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
