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
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult CambiarContraseña()
        {
            LoginController loginController = new LoginController();
            if (!loginController.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            return View();
        }

        // POST: /CambiarContraseña/
        [HttpPost]
        public ActionResult CambiarContraseña(string contraseña)
        {
            LoginController loginController = new LoginController();
            if (!loginController.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

            if (Session["UsuarioActual"] != null && contraseña != null)
            {
                Usuario usuarioToUpdate = db.Usuario.Find(Session["UsuarioActual"].ToString());

                if (usuarioToUpdate != null)
                {
                    ModelState.Remove("Contraseña");
                    ModelState.Remove("Usuario1");
                    usuarioToUpdate.Usuario1 = Session["UsuarioActual"].ToString();

                    if (ModelState.IsValid)
                    {
                        if (TryUpdateModel(usuarioToUpdate, "",
                           new string[] { "Contraseña", "IdRol", "Estado", "Usuario1" }))
                        {
                            usuarioToUpdate.Contraseña = Encriptar(contraseña);
                            try
                            {
                                db.SaveChanges();
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

        public string Encriptar(string cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        public bool validaUsuario(HttpSessionStateBase session)
        {
            bool usuarioValido = false;

            if (session["UsuarioActual"] == null || session.Timeout == 6)
            {
                usuarioValido = false;
            }
            else
            {
                usuarioValido = true;
            }

            return usuarioValido;
        }

        public bool validaRol(HttpSessionStateBase session)
        {
            bool rolAdmin = false;

            if (session["RolUsuarioActual"].ToString() == "Procesos")
            {
                rolAdmin = false;
            }
            else
            {
                rolAdmin = true;
            }

            return rolAdmin;
        }
    }
}