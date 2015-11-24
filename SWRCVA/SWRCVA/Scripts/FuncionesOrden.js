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
                    $("#ListaProductos").append('<tr><th class="active" align="left">Nombre</th><th class="active" align="Center">Cantidad</th><th class="active" align="Center">Ancho</th><th class="active" align="Center">Alto</th><th class="active" align="Center">Color Vidrio</th><th class="active" align="Center">Color Aluminio</th><th class="active" align="Center">Ancho Celocia</th><th class="active"></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#ListaProductos').append('<tr class="warning">' +
                                              '<td class="col-md-5" align="Left">' + data[i].Nombre + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Cantidad + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Ancho + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].Alto + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].ColorVidrio + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].ColorAluminio + '</td>' +
                                              '<td class="col-md-1" align="Center">' + data[i].AnchoCelocia + '</td>' +
                                              '<td class="col-md-1" align="Center"><input type="button" id="verDetalle" data-id=' + data[i].IdProducto + ' class="btn btn-primary" value="Ver materiales" /></td>' +
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
    $("#formOrden").on("click", "#verDetalle", function () {
    var id = $(this).attr("data-id");
    $.ajax({
        cache: false,
        url: "/Orden/ConsultarMateriales",
        type: "get",
        data: { idProducto : id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.length == 0) {
                $("#ListaMateriales tbody").empty();
                $("#ListaMateriales tbody").append('Este producto no cuenta con detalle de materiales.');
            } else {
                $("#ListaMateriales tbody").empty();
                for (var i = 0; i < data.length; i++) {
                    $('#ListaMateriales tbody').append("<tr class='active'><td align='left'>" + data[i].Nombre + "</td><td align='center'>" + data[i].CantMaterial + "</td></tr>");
                }
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
     })
   })
})
