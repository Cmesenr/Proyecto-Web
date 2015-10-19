function EliminarCliente(valor) {
    var params = { id: valor };
    var respuesta = confirm("Esta seguro de eliminar el cliente!");
    if (respuesta) {
        $.ajax({
            cache: false,
            url: "/Cliente/Borrar",
            type: "GET",
            data: params,
            contentType: "application/json; charset=utf-8",
            success: function (result) {

            }
        });
    } else {
        return false;
    }
}