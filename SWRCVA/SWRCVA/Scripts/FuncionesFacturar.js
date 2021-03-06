﻿$(document).ready(function () {
    $('#ListaProductos').DataTable({
        "paging": false,
        "ordering": false,
        "info": false,
        "searching": false,
        "scrollY": '200px',
        "scrollX": false,
        "scrollCollapse": false
    });
    $("#divLoading").addClass('show');
    $.ajax({
        cache: false,
        url: "/Factura/ConsultarMateriales",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#TableMateriales tbody").empty();
            $("#TableMateriales").append('<tbody>');
            for (var i = 0; i < data.length; i++) {
                $('#TableMateriales').append('<tr>' +
                                      '<td>' + data[i].Id + '</td>' +
                                      '<td>' + data[i].Nombre + '</td>' +
                                      '<td>' + data[i].Categoria + '</td>' +
                                      '<td>' + data[i].Color + '</td>' +
                                      '<td>' + "₡ " +data[i].Costo.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                      '<td> <button type="button" id="SeleccionarMaterial" class="btn btn-default btn-sm" data-idcolor="' + data[i].IdColor + '" data-cat="' + data[i].Categoria + '" data-costo="' + data[i].Costo + '"  data-myvalue="' + data[i].Id + '"><span class="glyphicon glyphicon-ok" /></button></td>' +
                                    '</tr>');

            }
            $("#TableMateriales").append('</tbody>');
            $('#TableMateriales').DataTable();
            $("#divLoading").fadeOut();
            $("#divLoading").removeClass('show');
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }

    })
})

