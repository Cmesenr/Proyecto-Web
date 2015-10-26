﻿using SWRCVA.Models;
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
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
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
            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");

            return View();
        }

        // POST: Usuario/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar([Bind(Include = " IdUsuario, Contraseña, IdRol, Estado, Usuario1")]Usuario usuario)
        {
            ViewBag.Rol = new SelectList(db.Rol, "IdRol", "Nombre");
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Imposible guardar cambios. Intentelo de nuevo, y si el problema persiste contacte el administrador del sistema.");
            }
            return View(usuario);
        }

        // GET: Usuario/Editar
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Editar
        [HttpPost, ActionName("Editar")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuarioToUpdate = db.Usuario.Find(id);
            if (TryUpdateModel(usuarioToUpdate, "",
               new string[] { "Contraseña, IdRol, Estado, Usuario" }))
            {
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
            return View(usuarioToUpdate);
        }

        // GET: Usuario/Borrar
        public ActionResult Borrar(int? id)
        {
            Usuario usuarioToUpdate = db.Usuario.Find(id);
            try
            {
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
    }
}