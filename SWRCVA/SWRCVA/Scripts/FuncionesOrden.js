function Editar(valor) {
    var params = { id: valor };
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