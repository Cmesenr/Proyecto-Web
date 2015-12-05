using System.Web;
using System.Web.Optimization;

namespace SWRCVA
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-iu").Include(
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                 "~/Content/bootstrap.css",
                                 "~/Content/themes/base/jquery.ui.css",
                                 "~/Content/site.css"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/bonnet").Include(
          "~/Scripts/jquery.bonnet*"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/animate.css",
                      "~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesParametros").Include(
                "~/Scripts/FuncionesParametros.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesProveedor").Include(
            "~/Scripts/FuncionesProveedor.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesCliente").Include(
                "~/Scripts/FuncionesCliente.js")); 
                bundles.Add(new ScriptBundle("~/bundles/FuncionesMaterial").Include(
                "~/Scripts/FuncionesMaterial.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesModalMaterial").Include(
                "~/Scripts/FuncionesModalMaterial.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesProducto").Include(
               "~/Scripts/FuncionesProducto.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesUsuario").Include(
               "~/Scripts/FuncionesUsuario.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesLogin").Include(
               "~/Scripts/FuncionesLogin.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesCotizacion").Include(
              "~/Scripts/FuncionesCotizacion.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesOrden").Include(
              "~/Scripts/FuncionesOrden.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesReportes").Include(
              "~/Scripts/FuncionesReportes.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqBootstrapValidation").Include(
              "~/Scripts/jqBootstrapValidation.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesFacturar").Include(
            "~/Scripts/FuncionesFacturar.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesTicket").Include(
            "~/Scripts/FuncionesTicket.js"));
            bundles.Add(new ScriptBundle("~/bundles/FuncionesRespaldo").Include(
            "~/Scripts/FuncionesRespaldo.js"));
            bundles.Add(new ScriptBundle("~/bundles/ReciboDinero").Include(
           "~/Scripts/ReciboDinero.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
            "~/Scripts/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dataTablesBoot").Include(
            "~/Scripts/dataTables.bootstrap.min.js"));
        }
    }
}
