using Microsoft.Reporting.WebForms;
using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteCotizacion(Reporte reporte)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportCotizacion.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteCotizacion", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, null, "Cotizacion", null)));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);

            return View();
        }

        // GET: Reporte
        public ActionResult ReporteFacturacion()
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteFacturacion(Reporte reporte)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportFacturacion.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteFacturacion", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, null, "Facturacion", reporte.Estado)));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);

            return View();
        }

        // GET: Reporte
        public ActionResult ReporteOrden()
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteOrden(Reporte reporte)
        {
            
            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportOrden.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteOrden", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, null, "Orden", reporte.Estado)));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);

            return View();
        }

        // GET: Reporte
        public ActionResult ReporteHistorico()
        {

            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteHistorico(Reporte reporte)
        {

            if (!LoginController.validaUsuario(Session))
                return RedirectToAction("Index", "Home");

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportHistorico.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteHistorico", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, reporte.Tabla, "Historico", null)));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);

            return View();
        }

        public JsonResult ConsultarClientes(string filtro)
        {
            var Clientes = (from s in db.Cliente
                            where s.Nombre.Contains(filtro) && s.Estado == 1
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

        public DataTable reporteCotizacionFacturacion(DateTime fechaInicio, DateTime fechaFin, int idCliente, string tabla, string reporte, string estado)
        {
            string stProcedure = "";
            string consulta = "";

            if (reporte == "Cotizacion")
            {
                stProcedure = "sp_getDatosReporteCotizacion";

                consulta = "set dateformat dmy; exec " + stProcedure + " '" + fechaInicio.ToShortDateString() + "','"
                    + fechaFin.ToShortDateString() + "'," + idCliente;
            }
            if (reporte == "Facturacion")
            {
                stProcedure = "sp_getDatosReporteFacturacion";

                consulta = "set dateformat dmy; exec " + stProcedure + " '" + fechaInicio.ToShortDateString() + "','"
                    + fechaFin.ToShortDateString() + "'," + idCliente + "," + int.Parse(estado);
            }
            if (reporte == "Orden")
            {
                stProcedure = "sp_getDatosReporteOrden";

                consulta = "set dateformat dmy; exec " + stProcedure + " '" + fechaInicio.ToShortDateString() + "','"

                    + fechaFin.ToShortDateString() + "'," + idCliente + ",'" + estado +"'";
            }
            if (reporte == "Historico")
            {
                stProcedure = "sp_getDatosReporteHistorico";

                consulta = "set dateformat dmy; exec " + stProcedure + " '" + fechaInicio.ToShortDateString() + "','"

                    + fechaFin.ToShortDateString() + "','" + tabla + "'";
            }

            DataTable dt = new DataTable();
            SqlConnection conn = ((SqlConnection)db.Database.Connection);
            conn.Open();

            System.Data.SqlClient.SqlDataAdapter adaptador = new System.Data.SqlClient.SqlDataAdapter(consulta, conn);
            System.Data.DataTable dataTable = new System.Data.DataTable();
            adaptador.Fill(dt);

            conn.Close();

            return dt;
        }
    }
}