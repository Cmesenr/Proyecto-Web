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
        /*Create instance of entity model*/
        DataContext db = new DataContext();

        // GET: /UserLogin/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            /*Getting data from database for user validation*/
            var usuarioActual = from s in db.Usuario.Where(s => (s.IdUsuario == login.IdUsuario) && (s.Contraseña == login.Contraseña))
                                select s.IdUsuario;

            if (usuarioActual.Count() > 0)
            {
                /*Redirect user to success apge after successfull login*/
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.Message = 0;
            }

            return View();
        }
    }
}