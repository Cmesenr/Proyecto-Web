function Editar(idCotizacion,idProducto) {
    var params = { idCotizacion: idCotizacion, idProducto:idProducto };
    $.ajax({
        cache: false,
        url: "/Orden/Editar",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            parent.document.location = parent.document.location;
        }
    });
}

function AlmacenarEstado(estado) {
    var params = { estado: estado };
    $.ajax({
        cache: false,
        url: "/Orden/AlmacenarEstado",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            parent.document.location = parent.document.location;
        }
    });
}