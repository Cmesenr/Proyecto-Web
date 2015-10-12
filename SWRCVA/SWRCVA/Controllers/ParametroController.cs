using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class ParametroController : Controller
    {
        DataContext db = new DataContext();
        // GET: Parametro
        public ActionResult Registrar()
        {
            return View();
        }

        // GET: Parametro/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Parametro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parametro/Registrar
        [HttpPost]
        public ActionResult Registrar(Parametro parametrop)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    switch (parametrop.parametro) {
                        case 1:
                            CategoriaMat cat = new CategoriaMat();
                            cat.Nombre = parametrop.Nombre;
                            cat.Usuario = parametrop.Usuario;
                            cat.Estado = parametrop.Estado;
                            db.CategoriaMat.Add(cat);
                            db.SaveChanges();
                            break;
                        case 2:
                            ColorMat color = new ColorMat();
                            color.Nombre = parametrop.Nombre;
                            color.Usuario = parametrop.Usuario;
                            color.Estado = parametrop.Estado;
                            db.ColorMat.Add(color);
                            db.SaveChanges();
                            break;
                        case 3:
                            TipoProducto tipo = new TipoProducto();
                            tipo.Nombre = parametrop.Nombre;
                            tipo.Usuario = parametrop.Usuario;
                            tipo.Estado = parametrop.Estado;
                            db.TipoProducto.Add(tipo);
                            db.SaveChanges();
                            break;
                        case 4:
                            Rol rolp = new Rol();
                            rolp.Nombre = parametrop.Nombre;
                            rolp.Usuario = parametrop.Usuario;
                            rolp.Estado = parametrop.Estado;
                            db.Rol.Add(rolp);
                            db.SaveChanges();
                            break;
                    }

                }
                return RedirectToAction("Registrar");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parametro/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Parametro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parametro/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parametro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
