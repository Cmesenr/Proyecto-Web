using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class ParametroController : Controller
    {
        DataContext db = new DataContext();

      
        public ActionResult Listar()
        {
            return View();
        }
        [HttpGet]
        public string ListarParametros(int id)
        {
            List<Parametro> para = new List<Parametro>();
            string resultado = "";
            switch (id)
            {
                case 1:
                    List<CategoriaMat> cat = new List<CategoriaMat>();
                    cat = db.CategoriaMat.ToList();
                   
                    foreach(var item in cat)
                    {
                        Parametro p1 = new Parametro();
                        p1.Id = item.IdCategoria;
                        p1.Nombre = item.Nombre;
                        if(item.Estado==1)
                            p1.Estado = "Activo";
                        else
                            p1.Estado = "Inactivo";
                        para.Add(p1);
                    }
                    
                    break;
                case 2:
                    List<ColorMat> color = new List<ColorMat>();
                    color = db.ColorMat.ToList();
                    
                    foreach (var item in color)
                    {
                        Parametro p1 = new Parametro();
                        p1.Id = item.IdColor;
                        p1.Nombre = item.Nombre;
                        if (item.Estado == 1)
                            p1.Estado = "Activo";
                        else
                            p1.Estado = "Inactivo";
                        para.Add(p1);
                    }
                    break;
                case 3:
                    List<TipoProducto> tipo = new List<TipoProducto>();
                    tipo = db.TipoProducto.ToList();
                    
                    foreach (var item in tipo)
                    {
                        Parametro p3 = new Parametro();
                        p3.Id = item.IdTipoProducto;
                        p3.Nombre = item.Nombre;
                        if (item.Estado == 1)
                            p3.Estado = "Activo";
                        else
                            p3.Estado = "Inactivo";
                        para.Add(p3);
                    }
                    break;
                case 4:
                    List<Rol> rol = new List<Rol>();
                    rol = db.Rol.ToList();
                    
                    foreach (var item in rol)
                    {
                        Parametro p4 = new Parametro();
                        p4.Id = item.IdRol;
                        p4.Nombre = item.Nombre;
                        if (item.Estado == 1)
                            p4.Estado = "Activo";
                        else
                            p4.Estado = "Inactivo";
                        para.Add(p4);
                    }
                    break;
                case 5:
                    List<SubCategoria> sub = new List<SubCategoria>();
                    sub = db.SubCategoria.ToList();
                  
                    foreach (var item in sub)
                    {
                        Parametro p5 = new Parametro();
                        p5.Id = item.IdSubCatMat;
                        p5.Nombre = item.Nombre;
                        if (item.Estado == 1)
                            p5.Estado = "Activo";
                        else
                            p5.Estado = "Inactivo";
                        para.Add(p5);
                    }
                    break;
                case 6:
                    List<Valor> valor = new List<Valor>();
                    valor = db.Valor.ToList();
                    
                    foreach (var item in valor)
                    {
                        Parametro p6 = new Parametro();
                        p6.Id = item.IdValor;
                        p6.Nombre = item.Nombre;
                        if (item.Estado == 1)
                            p6.Estado = "Activo";
                        else
                            p6.Estado = "Inactivo";
                        para.Add(p6);
                    }
                    break;
            }
            resultado= "<tr><th>Identificador</th><th>Nombre</th><th>Estado</th><th></th></tr>";
        foreach(var item in para)
            {
                resultado += "<tr><td>" + item.Id + "</td><td>" + item.Nombre + "</td><td>" + item.Estado +
                    "</td><td><a href='/Parametro/Editar/?id="+ item.Id+"&parametro="+id+ "'>Editar  </a>&nbsp&nbsp<a  onclick='EliminarParametro("+ item.Id+","+id+");'>Eliminar</a></td></tr> ";
            }
            return resultado;
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
        public ActionResult Editar(int id,int parametro)
        {
            ViewBag.CatMaterial = new SelectList(db.CategoriaMat, "IdCategoria", "Nombre");
            Parametro para = new Parametro();
            switch (parametro) { 
                case 1:
                    CategoriaMat cat = new CategoriaMat();
                    cat = db.CategoriaMat.Find(id);
                    para.Id = cat.IdCategoria;
                    para.Nombre = cat.Nombre;
                    para.Usuario = cat.Usuario;
                    para.Estado = cat.Estado.ToString();

                    break;
                case 2:
                    ColorMat color = new ColorMat();
                    color = db.ColorMat.Find(id);
                    para.Id = color.IdColor;
                    para.Nombre = color.Nombre;
                    para.Usuario = color.Usuario;
                    para.Estado = color.Estado.ToString();      
                    break;
                case 3:
                    TipoProducto tipo = new TipoProducto();
                    tipo = db.TipoProducto.Find(id);
                    para.Id = tipo.IdTipoProducto;
                    para.Nombre = tipo.Nombre;
                    para.Usuario = tipo.Usuario;
                    para.Estado = tipo.Estado.ToString();
                    break;
                case 4:
                    Rol rolp = new Rol();
                    rolp = db.Rol.Find(id);
                    para.Id = rolp.IdRol;
                    para.Nombre = rolp.Nombre;
                    para.Usuario = rolp.Usuario;
                    para.Estado = rolp.Estado.ToString();
                    break;
                case 5:
                    SubCategoria sub = new SubCategoria();
                    sub = db.SubCategoria.Find(id);
                    para.Id = sub.IdSubCatMat;
                    para.CategoriaId = sub.IdCatMat;
                    para.Nombre = sub.Nombre;
                    para.Usuario = sub.Usuario;
                    para.Estado = sub.Estado.ToString();
                    break;
                case 6:
                    Valor val = new Valor();
                    val = db.Valor.Find(id);
                    para.Id = val.IdValor;
                    para.Porcentaje = (val.Porcentaje*100);
                    para.Nombre = val.Nombre;
                    para.Usuario = val.Usuario;
                    para.Estado = val.Estado.ToString();
                    break;
            }
            return View(para);
        }

        // POST: Parametro/Editar/5
        [HttpPost]
        public ActionResult Editar(int id, Parametro parametrop)
        {
            try
            {
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
                return RedirectToAction("Listar");
            }
            catch
            {
                return View();
            }
        }

        // GET: Parametro/Delete/5  
        public void Eliminar(int id, int tabla)
        {
            switch (tabla)
            {
                case 1:
                    CategoriaMat cat = new CategoriaMat();
                    cat = db.CategoriaMat.Find(id);
                    db.Entry(cat).Entity.Estado =0;
                    db.SaveChanges();
                    break;
                case 2:
                    ColorMat color = new ColorMat();
                    color=db.ColorMat.Find(id);
                    db.Entry(color).Entity.Estado=0;
                    db.SaveChanges();
                    break;
                case 3:
                    TipoProducto tipo = new TipoProducto();
                    tipo = db.TipoProducto.Find(id);
                    db.Entry(tipo).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 4:
                    Rol rolp = new Rol();
                    rolp = db.Rol.Find(id);
                    db.Entry(rolp).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 5:
                    SubCategoria sub = new SubCategoria();
                    sub = db.SubCategoria.Find(id);
                    db.Entry(sub).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
                case 6:
                    Valor val = new Valor();
                    val = db.Valor.Find(id);
                    db.Entry(val).Entity.Estado = 0;
                    db.SaveChanges();
                    break;
            }
        }

    }
}
