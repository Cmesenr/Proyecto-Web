$(document).ready(function ()
{
    $('#ListaParametro').on("change", function () {
        var params = { id: $(this).val() };
        $('#TableLista').html('<div align="center"><img src="../Content/Imagenes/loading.gif"/></div>');
            $.ajax({
                url: "/Parametro/ListarParametros",
                type: "GET",
                data: params,
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $('#TableLista').fadeIn(1000).html(result);
                }
            })
    });
  


});