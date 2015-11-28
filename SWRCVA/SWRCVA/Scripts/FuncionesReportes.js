$(function () {
    $("#formReporte").on("click", "#SeleccionarCliente", function (e) {
        $("#txtNombre").val($(this).data("nombre"));
        $("#txtIdCliente").val($(this).data("myvalue"));
        $("#ModalCliente").modal("hide");
    })
    $("#txtNombre").on("click", function () {
        $("#ModalCliente").modal("show");
    })
    $("#txtClienteModal").on("keypress", function () {
        var params = { filtro: $("#txtClienteModal").val() };
        $('#TableCliente').html('<center><img src="/Content/Imagenes/Cargando.gif"/></center>');
        $.ajax({
            cache: false,
            url: "/Reporte/ConsultarClientes",
            type: "get",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#TableCliente").empty();
                $("#TableCliente").fadeIn(1000).html();
                $("#TableCliente").append('<tr><th>Cliente</th><th>Telefono</th><th>Correo</th><th></th></tr>');

                for (var i = 0; i < data.length; i++) {
                    $('#TableCliente').append('<tr>' +
                                          '<td>' + data[i].Nombre + '</td>' +
                                          '<td>' + data[i].Telefono + '</td>' +
                                          '<td>' + data[i].Correo + '</td>' +
                                          '<td> <button type="button" id="SeleccionarCliente" class="btn btn-default btn-sm" data-nombre="' + data[i].Nombre + '"  data-myvalue="' + data[i].IdCliente + '"><span class="glyphicon glyphicon-ok" /></button></td>' +
                                        '</tr>');

                }
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }

        })
    })

    $("#btnBuscar").on("click", function () {
        
        var params = { fechaInicio: $("#txtFechaInicio").val(), FechaFin: $("#txtFechaFin").val(), IdCliente: $("#txtIdCliente").val(), reporte: "Cotizacion" };
        $.ajax({
            cache: false,
            url: "/Reporte/ConsultarReporte",
            type: "get",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

            }
        })
    })


    $("#Procesar").on("click", function () {

        var txtFechaInicio = $("#FechaInicio").val();
        var txtFechaFin = $("#FechaFin").val();

        if (txtFechaInicio === "") {
            $("#FechaInicio").tooltip();
            $("#FechaInicio").focus();
            return false;
        }
        if (txtFechaFin === "") {
            $("#FechaFin").tooltip();
            $("#FechaFin").focus();
            return false;
        }
    })

});

