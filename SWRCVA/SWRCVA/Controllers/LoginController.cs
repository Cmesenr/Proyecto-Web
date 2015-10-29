﻿using SWRCVA.Models;
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
            Rol rolUsuarioActual = db.Rol.Find(usuarioActual.IdRol);

            if (usuarioActual == null)
            {
                ViewBag.Message = "¡Lo sentimos usuario no existe!";
            }

            if (usuarioActual != null && usuarioActual.Contraseña != login.Contraseña)
            {
                ViewBag.Message = "¡Lo sentimos contraseña inválida!";
            }

            if (usuarioActual != null && usuarioActual.Contraseña == login.Contraseña)
            {
                Session["UsuarioActual"] = usuarioActual.IdUsuario.ToString();
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
    }
}