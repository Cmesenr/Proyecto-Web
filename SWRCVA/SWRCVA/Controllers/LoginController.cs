using SWRCVA.Models;
using System;
using System.Collections.Generic;
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

            if (usuarioActual != null && usuarioActual.Contraseña == login.Contraseña)
            {
                Session["UsuarioActual"] = usuarioActual.IdUsuario.ToString();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "¡Lo sentimos datos no válidos, por favor intentelo de nuevo!";

                return View();
            }
        }

        //
        // POST: /Login/CerrarSession
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CerrarSession()
        {
            Session["UsuarioActual"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}