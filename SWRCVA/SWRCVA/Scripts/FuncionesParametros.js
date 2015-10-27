
$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        $(e.currentTarget).find('#btnModalborrar').val(id);
    });
    $('#modal').on('hidden.bs.modal', function (e) {
        $("#modal").removeData('bs.modal');
        parent.document.location = parent.document.location;
    });
});

$(document).ready(function ()
{
    $(function () { $("input,select,textarea").not("[type=submit]").jqBootstrapValidation(); });

EditarPara();
     
});
$(".edit").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("/Parametro/Editar/" + id, function () {
        $('#modal').modal("show");
    })
});

function EditarPara() {
  
        if ($('#ListaParametro').val() == 5) {
            $('#Listacategoria').slideDown();
            $('#Porcentaje').slideUp();
        }
        if ($('#ListaParametro').val() == 2) {
            $('#Listacategoria').slideDown();
            $('#Porcentaje').slideUp();
        }
        if ($('#ListaParametro').val() == 6) {
            $('#Porcentaje').slideDown();
            $('#Listacategoria').slideUp();
        }
        if ($('#ListaParametro').val() != 5 && $('#ListaParametro').val() != 6 && $('#ListaParametro').val() != 2) {
            $('#Porcentaje').slideUp();
            $('#Listacategoria').slideUp();
        }

}
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
function EliminarParametro(valor) {
    var params = { id: valor };
    $.ajax({
        cache: false,
        url: "/Parametro/Eliminar",
        type: "Post",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    })
}
