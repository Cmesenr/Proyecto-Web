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
        private List<ListaMaterialesEsperProducto> Listamateriales = new List<ListaMaterialesEsperProducto>();
        // GET: Productoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

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
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Forma,Imagen,Estado,Usuario")] Producto producto, HttpPostedFileBase ImageFile)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

            try
            {
                ViewBag.IdTipoProducto = new SelectList((from s in db.TipoProducto
                                                         where s.Estado == 1
                                                         select new
                                                         {
                                                             s.IdTipoProducto,
                                                             s.Nombre
                                                         }
                                                  ), "IdTipoProducto", "Nombre"); 
                ViewBag.Categorias = new SelectList((from s in db.CategoriaMat
                                                     where s.Estado == 1
                                                     select new
                                                     {
                                                         s.IdCategoria,
                                                         s.Nombre
                                                     }
                                                  ), "IdCategoria", "Nombre");

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
                            foreach (ListaMaterialesEsperProducto item in (List<ListaMaterialesEsperProducto>)TempData["ListaMateriales"])
                            {
                                ListaMatProducto LM = new ListaMatProducto();
                                LM.IdProducto= producto.IdProducto;
                                LM.IdMaterial = item.IdMaterial;
                                db.ListaMatProducto.Add(LM);
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
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.IdTipoProducto = new SelectList((from s in db.TipoProducto
                                                     where s.Estado == 1
                                                     select new
                                                     {
                                                         s.IdTipoProducto,
                                                         s.Nombre
                                                     }
                                                    ), "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList((from s in db.CategoriaMat
                                                 where s.Estado == 1
                                                 select new
                                                 {
                                                     s.IdCategoria,
                                                     s.Nombre
                                                 }
                                              ), "IdCategoria", "Nombre");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            var listaMatEsperados = db.MaterialEsperado.Where(s => s.IdForma == producto.Forma).ToList();
            var listaMatPro = (from s in db.ListaMatProducto
                               where s.IdProducto == producto.IdProducto
                               select s).ToList();
            foreach (var item in listaMatEsperados)
            {
                bool Activador = false;
                    foreach(var itemM in listaMatPro)
                    {
                        if (itemM.Material.IdTipoMaterial == item.IdTipoMaterial)
                        {
                            Activador = true;
                           ListaMaterialesEsperProducto MatProduct = new ListaMaterialesEsperProducto();
                            MatProduct.IdMaterial = itemM.IdMaterial;
                            MatProduct.NombreMaterial = itemM.Material.Nombre;
                            MatProduct.IdTipoMaterial = (int)itemM.Material.IdTipoMaterial;
                            MatProduct.IdForma = producto.Forma;
                            MatProduct.Nombre = item.TipoMaterial.Nombre;
                            MatProduct.Estado = 1;
                            Listamateriales.Add(MatProduct);
                          
                        }
                    }
                if (Activador == false)
                {
                    ListaMaterialesEsperProducto MatProduct = new ListaMaterialesEsperProducto();
                    MatProduct.IdTipoMaterial = (int)item.IdTipoMaterial;
                    MatProduct.IdForma = producto.Forma;
                    MatProduct.Nombre = item.TipoMaterial.Nombre;
                    MatProduct.Estado = 0;
                    Listamateriales.Add(MatProduct);
                }
                  

            }
            ViewData["ListaMaterialesEdit"] = Listamateriales;
            TempData["ListaMateriales"] = Listamateriales;
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Forma,Imagen,Estado,Usuario")] Producto producto, HttpPostedFileBase ImageFile)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

            ViewBag.IdTipoProducto = new SelectList((from s in db.TipoProducto
                                                     where s.Estado == 1
                                                     select new
                                                     {
                                                         s.IdTipoProducto,
                                                         s.Nombre
                                                     }
                                                              ), "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList((from s in db.CategoriaMat
                                                 where s.Estado == 1
                                                 select new
                                                 {
                                                     s.IdCategoria,
                                                     s.Nombre
                                                 }
                                              ), "IdCategoria", "Nombre");
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                Producto productoToUpdate = db.Producto.Find(producto.IdProducto);
                if (TryUpdateModel(productoToUpdate, "",
                   new string[] { "Nombre", "IdTipoProducto","Forma", "Imagen", "Estado", "Usuario" }))
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
                    foreach (var item in (List<ListaMaterialesEsperProducto>)TempData["ListaMateriales"])
                    {
                        if (item.Estado == 1)
                        {
                            ListaMatProducto LM = new ListaMatProducto();
                            LM.IdProducto = producto.IdProducto;
                            LM.IdMaterial = item.IdMaterial;
                            db.ListaMatProducto.Add(LM);
                        }
                
                    }
                    db.SaveChanges();
                    TempData["ListaMateriales"] = null;
                }

                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Productoes/Borrar/5
        public ActionResult Borrar(int? id)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");
            if (!LoginController.validaRol(Session))
                return RedirectToAction("Index", "Home");

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
        public JsonResult AgregarMaterial(int IdMat, string forma)
        {
            var resultado = "No se pudo agregar el color";
            if (TempData["ListaMateriales"] != null)
            {
                Listamateriales = (List<ListaMaterialesEsperProducto>)TempData["ListaMateriales"];

            }
            try
            {

                Material mater = db.Material.Find(IdMat);
                ListaMaterialesEsperProducto mat = new ListaMaterialesEsperProducto();
                mat.IdMaterial = mater.IdMaterial;
                mat.NombreMaterial = mater.Nombre;
                mat.IdTipoMaterial = (int)mater.IdTipoMaterial;
                var listaMatEsperados = db.MaterialEsperado.Where(s => s.IdForma == forma).ToList();
                bool Activador = false;
                foreach(var item in listaMatEsperados)
                {
                        if (item.IdTipoMaterial == mater.IdTipoMaterial)
                        {
                            for(int i=0;i< Listamateriales.Count(); i++)
                        {
                            if (Listamateriales[i].IdMaterial == IdMat)
                            {
                                TempData["ListaMateriales"] = Listamateriales;
                                resultado = "No se puede duplicar el Material!";
                                return Json(resultado,
                                JsonRequestBehavior.AllowGet);
                            }
                            if (Listamateriales[i].IdTipoMaterial == mat.IdTipoMaterial)
                            {
                                Listamateriales[i].IdForma = forma;
                                Listamateriales[i].IdMaterial = mat.IdMaterial;
                                Listamateriales[i].NombreMaterial = mat.NombreMaterial;
                                Listamateriales[i].Estado = 1;
                                Activador = true;
                            }
                        }
                        
                        }                 

                }
                if (Activador == false)
                {
                    TempData["ListaMateriales"] = Listamateriales;
                    resultado = "Material no apto para este producto!";
                    return Json(resultado,
                    JsonRequestBehavior.AllowGet);
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
                Listamateriales = (List<ListaMaterialesEsperProducto>)TempData["ListaMateriales"];
            }
            try
            {
                        for (int i = 0; i < Listamateriales.Count(); i++)
                        {
                            if (Listamateriales[i].IdMaterial == id)
                            {
                                Listamateriales[i].IdForma = "";
                                Listamateriales[i].IdMaterial = 0;
                                Listamateriales[i].NombreMaterial ="";
                                Listamateriales[i].Estado = 0;
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
                              where s.IdCatMat==id && s.Estado==1
                       select new
                       {
                           IdSubCatMat = s.IdSubCatMat,
                           Nombre=s.Nombre
                       }).ToList();
           
            
            return Json(SubCategoria,
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarMaterialesEsperados(string  id)
        {
            RefrescarLista();
            Listamateriales = (from s in db.MaterialEsperado
                               where s.IdForma == id
                               select new ListaMaterialesEsperProducto
                               {
                                   IdForma = s.IdForma,
                                   IdTipoMaterial = s.IdTipoMaterial,
                                   Nombre = s.TipoMaterial.Nombre,
                                   Estado = 0
                                     }).ToList();
            TempData["ListaMateriales"] = Listamateriales;
            return Json(Listamateriales,
              JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarMateriales(int IdCat, int? IdSubcat)
        {
            if (IdCat == 1)
            {
                var Materiales = (from s in db.Material
                                  where s.IdCatMat== IdCat && s.Estado == 1
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
                                  where s.IdCatMat == IdCat && s.IdSubCatMat == IdSubcat && s.Estado == 1
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
