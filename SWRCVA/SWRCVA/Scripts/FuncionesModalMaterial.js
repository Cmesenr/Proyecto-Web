$(document).ready(function () {

});

$(".edit").click(function () {
       var id = $(this).attr("data-id");
   $("#modal").load("/Material/Editar/" + id, function () {
        $('#modal').modal("show");
    })
   /*window.showModalDialog("/Material/Editar/" + id, "", "dialogWidth:600px;dialogHeight:900px");*/
});
function EliminarMaterial(valor) {

    var params = { id: valor };
    var resultado=confirm("Esta seguro de eliminar el proveedor!"); 
    if (resultado){$.ajax({
                    cache: false,
                    url: "/Material/Borrar",
                    type: "GET",
                    data: params,
                    contentType: "application/json; charset=utf-8",
                    success: function (result) {
                        parent.document.location = parent.document.location;
                    }
   });
    } else {
        return false;
    }

}

