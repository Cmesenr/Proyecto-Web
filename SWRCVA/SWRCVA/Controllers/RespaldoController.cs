using SWRCVA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWRCVA.Controllers
{
    public class RespaldoController : Controller
    {
        // GET: Respaldo
        public ActionResult RepaldarBD(string tipoOperacion)
        {
            /*if (Session["UsuarioActual"] == null || Session["RolUsuarioActual"].ToString() != "Administrador")
            {
                return RedirectToAction("Login", "Login");
            }*/

            if (tipoOperacion == "Respaldo")
            {
                EjecutarRespaldo();
            }
            if(tipoOperacion == "Restauracion")
            {
                EjecutarRestauracion();
            }

            return View();
        }

        public SqlConnection AbrirConexion()
        {
            SqlConnection conn;

            string stringConexion = "Data Source = localhost ; User Id = sa; Password = 123.";
            conn = new SqlConnection(stringConexion);

            return conn;
        }


        public void EjecutarRespaldo()
        {
            try
            {
                SqlConnection conn = AbrirConexion();
                conn.Open();

                string sql = "BACKUP DATABASE BDVIDASA TO DISK = 'C:\\Program Files\\Microsoft SQL Server\\MSSQL12.ANCA\\MSSQL\\Backup\\BDVIDASA.bak'";

                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();

                //ViewBag.Message("Se hizo el respaldo de la Base de Datos exitosamente.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Algo salió mal con esta operación. Por favor inténtelo de nuevo.");
            }
        }

        private void EjecutarRestauracion()
        {
            try
            {
                SqlConnection conn = AbrirConexion();
                conn.Open();

                string sql = "ALTER DATABASE BDVIDASA SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                sql += "RESTORE DATABASE BDVIDASA FROM DISK ='C:\\Program Files\\Microsoft SQL Server\\MSSQL12.ANCA\\MSSQL\\Backup\\BDVIDASA.bak' WITH REPLACE";

                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();

                //ViewBag.Message("Se hizo el respaldo de la Base de Datos exitosamente.");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Algo salió mal con esta operación. Por favor inténtelo de nuevo.");
            }
        }
    }
}