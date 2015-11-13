$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        var data = $(e.relatedTarget).data().info;
        $(e.currentTarget).find('#btnModalborrar').val(id);
        $(e.currentTarget).find('#TextModal').html("Esta seguro que desea borrar el Cliente "+data+" ?");
    });

});
$(document).ready(function (e) {
    $('#modal').on('hidden.bs.modal', function (e) {
        $("#modal").removeData('bs.modal');
        parent.document.location = parent.document.location;
    });
});
$(".edit").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("/Cliente/Editar/" + id, function () {
        $('#modal').modal("show");
    })
});

function EliminarCliente(valor) {
    var params = { id: valor };
        $.ajax({
            cache: false,
            url: "/Cliente/Borrar",
            type: "GET",
            data: params,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                parent.document.location = parent.document.location;
            }
        });
}
var nav4 = window.Event ? true : false;
function acceptonlyNum(evt) {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57, '.' = 46
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57));
}