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
            return View();
        }

        // POST: Reporte
        [HttpPost]
        public ActionResult ReporteCotizacion(Reporte reporte)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportCotizacion.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteCotizacion", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, "Cotizacion")));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "inline; filename= Cotizacion." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download

            return View();
        }

        // GET: Reporte
        public ActionResult ReporteFacturacion()
        {
            return View();
        }

        // POST: Reporte
        [HttpPost]
        public ActionResult ReporteFacturacion(Reporte reporte)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer reportviewer = new ReportViewer();
            reportviewer.ProcessingMode = ProcessingMode.Local;
            reportviewer.LocalReport.ReportPath = "Reportes/ReportFacturacion.rdlc";
            reportviewer.LocalReport.DataSources.Add(new ReportDataSource("DSReporteFacturacion", reporteCotizacionFacturacion(reporte.FechaInicio, reporte.FechaFin, reporte.IdCliente, "Facturacion")));
            reportviewer.LocalReport.Refresh();

            byte[] bytes = reportviewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            //Response.AddHeader("content-disposition", "inline; filename= Cotizacion." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download

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

        public DataTable reporteCotizacionFacturacion(DateTime fechaInicio, DateTime fechaFin, int idCliente, string reporte)
        {
            string stProcedure = "";

            if (reporte == "Cotizacion")
            {
                stProcedure = "sp_getDatosReporteCotizacion";
            }
            else
            {
                stProcedure = "sp_getDatosReporteFacturacion";
            }

            string pConsulta = "set dateformat dmy; exec " + stProcedure + " '" + fechaInicio.ToShortDateString() + "','"
                + fechaFin.ToShortDateString() + "'," + idCliente;

            DataTable dt = new DataTable();
            SqlConnection conn = ((SqlConnection)db.Database.Connection);
            conn.Open();

            System.Data.SqlClient.SqlDataAdapter adaptador = new System.Data.SqlClient.SqlDataAdapter(pConsulta, conn);
            System.Data.DataTable dataTable = new System.Data.DataTable();
            adaptador.Fill(dt);

            conn.Close();

            return dt;
        }
    }
}