﻿using PagedList;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class ParametroController : Controller
    {
        DataContext db = new DataContext();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? tabla)
        {

            if (tabla != null)
            {
                Session["Currentabla"] = tabla;
            }
           
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Nombre" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var Parametros = new List<Parametro>();        
            var ParametroResult = Parametros.AsQueryable();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            if (tabla == null&& Session["Currentabla"] != null)
            {
                tabla =(int)Session["Currentabla"];
            }
            switch (tabla)
            {
                case 1:
                    ViewBag.CurrentFilter = searchString;
                    var Categorias = from s in db.CategoriaMat
                                     select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Categorias = Categorias.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in Categorias)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdCategoria;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();
                   
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                     pageSize = 5;
                     pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
                case 2:
                    ViewBag.CurrentFilter = searchString;
                    var colorMaterial = from s in db.ColorMat
                                     select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        colorMaterial = colorMaterial.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in colorMaterial)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdColor;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();                   
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                    pageSize = 5;
                    pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
                case 3:
                    ViewBag.CurrentFilter = searchString;
                    var tipoProducto = from s in db.TipoProducto
                                        select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        tipoProducto = tipoProducto.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in tipoProducto)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdTipoProducto;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();
                    
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                    pageSize = 5;
                    pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
                case 4:
                    ViewBag.CurrentFilter = searchString;
                    var rol = from s in db.Rol
                                       select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        rol = rol.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in rol)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdRol;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();
                    
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                    pageSize = 5;
                    pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
                case 5:
                    ViewBag.CurrentFilter = searchString;
                    var subCategoria = from s in db.SubCategoria
                              select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        subCategoria = subCategoria.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in subCategoria)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdSubCatMat;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();
                    
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                    pageSize = 5;
                    pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
                case 6:
                    ViewBag.CurrentFilter = searchString;
                    var valor = from s in db.Valor
                                       select s;
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        valor = valor.Where(s => s.Nombre.Contains(searchString)
                                                       );
                    }
                    foreach (var item in valor)
                    {

                        Parametro p1 = new Parametro();
                        p1.Id = item.IdValor;
                        p1.Nombre = item.Nombre;
                        p1.Estado = item.Estado.ToString();
                        Parametros.Add(p1);
                    }
                    ParametroResult = Parametros.AsQueryable();
                   
                    switch (sortOrder)
                    {
                        case "Nombre":
                            ParametroResult = ParametroResult.OrderByDescending(s => s.Nombre);
                            break;
                        default:  // Name ascending 
                            ParametroResult = ParametroResult.OrderBy(s => s.Nombre);
                            break;
                    }

                    pageSize = 5;
                    pageNumber = (page ?? 1);
                    return View(ParametroResult.ToPagedList(pageNumber, pageSize));
            }

            return View(ParametroResult.ToPagedList(pageNumber, pageSize));
            
        }
   

        // GET: Parametro/Registrar
        public ActionResult Registrar()
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            return View();
        }
        // POST: Parametro/Registrar
        [HttpPost]
        public ActionResult Registrar(Parametro parametrop)
        {
            try
            {
               ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
                if (!ModelState.IsValidField("CategoriaId")&& parametrop.parametro!=5|| !ModelState.IsValidField("Porcentaje") && parametrop.parametro != 6 || ModelState.IsValid)
                {
                    switch (parametrop.parametro) {
                        case 1:
                            CategoriaMat cat = new CategoriaMat();
                            cat.Nombre = parametrop.Nombre;
                            cat.Usuario = parametrop.Usuario;
                            cat.Estado = Convert.ToInt32(parametrop.Estado);
                            db.CategoriaMat.Add(cat);
                            db.SaveChanges();
                            break;
                        case 2:
                            ColorMat color = new ColorMat();
                            color.Nombre = parametrop.Nombre;
                            color.Usuario = parametrop.Usuario;
                            color.Estado = Convert.ToInt32(parametrop.Estado);
                            db.ColorMat.Add(color);
                            db.SaveChanges();
                            break;
                        case 3:
                            TipoProducto tipo = new TipoProducto();
                            tipo.Nombre = parametrop.Nombre;
                            tipo.Usuario = parametrop.Usuario;
                            tipo.Estado = Convert.ToInt32(parametrop.Estado);
                            db.TipoProducto.Add(tipo);
                            db.SaveChanges();
                            break;
                        case 4:
                            Rol rolp = new Rol();
                            rolp.Nombre = parametrop.Nombre;
                            rolp.Usuario = parametrop.Usuario;
                            rolp.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Rol.Add(rolp);
                            db.SaveChanges();
                            break;
                        case 5:
                            SubCategoria sub = new SubCategoria();
                            sub.Nombre = parametrop.Nombre;
                            sub.IdCatMat = parametrop.CategoriaId;
                            sub.Usuario = parametrop.Usuario;
                            sub.Estado = Convert.ToInt32(parametrop.Estado);
                            db.SubCategoria.Add(sub);
                            db.SaveChanges();
                            break;
                        case 6:
                            Valor val = new Valor();
                            val.Nombre = parametrop.Nombre;
                            val.Porcentaje = (parametrop.Porcentaje)/100;
                            val.Usuario = parametrop.Usuario;
                            val.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Valor.Add(val);
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
        // GET: Parametro/Editar/5
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Editar(int? id)
        {
            if (id == null|| Session["Currentabla"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            int tabla = (int)Session["Currentabla"];
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            Parametro para = new Parametro();
            switch (tabla) { 
                case 1:
                    CategoriaMat cat = new CategoriaMat();
                    cat = db.CategoriaMat.Find(id);
                    if (cat == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 1;
                    para.Id = cat.IdCategoria;
                    para.Nombre = cat.Nombre;
                    para.Usuario = cat.Usuario;
                    para.Estado = cat.Estado.ToString();

                    break;
                case 2:
                    ColorMat color = new ColorMat();
                    color = db.ColorMat.Find(id);
                    if (color == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 2;
                    para.Id = color.IdColor;
                    para.Nombre = color.Nombre;
                    para.Usuario = color.Usuario;
                    para.Estado = color.Estado.ToString();      
                    break;
                case 3:
                    TipoProducto tipo = new TipoProducto();
                    tipo = db.TipoProducto.Find(id);
                    if (tipo == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 3;
                    para.Id = tipo.IdTipoProducto;
                    para.Nombre = tipo.Nombre;
                    para.Usuario = tipo.Usuario;
                    para.Estado = tipo.Estado.ToString();
                    break;
                case 4:
                    Rol rolp = new Rol();
                    rolp = db.Rol.Find(id);
                    if (rolp == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 4;
                    para.Id = rolp.IdRol;
                    para.Nombre = rolp.Nombre;
                    para.Usuario = rolp.Usuario;
                    para.Estado = rolp.Estado.ToString();
                    break;
                case 5:
                    SubCategoria sub = new SubCategoria();
                    sub = db.SubCategoria.Find(id);
                    if (sub == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 5;
                    para.Id = sub.IdSubCatMat;
                    para.CategoriaId = sub.IdCatMat;
                    para.Nombre = sub.Nombre;
                    para.Usuario = sub.Usuario;
                    para.Estado = sub.Estado.ToString();
                    break;
                case 6:
                    Valor val = new Valor();
                    val = db.Valor.Find(id);
                    if (val == null)
                    {
                        return HttpNotFound();
                    }
                    para.parametro = 6;
                    para.Id = val.IdValor;
                    para.Porcentaje = (val.Porcentaje*100);
                    para.Nombre = val.Nombre;
                    para.Usuario = val.Usuario;
                    para.Estado = val.Estado.ToString();
                    break;
            }
            return PartialView(para);
        }

        // POST: Parametro/Editar/5
        [HttpPost]
        public ActionResult Editar(int id, Parametro parametrop)
        {
            try
            {
                parametrop.parametro= (int)Session["Currentabla"];
                ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
                if (!ModelState.IsValidField("CategoriaId") && parametrop.parametro != 5 || !ModelState.IsValidField("Porcentaje") && parametrop.parametro != 6 || ModelState.IsValid)
                {
                    switch (parametrop.parametro)
                    {
                        case 1:
                            CategoriaMat cat = new CategoriaMat();
                            cat.IdCategoria = id;
                            cat.Nombre = parametrop.Nombre;
                            cat.Usuario = parametrop.Usuario;
                            cat.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(cat).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case 2:
                            ColorMat color = new ColorMat();
                            color.IdColor = id;
                            color.Nombre = parametrop.Nombre;
                            color.Usuario = parametrop.Usuario;
                            color.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(color).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case 3:
                            TipoProducto tipo = new TipoProducto();
                            tipo.IdTipoProducto = id;
                            tipo.Nombre = parametrop.Nombre;
                            tipo.Usuario = parametrop.Usuario;
                            tipo.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(tipo).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case 4:
                            Rol rolp = new Rol();
                            rolp.IdRol = id;
                            rolp.Nombre = parametrop.Nombre;
                            rolp.Usuario = parametrop.Usuario;
                            rolp.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(rolp).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case 5:
                            SubCategoria sub = new SubCategoria();
                            sub.IdSubCatMat = id;
                            sub.Nombre = parametrop.Nombre;
                            sub.IdCatMat = parametrop.CategoriaId;
                            sub.Usuario = parametrop.Usuario;
                            sub.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(sub).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        case 6:
                            Valor val = new Valor();
                            val.IdValor = id;
                            val.Nombre = parametrop.Nombre;
                            val.Porcentaje = (parametrop.Porcentaje) / 100;
                            val.Usuario = parametrop.Usuario;
                            val.Estado = Convert.ToInt32(parametrop.Estado);
                            db.Entry(val).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                    }

                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Eliminar(int? id)
        {
            int tabla = (int)Session["Currentabla"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            switch (tabla)
            {
                case 1:
                    CategoriaMat cat = new CategoriaMat();
                    cat = db.CategoriaMat.Find(id);
                    if (cat == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(cat).Entity.Estado =0;
                    db.SaveChanges();
                    break;
                case 2:
                    ColorMat color = new ColorMat();
                    color=db.ColorMat.Find(id);
                    if (color == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(color).Entity.Estado=0;
                    db.SaveChanges();
                    break;
                case 3:
                    TipoProducto tipo = new TipoProducto();
                    tipo = db.TipoProducto.Find(id);
                    if (tipo == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(tipo).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 4:
                    Rol rolp = new Rol();
                    rolp = db.Rol.Find(id);
                    if (rolp == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(rolp).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 5:
                    SubCategoria sub = new SubCategoria();
                    sub = db.SubCategoria.Find(id);
                    if (sub == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(sub).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 6:
                    Valor val = new Valor();
                    val = db.Valor.Find(id);
                    if (val == null)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(val).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
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
