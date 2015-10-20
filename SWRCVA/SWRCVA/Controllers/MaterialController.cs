using PagedList;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class MaterialController : Controller
    {
        DataContext db = new DataContext();
        // GET: Material

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

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Materiales.ToPagedList(pageNumber, pageSize));
        }

        // GET: Material/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Material/Create
        public ActionResult Registrar()
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
            ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
            ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
            Material objMat = new Material();
            return PartialView();
        }

        // POST: Material/Create
        [HttpPost]
        public ActionResult Registrar(Material material)
        {
            try
            {
                ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
                ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
                ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
                ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
                if (ModelState.IsValid)
                {
                    db.Material.Add(material);
                    db.SaveChanges();
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
        public ActionResult Editar(int id)
        {
            return View();
        }

        // POST: Material/Edit/5
        [HttpPost]
        public ActionResult Editar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Material/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Material/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