$(document).ready(function () {

    $.extend(true, $.fn.dataTable.defaults, {
        "lengthMenu": [[5,10,15,-1], [5,10,15, "All"]],
        "language": {
            "url": "../Scripts/Spanish.json"
        }
    });

   
    //Listar Clientes
    $("#txtClienteModal").on("keypress", function () {
        var params = { filtro: $("#txtClienteModal").val() };
        $('#TableCliente').html('<center><img src="/Content/Imagenes/Cargando.gif"/></center>');
        $.ajax({
            cache: false,
            url: "/Cotizacion/ConsultarClientes",
            type: "get",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#TableCliente").empty();
                $("#TableCliente").fadeIn(1000).html();
                $("#TableCliente").append('<tr><th>Cliente</th><th>Telefono</th><th>Correo</th><th></th></tr>');

                for (var i = 0; i < data.length; i++) {
                    $('#TableCliente').append('<tr>'+
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
    //Abrir modal  Material
    $("#btnBuscarMat").on("click", function () {
        $("#ModalMateriales").modal("show");
    })
    //cargar datos a modal eliminar
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        var data = $(e.relatedTarget).data().info;
        $(e.currentTarget).find('#btnModalborrar').val(id);
        $(e.currentTarget).find('#TextModal').html("Esta seguro que desea Anular la Factura " + data + " ?");
    });
    //Selecionar El Material
    $("#headerPrincipal").on("click", "#SeleccionarMaterial", function (e) {
        $("#txtProducto").val($(this).data("myvalue"));
        $("#txtProducto").data("costo", $(this).data("costo"));
        $("#txtProducto").data("idcolor", $(this).data("idcolor"));
        VerificarMaterial($("#txtProducto").val());
        $("#ModalMateriales").modal("hide");

    })

    //Selecionar El CLiente
    $("#headerPrincipal").on("click", "#SeleccionarCliente", function (e) {
        $("#txtClienteFinal").val($(this).data("nombre"));
        $("#txtClienteFinal").data("cliente", $(this).data("myvalue"));
        $("#ModalCliente").modal("hide");
    })
    //Mostrar el modal CLiente
    $("#txtClienteFinal").on("click", function () {
        $("#ModalCliente").modal("show");

    })
    //estableser el focus al levantar el modal
    $('#ModalCliente').on('shown.bs.modal', function () {
        $("#txtClienteModal").focus();
    });
    //Guardar Cliente 
    $("#headerPrincipal").on("click", "#btnGuardarModal", function () {
        if ($('#txtNombreCliente')[0].checkValidity() == false) {
            $("#txtNombreCliente").tooltip();
            $("#txtNombreCliente").focus();
            return false;
        }
        else if ($('#txtTelefonoCliente')[0].checkValidity() == false) {
            $("#txtTelefonoCliente").tooltip();
            $("#txtTelefonoCliente").focus();
            return false;
        } else if ($('#txtCorreoCliente')[0].checkValidity() == false) {
            $("#txtCorreoCliente").tooltip();
            $("#txtCorreoCliente").focus();
            return false;
        } else if ($('#txtDireccionCliente')[0].checkValidity() == false) {
            $("#txtDireccionCliente").tooltip();
            $("#txtDireccionCliente").focus();
            return false;
        }

        var para = { Nombre: $('#txtNombreCliente').val(), Telefono: $('#txtTelefonoCliente').val(), Correo: $('#txtCorreoCliente').val(), Direccion: $('#txtDireccionCliente').val() };
        $.ajax({
            cache: false,
            url: "/Factura/GuardarCliente",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (typeof (data) == "string") {
                    $("#TextModal").html(data);
                    $('#ModalError').modal("show");
                }
                else {
                    $('#ModalCrearCliente').modal("hide");
                    $("#ModalCliente").modal("hide");
                    $("#txtClienteFinal").val(data.Nombre);
                    $("#txtClienteFinal").data("cliente", data.IdCliente);
                }

            }
        })

    })
    //limpiar campos de crear cliente
    $('#ModalCrearCliente').on('hidden.bs.modal', function () {
        $('#txtNombreCliente').val("");
        $('#txtTelefonoCliente').val("");
        $('#txtCorreoCliente').val("");
        $('#txtDireccionCliente').val("");
    });
    //Limpiar campo buscar cliente
    $('#ModalCliente').on('hidden.bs.modal', function () {
        $("#txtClienteModal").val("");
    });
    //Agregar Producto 
    $("#formFacturar").on("click", "#btnAgregarMat", function () {
        if ($('#txtProducto')[0].checkValidity() == false) {
            $("#txtProducto").tooltip();
            $("#txtProducto").focus();
            return false;
        }
        else if ($('#txtCantidad')[0].checkValidity() == false) {
            $("#txtCantidad").tooltip();
            $("#txtCantidad").focus();
            return false;
        }
        else if ($('#txtAncho')[0].checkValidity() == false) {
            $("#txtAncho").tooltip();
            $("#txtAncho").focus();
            return false;
        }
        else if ($('#txtAlto')[0].checkValidity() == false) {
            $("#txtAlto").tooltip();
            $("#txtAlto").focus();
            return false;
        }
        else if ($('#txtExtra')[0].checkValidity() == false) {
            $("#txtExtra").tooltip();
            $("#txtExtra").focus();
            return false;
        }

        var paraProd = { Idpro: $('#txtProducto').val(), IdColor: $('#txtProducto').data("idcolor"), Cant: $("#txtCantidad").val(), costo: $('#txtProducto').data("costo"), extra: $('#txtExtra').val(), Ancho: $("#txtAncho").val(), Alto: $("#txtAlto").val() };
        $.ajax({
            cache: false,
            url: "/Factura/AgregarProducto",
            type: "get",
            data: paraProd,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (typeof (data) == "string") {
                    $("#TextModal").html(data);
                    $("#HeaderModalInfo").html("Error");
                    $('#ModalError').modal("show");
                }
                else {
                        $("#ListaProductos tbody").empty();
                        for (var i = 0; i < data.length; i++) {
                            $('#ListaProductos tbody').append('<tr class="trTableFact warning">' +
                                                  '<td>' + data[i].Nombre + '</td>' +
                                                   '<td>' + data[i].CantMat + '</td>' +
                                                  '<td>' + "₡ " +data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                  '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                '</tr>');
                        }
                        CalcularTotal();
                        LimpiarCampos();
                        ocultarCamposVidrio();
                    }

                },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }

        })

    })

    $("#formFacturar").on("click", "#eliminarProducto", function () {
        var id = $(this).attr("data-id");
        $.ajax({
            cache: false,
            url: "/Factura/EliminarProducto",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#ListaProductos tbody").empty();
                 for (var i = 0; i < data.length; i++) {
                    $('#ListaProductos tbody').append('<tr class="trTableFact warning">' +
                                          '<td>' + data[i].Nombre + '</td>' +
                                           '<td>' + data[i].CantMat + '</td>' +
                                          '<td>' + "₡ " +data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
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
 //Imprimir 
    $(".pulse").click(function () {
    var id = $(this).attr("data-id");
    var WindowObject = window.open("/Factura/FacturaTicket?id=" + id + "", "PrintWindow",
"width=310,height=500,toolbars=no,scrollbars=yes,status=no,resizable=yes");
    WindowObject.focus();
   
});
})
//Click a Terminar Factura
$("#btnFacturar").on("click", function () {
    if ($('#txtClienteFinal')[0].checkValidity() == false) {
        $("#txtClienteFinal").tooltip();
        $("#txtClienteFinal").focus();
        return false;
    }
    $("#lblClienteFact").html($("#txtClienteFinal").val());
    if ($("#txtSaldo").html() != "") {
        $("#lblMontoTotal").html("₡ " + $("#txtSaldo").html().substr(1));
    }
    else {
        $("#lblMontoTotal").html("₡ " + $("#txtTotal").html().substr(1));
    }
    
    
        $("#ModalFacturar").modal("show");
})
//Click Pagar 
$("#btnPagar").on("click", function () {
    if ($("#txtMontoPagar").hasClass("alert-success")) {
        var patron = ",";
        var MontoP = 0;
        var MontoTotal = $("#txtTotal").html().substring(1);
        MontoTotal = parseFloat(MontoTotal.replace(patron, ''));

        var para = { IdCliente: $("#txtClienteFinal").data("cliente"), IdCotizacion: $("#txtCotizacion").val(), MontoPagar: $("#txtMontoPagar").val(), MontoTotal: MontoTotal }
        $.ajax({
            cache: false,
            url: "/Factura/ProcesarFactura",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Factura Registrada!") {
                    $("#ModalFacturar").modal("hide");
                    $("#TextModalinfo").html(data);
                    $("#HeaderModalInfo").html("Registrada");
                    $('#ModalMensaje').modal("show");
                } else {
                    $("#ModalFacturar").modal("hide");
                    $("#TextModal").html(data);
                    $("#HeaderModalInfo").html("Error");
                    $('#ModalError').modal("show");

                }
               
            }
        })
    }
    else {

        $("#txtMontoPagar").tooltip();
        $("#txtMontoPagar").focus();
    }
})
$('#ModalMensaje').on('hidden.bs.modal', function () {
    $("#ModalFacturar").removeData('bs.modal');
    PrintContent();
    WindowObject.focus();
    //window.location.href = "/Factura/Facturar";
});
$('#ModalFacturar').on('hidden.bs.modal', function () {
    $("#ModalFacturar").removeData('bs.modal');
    $("#ModalMensaje").removeData('bs.modal');
    $("#txtMontoPagar").removeClass("alert-success");
    $("#lblMontoCambio").html("₡ " + "0.00");
});
function CargarListaProductos() {
    $.ajax({
        cache: false,
        url: "/Factura/ConsultarListaProductos",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != "" && data != null) {
                if (typeof (data) == "string") {
                    $("#TextModal").html(data);
                    $("#HeaderModalInfo").html("Error");
                    $('#ModalError').modal("show");
                }
                else {
                    if (data[0].IdProducto == null) {
                        var Resultado = "<center>El o los materiales siguienetes no poseen el color selecionado:</center> <ul>";
                        for (var i = 0; i < data.length; i++) {
                            Resultado += "<li>" + data[i] + "</li>";
                        }
                        Resultado += "</ul>";
                        $("#TextModal").html(Resultado);
                        $('#ModalError').modal("show");
                    }
                    else {
                        $("#ListaProductos tbody").empty();
                 
                        for (var i = 0; i < data.length; i++) {
                            $('#ListaProductos tbody').append('<tr class="trTableFact warning">' +
                                                      '<td>' + data[i].Nombre + '</td>' +
                                                       '<td>' + data[i].CantMat + '</td>' +
                                                      '<td>'+"₡ "+ data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                      '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                    '</tr>');
                        }
                        CalcularTotal();
                    }

                }
            }
            else {
                $("#ListaProductos tbody").empty();
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }

    })
}
function VerificarMaterial(id) {
    if (id != "") {
        $.ajax({
            cache: false,
            url: "/Factura/VerificarMaterial",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#txtExtra").attr("placeholder", "% Extra");
                $("#txtExtra").removeAttr("required");

                if (data == "Vidrio") {
                    mostrarCamposVidrio();
                } else if (data == "Paleta") {
                    MostrarCamposPaleta();
                }
                else if (data == "Material") {
                    ocultarCamposVidrio();
                }
                else if (data == "Especial") {
                    CamposEspecial();
                }

            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        })
    }
}
function LimpiarListaProductos() {
    $("#ListaProductos tbody").empty();
    $("#txtTotal").html("₡ " + "0.00");
    $("#txtSaldo").html("₡ " + "0.00");
}
function CalcularTotal() {
    if ($("#txtCotizacion").val() != "") {
        $.ajax({
            cache: false,
            url: "/Factura/CalcularSaldo",
            type: "get",
            data: { id: $("#txtCotizacion").val() },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#Saldodiv").removeAttr("hidden");
                $("#txtSaldo").html("₡ " + round5(parseFloat(data)).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            }
        })
    } else {
        $("#txtSaldo").html("");
        $("#Saldodiv").attr("hidden", "hidden");
    }
        
    $.ajax({
        cache: false,
        url: "/Factura/CalcularTotal",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#txtTotal").html("₡ " + round5(parseFloat(data)).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        }
    })
}
function RefrescarLista() {
    $.ajax({
        cache: false,
        url: "/Factura/LimpiarListas",
        type: "GET",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    });

}
function AnularFactura(valor) {
    var params = { id: valor };
    $.ajax({
        cache: false,
        url: "/Factura/AnularFactura",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result == "ok") {
                parent.document.location = parent.document.location;
            }
            else {
                $("#TextModal").html(result);
                $('#ModalError').modal("show");
            }
        }
    });

}
function mostrarCamposVidrio() {
    $("#txtAncho").removeAttr("disabled");
    $("#txtAlto").removeAttr("disabled");
    $("#txtAncho").attr("required", "required");
    $("#txtAlto").attr("required", "required");
}
function ocultarCamposVidrio() {
    $("#txtAncho").attr("disabled", "disabled");
    $("#txtAlto").attr("disabled", "disabled");
    $("#txtAncho").removeAttr("required");
    $("#txtAlto").removeAttr("required");
}
function MostrarCamposPaleta() {
    $("#txtAncho").removeAttr("disabled");
    $("#txtAncho").attr("placeholder", "Largo");
    $("#txtAlto").attr("disabled", "disabled");
    $("#txtAncho").attr("required", "required");
    $("#txtAlto").removeAttr("required");
}
function CamposEspecial() {
    $("#txtAncho").attr("disabled", "disabled");
    $("#txtAlto").attr("disabled", "disabled");
    $("#txtAncho").removeAttr("required");
    $("#txtAlto").removeAttr("required");
    $("#txtExtra").attr("placeholder", "Precio ₡");
    $("#txtExtra").attr("required","required");
}
function LimpiarCampos(){
    $('#txtProducto').val("");
    $('#txtCantidad').val("");
    $('#txtAncho').val("");
    $('#txtAlto').val("");
    $('#txtExtra').val("");
    $("#txtExtra").attr("placeholder", "% Extra");
}
function LimpiarDatosRegistro() {
    $("#txtClienteFinal").val("");
    $("#txtClienteFinal").removeAttr("disabled", "disabled");
    $("#txtCotizacion").val("");
    $("#txtMontoPagar").val("");
}
var nav4 = window.Event ? true : false;
function acceptNum(evt) {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57, '.' = 46
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57) || key == 46);
}
function acceptonlyNum(evt) {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57, '.' = 46
    var key = nav4 ? evt.which : evt.keyCode;
    return (key <= 13 || (key >= 48 && key <= 57));
}
//calcular cambio
function CalcularCambio() {
    var MontoP = 0;
    var MontoTotal = $("#lblMontoTotal").html().substring(1);
    MontoTotal = MontoTotal.replace(/[,]/gi, '');
    MontoTotal = parseFloat(MontoTotal);

    MontoP = parseFloat($("#txtMontoPagar").val());
    var Cambio=0;
    if (MontoTotal <= MontoP) {
        Cambio = parseFloat(MontoP - MontoTotal).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
        $("#lblMontoCambio").html("₡ "+Cambio);
        $("#txtMontoPagar").attr("class", "form-control input-lg alert-success");
    } else {
        $("#txtMontoPagar").attr("class", "form-control input-lg alert-danger");
        $("#lblMontoCambio").html("₡ " + "0.00");
    }

}
//redondear a 5
function round5(x) {
    return Math.ceil(x / 5) * 5;
}
var WindowObject = new Object();
function PrintContent() {
    if ($("#txtCotizacion").val() != "") {
        WindowObject = window.open("/Factura/Ticket/" + $("#txtCotizacion").val() + "/" + $("#txtMontoPagar").val() + "", "PrintWindow",
       "width=300,height=500,toolbars=no,scrollbars=yes,status=no,resizable=yes");
        WindowObject.focus();
        LimpiarListaProductos();
        LimpiarDatosRegistro();

    }
    else {
        WindowObject = window.open("/Factura/Ticket/" + null + "/" + $("#txtMontoPagar").val() + "", "PrintWindow",
       "width=300,height=500,toolbars=no,scrollbars=yes,status=no,resizable=yes");
        WindowObject.focus();
        LimpiarListaProductos();
          LimpiarDatosRegistro();
    }
   
}
