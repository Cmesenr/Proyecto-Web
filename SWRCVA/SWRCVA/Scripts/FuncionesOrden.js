function CargarListaProductos() {
    $.ajax({
        cache: false,
        url: "/Orden/ConsultarListaProductos",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data.length == 0) {
                $("#ListaProductos").append('<tr><th>Lo sentimos para ésta orden no existen productos</th></tr>');
            }
            else {
                    $("#ListaProductos").empty();
                    $("#ListaProductos").append('<tr><th class="active" align="Center">Código</th><th class="active" align="left">Nombre</th><th class="active" align="Center">Cantidad</th><th class="active" align="Center">Ancho</th><th class="active" align="Center">Alto</th><th class="active"></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#ListaProductos').append('<tr class="warning">' +
                                              '<td class="col-md-1" align="Center">' + data[i].IdProducto + '</td>' +
                                              '<td class="col-md-5" align="Left">' + data[i].Nombre + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Cantidad + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Ancho + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Alto + '</td>' +
                                              '<td class="col-md-1" align="Center"><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="Ver materiales" /></td>' +
                                            '</tr>');
                }
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
    })
}
//Terminar Orden
$("#btnProcesarEdit").on("click", function () {
    $.ajax({
        cache: false,
        url: "/Orden/ProcesarOrden",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == "Orden Terminada!") {
                $("#TextModalinfo").html(data);
                $("#HeaderModalInfo").html("Procesado");
                $('#ModalMensaje').modal("show");
            }
        }
    })

})
$(document).ready(function () {
$("#formOrden").on("click", "#eliminarProducto", function () {
    var id = $(this).attr("data-id");
    $.ajax({
        cache: false,
        url: "/Orden/ConsultarMateriales",
        type: "get",
        data: { idProducto : id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#ListaMateriales").empty();
            $("#ListaMateriales").append('<tr class="active"><th>Producto</th><th>Cotización</th><th>Material</th></tr>');

            for (var i = 0; i < data.length; i++) {
                $('#ListaMateriales').append('<tr class="warning">' +
                                      '<td>' + data[i].IdMaterial + '</td>' +
                                      '<td>' + data[i].Nombre + '</td>' +
                                      '<td>' + data[i].Costo + '</td>' +
                                      '</tr>');
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
     })
   })
})
