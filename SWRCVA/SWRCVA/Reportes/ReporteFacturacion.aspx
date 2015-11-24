<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteFacturacion.aspx.cs" Inherits="SWRCVA.Reportes.ReporteFacturacion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $("#txtBoxFecha").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <thead>
                <tr>
                    <th colspan="3" style="font-size:x-large;text-align:left" >
                Filtros Reporte Cotización
                    </th>
                </tr>
            </thead>
           <tr>
             <td>
              <asp:Label ID="LabelFecha" runat="server" Text="Por favor indique una fecha:"></asp:Label>
             </td>
             <td>
              <asp:TextBox ID="txtBoxFecha" runat="server" ReadOnly = "false"></asp:TextBox>
             </td>
           </tr>
           <tr>
             <td>
              <asp:Label ID="LabelNombre" runat="server" Text="Por favor escriba el nombre del cliente:"></asp:Label>
             </td>
             <td>
              <asp:TextBox ID="TextBoxNombre" runat="server" ReadOnly = "false"></asp:TextBox>
             </td>
             <td>
                  <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
             </td>
           </tr>
        </table>
        <rsweb:ReportViewer ID="ReportViewerFacturacion" runat="server" Width="825px"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
