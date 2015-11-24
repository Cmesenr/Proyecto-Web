using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class ReporteController : Controller
    {
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
    }
}