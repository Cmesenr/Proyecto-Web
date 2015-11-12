﻿$(document).ready(function () {
    $("#txtClienteModal").keypress(function () {
        var params = {filtro:$("#txtClienteModal").val()};
        $.ajax({
            cache: false,
            url: "/Cotizacion/ConsultarClientes",
            type: "get",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#TableCliente").empty();
                $("#TableCliente").append('<tr><th>Cliente</th><th>Telefono</th><th>Correo</th><th></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#TableCliente').append('<tr>' +
                                              '<td>' + data[i].Nombre + '</td>' +
                                              '<td>' + data[i].Telefono + '</td>' +
                                              '<td>' + data[i].Correo + '</td>' +
                                              '<td> <button type="button" id="SeleccionarCliente" class="btn btn-default btn-sm"><span data-id=' + data[i].IdCliente + ' class="glyphicon glyphicon-ok" /></button></td>' +
                                            '</tr>');
           
                }
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }

        })
    })
    $("#txtClienteFinal").on("click", function () {
        $("#ModalCliente").modal("show");
    })
    $('#DropDownProductos').on("change", function () {
        ConsultarImagen($('#DropDownProductos').val());
    })
    $('#DropDownTipoProductos').on("change", function () {
        var id = $('#DropDownTipoProductos').val();
        $.ajax({
            cache: false,
            url: "/Cotizacion/ConsultarProductos",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var items = "<option value=''>Productos..</option>";
                for (var i = 0; i < data.length; i++) {

                    items += "<option value='" + data[i].IdProducto + "'>" + data[i].Nombre + "</option>";
                }

                $("#DropDownProductos").html(items);

            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        })
    })
    $("#formCotizar").on("click", "#btnAgregar", function () {
        
        if ($('#DropDownTipoProductos')[0].checkValidity() == false) {
            $("#DropDownTipoProductos").tooltip();
            $("#DropDownTipoProductos").focus();
            return false;
        }
        else if ($('#DropDownProductos')[0].checkValidity() == false) {
            $("#DropDownProductos").tooltip();
            $("#DropDownProductos").focus();
            return false;
        } else if ($('#DropDownCVidrio')[0].checkValidity() == false) {           
            $("#DropDownCVidrio").tooltip();
            $("#DropDownCVidrio").focus();
            return false;
        } else if ($('#DropDownCAluminio')[0].checkValidity() == false) {
            $("#DropDownCAluminio").tooltip();
            $("#DropDownCAluminio").focus();    
            return false;
        } else if ($('#DropDownInstalacion')[0].checkValidity() == false) {           
            $("#DropDownInstalacion").tooltip();
            $("#DropDownInstalacion").focus();
            return false;
        }
        else if ($('#DropDownVidrio')[0].checkValidity() == false) {
            $("#DropDownVidrio").tooltip();
            $("#DropDownVidrio").focus();
            return false;
        }
        else if ($('#txtCantidad')[0].checkValidity() == false) {
            $("#txtCantidad").tooltip();
            $("#txtCantidad").focus();           
            return false;
        } else if ($('#txtAncho')[0].checkValidity() == false) {           
            $("#txtAncho").tooltip();
            $("#txtAncho").focus();
            return false;
        } else if ($('#txtAlto')[0].checkValidity() == false) {           
            $("#txtAlto").tooltip();
            $("#txtAlto").focus();
            return false;
        }
        var paraProd = { Idpro: $('#DropDownProductos').val(), Cvidrio: $("#DropDownCVidrio").val(), CAluminio: $('#DropDownCAluminio').val(), Insta: $('#DropDownInstalacion').val(), Cant: $('#txtCantidad').val(), Ancho: $('#txtAncho').val(), Alto: $('#txtAlto').val(), vidrio: $('#DropDownVidrio').val() };
               $.ajax({
                cache: false,
                url: "/Cotizacion/AgregarProducto",
                type: "get",
                data: paraProd,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (typeof (data) == "string") {
                        $("#TextModal").html(data);
                        $('#ModalError').modal("show");
                    }
                    else {
                        if (data[0].IdProducto == null) {
                            var Resultado = "<center>El o los materiales siguienetes no poseen el color selecionado:</center> <ul>";
                            for (var i = 0; i < data.length; i++) {
                                Resultado +="<li>"+ data[i]+"</li>";
                            }
                            Resultado += "</ul>";
                            $("#TextModal").html(Resultado);
                            $('#ModalError').modal("show");
                        }
                        else{
                        $("#ListaProductos").empty();
                        $("#ListaProductos").append('<tr><th>Producto</th><th>Cantidad</th><th>Subtotal</th><th></th></tr>');

                        for (var i = 0; i < data.length; i++) {
                            $('#ListaProductos').append('<tr>' +
                                                  '<td>' + data[i].Nombre + '</td>' +
                                                   '<td>' + data[i].Cantidad + '</td>' +
                                                  '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                  '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                '</tr>');
                        }
                        CalcularTotal();
                        }

                    }
                },
                error: function (result) {
                    alert('ERROR ' + result.status + ' ' + result.statusText);
                }

            })
      
    })
    $("#formCotizar").on("click", "#eliminarProducto", function () {
        var id = $(this).attr("data-id");
        $.ajax({
            cache: false,
            url: "/Cotizacion/EliminarProducto",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#ListaProductos").empty();
                $("#ListaProductos").append('<tr><th>Producto</th><th>Cantidad</th><th>Subtotal</th><th></th></tr>');

                for (var i = 0; i < data.length; i++) {
                    $('#ListaProductos').append('<tr>' +
                                          '<td>' + data[i].Nombre + '</td>' +
                                           '<td>' + data[i].Cantidad + '</td>' +
                                          '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                          '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                        '</tr>');
                }
                CalcularTotal();
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        })

    })

})
function CalcularTotal() {
    $.ajax({
        cache: false,
        url: "/Cotizacion/CalcularTotal",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#txtTotal").html("₡ " + data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        }
    })
}
function ConsultarImagen(id) {
    var para = {id:id};
    $.ajax({
        cache: false,
        url: "/Cotizacion/ConsultarImagen",
        type: "get",
        data: para,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#MostrarImagen').attr('src', "data:image/png;base64," + data)
        }
    })
}
var nav4 = window.Event ? true : false;
function acceptNum(evt) {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57, '.' = 46
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}