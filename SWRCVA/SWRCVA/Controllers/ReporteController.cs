using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class ReporteController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Reporte
        public ActionResult ReporteCotizacion()
        {
            return View();
        }

        // GET: Reporte
        public ActionResult ReporteFacturacion()
        {
            return View();
        }
        public JsonResult ConsultarClientes(string filtro)
        {
            var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro)
                            select new
                            {
                                s.IdCliente,
                                s.Nombre,
                                s.Telefono,
                                s.Correo
                            }).Take(5);

            return Json(Clientes,
             JsonRequestBehavior.AllowGet);

        }
    }
}