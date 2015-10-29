using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace SWRCVA.Controllers
{
    public class ClienteController : Controller
    {
        DataContext db = new DataContext();

        // GET: Cliente
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

            var clientes = from s in db.Cliente
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.Nombre.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Nombre":
                    clientes = clientes.OrderByDescending(s => s.Nombre);
                    break;
                default:  // Name ascending 
                    clientes = clientes.OrderBy(s => s.Nombre);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(clientes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cliente/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Cliente/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Cliente cliente)
        {
            try
            {
                ModelState.Remove("Usuario");
                cliente.Usuario = Session["UsuarioActual"].ToString();

                if (ModelState.IsValid)
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(cliente);
        }

        // GET: Cliente/Editar/5
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return PartialView(cliente);
        }

        // POST: Cliente/Editar/5
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente clienteToUpdate = db.Cliente.Find(id);
            if (TryUpdateModel(clienteToUpdate, "",
               new string[] { "Nombre", "Telefono", "Correo", "Direccion", "Estado" }))
            {
                clienteToUpdate.Usuario = Session["UsuarioActual"].ToString();
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
            return View(clienteToUpdate);
        }

        // GET: Cliente/Borrar/5
        public ActionResult Borrar(int? id)
        {
            Cliente clienteToUpdate = db.Cliente.Find(id);
            try
            {
                clienteToUpdate.Usuario = Session["UsuarioActual"].ToString();
                clienteToUpdate.Estado = 0;
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