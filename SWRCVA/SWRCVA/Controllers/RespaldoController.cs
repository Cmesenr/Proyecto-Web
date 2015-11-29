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
            LoginController login = new LoginController();
            if (!login.validaUsuario(Session))
                return RedirectToAction("Login", "Login");

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

        private SqlConnection AbrirConexion()
        {
            SqlConnection conn;

            string stringConexion = "Data Source = Angel\\Anca; User Id = UserBackup; Password = UserBackup";
            conn = new SqlConnection(stringConexion);

            return conn;
        }

        private void EjecutarRespaldo()
        {
            try
            {
                SqlConnection conn = AbrirConexion();
                conn.Open();

                string sql = "BACKUP DATABASE BDVIDASA TO DISK = 'C:\\Program Files\\Microsoft SQL Server\\MSSQL12.ANCA\\MSSQL\\Backup\\BDVIDASA.bak'";

                SqlCommand command = new SqlCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();
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
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Algo salió mal con esta operación. Por favor inténtelo de nuevo.");
            }
        }
    }
}