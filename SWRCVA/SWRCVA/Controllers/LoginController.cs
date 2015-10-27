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
            //var usuarioActual = from s in db.Usuario.Where(s => (s.IdUsuario == login.IdUsuario) && (s.Contraseña == login.Contraseña))
            //                    select s.IdUsuario;

            Usuario usuarioActual = db.Usuario.Find(login.IdUsuario);

            Session["UsuarioActual"] = usuarioActual.IdUsuario.ToString();

            return RedirectToAction("Index", "Home");
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