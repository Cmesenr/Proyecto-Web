using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity.Infrastructure;

namespace SWRCVA.Controllers
{
    public class UsuarioController : Controller
    {
        DataContext db = new DataContext();

        // GET: Usuario
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdUsuarioSortParm = String.IsNullOrEmpty(sortOrder) ? "IdUsuario" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var usuarios = from s in db.Usuario
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(s => s.IdUsuario.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "IdUsuario":
                    usuarios = usuarios.OrderByDescending(s => s.IdUsuario);
                    break;
                default:  // IdUsuario ascending 
                    usuarios = usuarios.OrderBy(s => s.IdUsuario);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(usuarios.ToPagedList(pageNumber, pageSize));
        }

        // GET: Usuario/Registrar
        public ActionResult Registrar()
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");
            return View();
        }

        // POST: Usuario/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(Usuario usuario)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");
            try
            {
                ModelState.Remove("Usuario1");
                usuario.Contraseña = Encriptar(usuario.Contraseña);
                usuario.Usuario1 = Session["UsuarioActual"].ToString();

                Usuario validaUsuario = db.Usuario.Find(usuario.IdUsuario);

                if ( validaUsuario == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Usuario.Add(usuario);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(usuario);
        }

        // GET: Usuario/Editar
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(string id)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Usuario usuario = db.Usuario.Find(id);
            if (usuario != null)
            {
                usuario.Contraseña = DesEncriptar(usuario.Contraseña);
            }

            if (usuario == null)
            {
                return HttpNotFound();
            }
            return PartialView(usuario);
        }

        // POST: Usuario/Editar
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPost(string id, Usuario usuario)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ModelState.Remove("Usuario1");
            usuario.Usuario1= Session["UsuarioActual"].ToString();
            Usuario usuarioToUpdate = db.Usuario.Find(id);


            if (ModelState.IsValid) {
                if (TryUpdateModel(usuarioToUpdate, "",
                   new string[] { "Contraseña", "IdRol", "Estado", "Usuario1" }))
                {
                    usuarioToUpdate.Contraseña = Encriptar(usuario.Contraseña);
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
            }
            return View(usuarioToUpdate);
        }

        // GET: Usuario/Borrar
        public ActionResult Borrar(string id)
        {
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            Usuario usuarioToUpdate = db.Usuario.Find(id);
            try
            {
                usuarioToUpdate.Usuario1 = Session["UsuarioActual"].ToString();
                usuarioToUpdate.Estado = 0;
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

        public string Encriptar(string cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public string DesEncriptar(string cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(cadenaAdesencriptar);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
    }
}