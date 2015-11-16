using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class LoginController : Controller
    {
        DataContext db = new DataContext();

        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Login/
        [HttpPost]
        public ActionResult Login(Login login)
        {
            Usuario usuarioActual = db.Usuario.Find(login.IdUsuario);

            if (usuarioActual == null)
            {
                ViewBag.Message = "¡Lo sentimos usuario no existe!";
            }

            if (usuarioActual != null && usuarioActual.Contraseña != Encriptar(login.Contraseña))
            {
                ViewBag.Message = "¡Lo sentimos contraseña inválida!";
            }

            if (usuarioActual != null && usuarioActual.Contraseña == Encriptar(login.Contraseña))
            {
                Session["UsuarioActual"] = usuarioActual.IdUsuario.ToString();

                Rol rolUsuarioActual = db.Rol.Find(usuarioActual.IdRol);
                Session["RolUsuarioActual"] = rolUsuarioActual.Nombre.ToString();

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // POST: /Login/CerrarSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarSession()
        {
            Session["UsuarioActual"] = null;
            Session["RolUsuarioActual"] = null;

            return RedirectToAction("Index", "Home");
        }

        // GET: /CambiarClave/
        public ActionResult CambiarContraseña()
        {
            return View();
        }

        // POST: /CambiarContraseña/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContraseña(Login login)
        {
            if (login.IdUsuario != null && login.Contraseña != null)
            {
                Usuario usuarioToUpdate = db.Usuario.Find(login.IdUsuario);

                if (usuarioToUpdate != null)
                {
                    ModelState.Remove("Contraseña");
                    ModelState.Remove("Usuario1");
                    usuarioToUpdate.Usuario1 = login.IdUsuario;

                    if (ModelState.IsValid)
                    {
                        if (TryUpdateModel(usuarioToUpdate, "",
                           new string[] { "Contraseña", "IdRol", "Estado", "Usuario1" }))
                        {
                            usuarioToUpdate.Contraseña = Encriptar(login.Contraseña);
                            try
                            {
                                db.SaveChanges();

                                return RedirectToAction("Login");
                            }
                            catch (RetryLimitExceededException /* dex */)
                            {
                                ModelState.AddModelError("", "Imposible guardar los cambios. Intentelo de nuevo, si el problema persiste, contacte el administrador del sistema.");
                            }
                        }
                    }
                }
            }
            return View();
        }

        public string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
    }
}