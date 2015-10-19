function EliminarProveedor(valor) {
    var params = { id: valor };
    if (confirm("Esta seguro de eliminar el proveedor!")==true) {
        $.ajax({
            cache: false,
            url: "/Proveedor/Borrar",
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