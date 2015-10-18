﻿using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class MaterialController : Controller
    {
        DataContext db = new DataContext();
        // GET: Material
        public ActionResult Index()
        {


            
            return View();
        }

        // GET: Material/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Material/Create
        public ActionResult Registrar()
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            ViewBag.ColorMaterial = new SelectList(db.ColorMat, "IdColor", "Nombre");
            ViewBag.SubCatMaterial = new SelectList(db.SubCategoria, "IdSubCatMat", "Nombre");
            ViewBag.Proveedor = new SelectList(db.Proveedor, "IdProveedor", "Nombre");
            return View();
        }

        // POST: Material/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Material/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Material/Edit/5
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

        // GET: Material/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Material/Delete/5
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
