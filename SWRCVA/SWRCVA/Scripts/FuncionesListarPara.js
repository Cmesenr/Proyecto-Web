$(document).ready(function ()
{
    $('#ListaParametro').on("change", function () {
        listar($(this).val());
    });
   
});
function listar(parametro) {
    var params = { id: parametro };
    $('#TableLista').html('<div align="center"><img src="../Content/Imagenes/loading.gif"/></div>');
    $.ajax({
        cache: false,
        url: "/Parametro/ListarParametros",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $('#TableLista').fadeIn(1000).html(result);
        }
    })
}
function EliminarParametro(valor, parametro) {
    var params = { id: valor, Tabla:parametro };
    confirm("Esta seguro de eliminar el parametro!");
    $.ajax({
        cache: false,
        url: "/Parametro/Eliminar",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            
            listar(parametro);
        }
    })
}
