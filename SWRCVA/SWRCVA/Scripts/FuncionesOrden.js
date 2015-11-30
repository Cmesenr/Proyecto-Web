function CargarListaProductos() {
    $('#ListaMateriales').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "scrollY": '200px',
        "scrollX": false,
        "scrollCollapse": false
    });
    $.ajax({
        cache: false,
        url: "/Orden/ConsultarListaProductos",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data.length == 0) {
                $("#ListaProductosOrden tbody").append('<tr><td align="center">Lo sentimos para ésta orden no existen productos</td></tr>');
            }
            else {
                $("#ListaProductosOrden tbody").empty();
                                  for (var i = 0; i < data.length; i++) {
                                      if (data[i].Ancho == 0 || data[i].Ancho == null) {
                                          data[i].Ancho = "n/a";
                                      }
                                      if (data[i].Alto == 0 || data[i].Alto == null) {
                                          data[i].Alto = "n/a";
                                      }
                                      if (data[i].ColorVidrio == 0 || data[i].ColorVidrio == null) {
                                          data[i].ColorVidrio = "n/a";
                                      }
                                      if (data[i].ColorAluminio == 0 || data[i].ColorAluminio == null) {
                                          data[i].ColorAluminio = "n/a";
                                      }
                                      if (data[i].ColorPaleta == 0 || data[i].ColorPaleta == null) {
                                          data[i].ColorPaleta = "n/a";
                                      }
                                      if (data[i].AnchoCelocia == 0 || data[i].AnchoCelocia == null) {
                                          data[i].AnchoCelocia = "n/a";
                                      }
                                      $('#ListaProductosOrden tbody').append('<tr class="warning">' +
                                              '<td>' + data[i].Nombre + '</td>' +
                                              '<td>' + data[i].CantMat + '</td>' +
                                              '<td>' + data[i].Ancho + '</td>' +
                                              '<td>' + data[i].Alto + '</td>' +
                                              '<td>' + data[i].ColorVidrio + '</td>' +
                                              '<td>' + data[i].ColorAluminio + '</td>' +
                                              '<td>' + data[i].ColorPaleta + '</td>' +
                                              '<td>' + data[i].AnchoCelocia + '</td>' +
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
    //Redirecionar a index cuando se guarda o procesa
    $('#ModalMensaje').on('hidden.bs.modal', function () {
        $("#ModalMensaje").removeData('bs.modal');
        window.location.href = "/Orden/index";
    });
    $("#formOrden").on("click", "#verDetalle", function () {
        var id = $(this).attr("data-id");
        $('#ListaMateriales tbody').html('<tr><td colspan="4"><center><img src="/Content/Imagenes/loading3.gif"/></center></td></tr>');
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
                $("#ListaMateriales tbody").fadeIn(1000).html();
                $("#ListaMateriales tbody").append('<tr><td colspan="2" align="center">Este producto no cuenta con detalle de materiales.</td></tr>');
            } else {

                $("#ListaMateriales tbody").empty();
                $("#ListaMateriales tbody").fadeIn(1000).html();
                
                for (var i = 0; i < data.length; i++) {
                        //t.row.add([
                        //    data[i].Nombre,
                        //    data[i].CantMaterial,
                        //]).draw(false);
                    $('#ListaMateriales tbody').append("<tr class='active'><td class='col-md-10'>" + data[i].Nombre + "</td><td class='col-md-10'>" + data[i].CantMaterial + "</td></tr>");
                }
                
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
     })
   })
})
