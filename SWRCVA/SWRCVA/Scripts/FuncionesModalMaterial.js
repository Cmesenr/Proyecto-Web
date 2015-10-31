$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        var data = $(e.relatedTarget).data().info;
        $(e.currentTarget).find('#btnModalborrar').val(id);
        $(e.currentTarget).find('#TextModal').html("Esta seguro que desea borrar el Material " + data + " ?");
    });
});

$(".edit").click(function () {
       var id = $(this).attr("data-id");
   $("#modal").load("/Material/Editar/" + id, function () {
        $('#modal').modal("show");
   })
 
});
function EliminarMaterial(valor) {

 var params = { id: valor };
  $.ajax({
                    cache: false,
                    url: "/Material/Borrar",
                    type: "GET",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        parent.document.location = parent.document.location;
                    }
   });

}
function RefrescarLista() {
    $.ajax({
        cache: false,
        url: "/Material/RefrescarLista",
        type: "GET",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    });

}
