using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SWRCVA.Models
{
    public class Reportes
    {
        private DataContext db = new DataContext();

        public DataTable reporteCotizacionFacturacion(DateTime fecha, string nombre, string reporte)
        {
            string stProcedure = "";

            if(reporte == "Cotizacion")
            {
                stProcedure = "sp_getDatosReporteCotizacion";
            }
            else
            {
                stProcedure = "sp_getDatosReporteFacturacion";
            }

            string pConsulta = "set dateformat dmy; exec "+ stProcedure +" '" + fecha.ToShortDateString() + "','"+ nombre +"'";

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