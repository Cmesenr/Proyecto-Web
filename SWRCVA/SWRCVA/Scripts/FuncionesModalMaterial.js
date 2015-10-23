$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        $(e.currentTarget).find('#btnModalborrar').val(id);
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

