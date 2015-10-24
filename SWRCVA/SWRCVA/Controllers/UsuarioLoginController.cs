using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class UsuarioLoginController : Controller
    {
        /*Create instance of entity model*/
        DataContext db = new DataContext();

        // GET: /UserLogin/
        public ActionResult Index()
        {
            //UsuarioLogin usuarioLogin = new UsuarioLogin();
            return View(/*usuarioLogin*/);
        }

        [HttpPost]
        public ActionResult Index(UsuarioLogin usuarioLogin)
        {
            /*Getting data from database for user validation*/
            var usuarioActual = (from usuario in db.Usuario
                                  where usuario.IdUsuario == usuarioLogin.UserId
                                  && usuario.Contraseña == usuario.Contraseña
                                  select usuario);
            if (usuarioActual.Count() > 0)
            {
                /*Redirect user to success apge after successfull login*/
                ViewBag.Message = 1;
            }
            else
            {
                ViewBag.Message = 0;
            }
            return View(usuarioActual);
        }
    }
}