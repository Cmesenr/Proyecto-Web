using PagedList;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class MaterialController : Controller
    {
        DataContext db = new DataContext();
        // GET: Material
        private List<ColorMaterial> Listacolores = new List<ColorMaterial>();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            
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
            var Materiales = from s in db.Material
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Materiales = Materiales.Where(s => s.Nombre.Contains(searchString)
                                                || s.CategoriaMat.Nombre.Contains(searchString)
                                                || s.SubCategoria.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    Materiales = Materiales.OrderByDescending(s => s.Nombre);
                    break;
                default:  // Name ascending 
                    Materiales = Materiales.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(Materiales.ToPagedList(pageNumber, pageSize));
        }


        // GET: Material/Create
        public ActionResult Registrar()
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
            ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
            ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
            return View();
        }

        // POST: Material/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Material material)
        {
            try
            {
                //ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
               // ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
                ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
                ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");

                //ModelState.Remove("Usuario");
                material.Usuario = Session["UsuarioActual"].ToString();
                if (ModelState.IsValid)
                {

                    if (material.IdCatMat != 1)
                    {
                        if (TempData["ListaColores"] != null)
                        {
                            db.Material.Add(material);
                            db.SaveChanges();
                            foreach (ColorMaterial item in (List<ColorMaterial>)TempData["ListaColores"])
                            {
                                item.IdMaterial = material.IdMaterial;
                                db.ColorMaterial.Add(item);
                            }
                            db.SaveChanges();
                            TempData["ListaColores"] = null;
                        }
                        else
                        {
                            ModelState.AddModelError("ColorMaterial", "Debe registrar al menos un Color.");
                            return View(material);
                        }

                    }
                    else
                    {
                        db.Material.Add(material);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(material);
        }

        // GET: Material/Edit/5
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(int? id)
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
            ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
            ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            if (material.IdCatMat!=1)
            {
                
                foreach (var item in (List<ColorMaterial>)material.ColorMaterial.ToList())
                {
                    ColorMaterial color = new ColorMaterial();
                    color.IdMaterial = item.IdMaterial;
                    color.IdColorMat = item.IdColorMat;
                    color.Costo = item.Costo;
                    color.NombreMaterial = item.ColorMat.Nombre.Trim();
                    Listacolores.Add(color);
                }
                TempData["ListaColores"] = Listacolores;
            }
            return PartialView(material);
        }

        // POST: Material/Edit/5
        [HttpPost]
        public ActionResult Editar(int? id, Material material)
        {
                  ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
                ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
                ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
                ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
                    try
                    {
                if (ModelState.IsValid)
                {
                    material.IdMaterial = id.Value;
                    db.Entry(material).State = EntityState.Modified;
                    db.SaveChanges();
                    if (material.IdCatMat != 1)
                    {
                        var colores = from s in db.ColorMaterial
                                           select s;
                        colores = colores.Where(s => s.IdMaterial == id
                                                            );
                        Listacolores = colores.ToList();
                        foreach (ColorMaterial item in Listacolores)
                        {   
                            db.ColorMaterial.Attach(item);
                            db.ColorMaterial.Remove(item);
                            db.SaveChanges();
                        }
                        if (TempData["ListaColores"] != null)
                        {
                            foreach (ColorMaterial item in (List<ColorMaterial>)TempData["ListaColores"])
                            {
                                item.IdMaterial = material.IdMaterial;
                                db.ColorMaterial.Add(item);
                            }
                            db.SaveChanges();
                            TempData["ListaColores"] = null;
                        }
                        else
                        {
                            ModelState.AddModelError("ColorMaterial", "Debe registrar al menos un Color.");
                            return PartialView(material);
                        }
                    }
                   

                    return RedirectToAction("Index");
                }
                       
                    }
                    catch (RetryLimitExceededException /* dex */)
                    {
                        ModelState.AddModelError("", "Imposible guardar los cambios. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
            }
            return PartialView(material);
        }

        // GET: Material/Delete/5
        public ActionResult Borrar(int id)
        {
            Material materialToUpdate = db.Material.Find(id);
            try
            {
                materialToUpdate.Estado = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible eliminar el registro. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
            }

            return RedirectToAction("Index");
        }
 
        public JsonResult AgregarColor(int IdColor, decimal Costo)
        {
            var resultado = "No se pudo agregar el color";
            if (TempData["ListaColores"] != null) {
                Listacolores = (List<ColorMaterial>)TempData["ListaColores"];
                
            }
            try
            {
                ColorMaterial colormaterial = new ColorMaterial();
                colormaterial.IdColorMat = IdColor;
                colormaterial.Costo = Costo;
                colormaterial.NombreMaterial = db.ColorMat.Find(IdColor).Nombre.Trim();
                if (Listacolores.Count()==0) { Listacolores.Add(colormaterial); }
                else
                {

                    foreach (ColorMaterial color in Listacolores)
                    {
                        if (color.IdColorMat == IdColor)
                        {
                            TempData["ListaColores"] = Listacolores;
                            resultado = "No se puede duplicar el color!";
                            return Json(resultado,
                            JsonRequestBehavior.AllowGet);
                        }

                    }
                    Listacolores.Add(colormaterial);
                }
            }
            catch{
                  return Json(resultado,
                JsonRequestBehavior.AllowGet);
            }
            TempData["ListaColores"] = Listacolores;
            return Json(Listacolores.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult EliminarColor(int id)
        {
            if (TempData["ListaColores"] != null)
            {
                Listacolores = (List<ColorMaterial>)TempData["ListaColores"];
            }
            try
            {
                foreach(ColorMaterial color in Listacolores)
                {
                    if (color.IdColorMat == id) { 
                        Listacolores.Remove(color);
                        break;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "No se pudo borrar la linea, y si el problema persiste contacte el administrador del sistema.");
            }
            TempData["ListaColores"] = Listacolores;
            return Json(Listacolores.ToList(),
                JsonRequestBehavior.AllowGet);

        }
        public JsonResult CargarSubcategoria(int id)
        {
            var SubCategoria = from s in db.SubCategoria
                               select s;
            SubCategoria = SubCategoria.Where(s => s.IdCatMat == id);
            List<SubCategoria> listSubCat = new List<SubCategoria>();
            foreach (var item in SubCategoria.ToList())
            {
                SubCategoria s = new Models.SubCategoria();
                s.IdSubCatMat = item.IdSubCatMat;
                s.Nombre = item.Nombre;
                listSubCat.Add(s);
            }

            return Json(listSubCat.ToList(),
               JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarColores(int id)
        {
            var Colores = from s in db.ColorMat
                          select s;
            Colores = Colores.Where(s => s.IdCatMaterial == id);
            List<ColorMat> listcolor = new List<ColorMat>();
            foreach (var item in Colores.ToList())
            {
                ColorMat s = new Models.ColorMat();
                s.IdColor = item.IdColor;
                s.Nombre = item.Nombre;
                listcolor.Add(s);
            }

            return Json(listcolor.ToList(),
               JsonRequestBehavior.AllowGet);
        }
        public void RefrescarLista()
        {
            TempData["ListaColores"] = null;
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
