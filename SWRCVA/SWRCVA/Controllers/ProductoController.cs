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

namespace SWRCVA.Controllers
{
    public class ProductoController : Controller
    {
        private DataContext db = new DataContext();
        private List<ListaMatProducto> Listamateriales = new List<ListaMatProducto>();
        // GET: Productoes
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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
                producto = producto.Where(s => s.Nombre.Contains(searchString)
                                                || s.TipoProducto.Nombre.Contains(searchString)
                                                );
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
            ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
            ViewBag.Categorias = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Imagen,Estado,Usuario")] Producto producto)
        {
            try
            {
                ViewBag.IdTipoProducto = new SelectList(db.TipoProducto, "IdTipoProducto", "Nombre");
                if (ModelState.IsValid)
                {

                      if (TempData["ListaMateriales"] != null)
                        {
                            db.Producto.Add(producto);
                            db.SaveChanges();
                            foreach (ListaMatProducto item in (List<ListaMatProducto>)TempData["ListaMateriales"])
                            {
                                item.IdMaterial = producto.IdProducto;
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
        public ActionResult Edit(int? id)
        {
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
            return PartialView(producto);
        }

        // POST: Productoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProducto,Nombre,IdTipoProducto,Imagen,Estado,Usuario")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                if (TempData["ListaMateriales"] != null)
                {
                   
                    foreach (ListaMatProducto item in producto.ListaMatProducto.ToList())
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
        public JsonResult AgregarMaterial(int IdMaterial)
        {
            var resultado = "No se pudo agregar el color";
            if (TempData["ListaMateriales"] != null)
            {
                Listamateriales = (List<ListaMatProducto>)TempData["ListaMateriales"];

            }
            try
            {
                ListaMatProducto ListMat = new ListaMatProducto();
                ListMat.IdMaterial = IdMaterial;
                ListMat.Usuario = "Charlie";
                ListMat.NombreMaterial = db.Material.Find(IdMaterial).Nombre;
                if (Listamateriales.Count() == 0) { Listamateriales.Add(ListMat); }
                else
                {

                    foreach (ListaMatProducto listMatProduct in Listamateriales)
                    {
                        if (listMatProduct.IdMaterial == IdMaterial)
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
           var SubCategoria = from s in db.SubCategoria
                       select s;
            SubCategoria = SubCategoria.Where(s => s.IdCatMat==id                                         
                                                );
            return Json(SubCategoria.ToList(),
               JsonRequestBehavior.AllowGet);
        }

        public void RefrescarLista()
        {
            TempData["ListaMateriales"] = null;
        }

        public void CargarImagen()
        {
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                TempData["Image"] = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];
            }
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
