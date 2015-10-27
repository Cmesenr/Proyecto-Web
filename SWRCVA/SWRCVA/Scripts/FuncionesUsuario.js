$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        var data = $(e.relatedTarget).data().info;
        $(e.currentTarget).find('#btnModalborrar').val(id);
        $(e.currentTarget).find('#TextModal').html("Esta seguro que desea borrar el Usuario "+data+" ?");
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
    $("#modal").load("/Usuario/Editar/" + id, function () {
        $('#modal').modal("show");
    })
});

function EliminarUsuario(valor) {
    var params = { id: valor }; 
        $.ajax({
            cache: false,
            url: "/Usuario/Borrar",
            type: "GET",
            data: params,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                parent.document.location = parent.document.location;
            }
        });

}