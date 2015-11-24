using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SWRCVA.Reportes
{
    public partial class ReporteFacturacion : System.Web.UI.Page
    {
        private void CargarReporte(DateTime dt, string nombre, string tipoReporte)
        {
            Models.Reportes reporte = new Models.Reportes();

            ReportViewerFacturacion.LocalReport.DataSources.Clear();
            //ReportViewerFacturacion.LocalReport.DataSources.Add(new ReportDataSource("DSReporteFacturacion", reporte.reporteCotizacionFacturacion(dt, nombre, tipoReporte)));
            ReportViewerFacturacion.LocalReport.Refresh();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtBoxFecha.Text != "")
            {
                DateTime dt = DateTime.ParseExact(txtBoxFecha.Text, "MM/dd/yyyy", null);
                string Nombre = TextBoxNombre.Text;
                CargarReporte(dt, Nombre, "Facturacion");
            }
            else
            {
                txtBoxFecha.Focus();
            }
        }
    }
}