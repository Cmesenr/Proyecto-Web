using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace SWRCVA.Controllers
{
    public class ProveedorController : Controller
    {
        DataContext db = new DataContext();

        // GET: Proveedor
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

            var proveedores = from s in db.Proveedor
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                proveedores = proveedores.Where(s => s.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    proveedores = proveedores.OrderByDescending(s => s.Nombre);
                    break;
                default:  // Name ascending 
                    proveedores = proveedores.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(proveedores.ToPagedList(pageNumber, pageSize));
        }

        // GET: Proveedor/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Proveedor/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = " IdProveedor,Nombre, Correo, Direccion, Telefono, Estado, Usuario")]Proveedor proveedor)
        {
            try
            {
                ModelState.Remove("Usuario");
                proveedor.Usuario = Session["UsuarioActual"].ToString();

                if (ModelState.IsValid)
                {
                    db.Proveedor.Add(proveedor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(proveedor);
        }

        // GET: Proveedor/Editar
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return PartialView(proveedor);
        }

        // POST: Proveedor/Editar
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedorToUpdate = db.Proveedor.Find(id);
            if (TryUpdateModel(proveedorToUpdate, "",
               new string[] { "Nombre", "Correo", "Direccion","Telefono","Estado" }))
            {
                proveedorToUpdate.Usuario = Session["UsuarioActual"].ToString();
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Imposible guardar los cambios. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
                }
            }
            return View(proveedorToUpdate);
        }

        // GET: Proveedor/Borrar
        public ActionResult Borrar(int? id)
        {
            Proveedor proveedorToUpdate = db.Proveedor.Find(id);
            try
            {
                proveedorToUpdate.Usuario= Session["UsuarioActual"].ToString();
                proveedorToUpdate.Estado = 0;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible eliminar el registro. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
            }

            return RedirectToAction("Index");
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