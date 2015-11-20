﻿$(document).ready(function () {
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
    //consultar imagen producto
    $('#DropDownProductos').on("change", function () {
        ConsultarImagen($('#DropDownProductos').val());
    })
    //Redirecionar a index cuando se guarda o procesa
    $('#ModalMensaje').on('hidden.bs.modal', function () {
        $("#ModalMensaje").removeData('bs.modal');
        window.location.href = "/Cotizacion/index";
    });
    //cargar productos 
    $('#DropDownTipoProductos').on("change", function () {
        var id = $('#DropDownTipoProductos').val();
        if (id != "") {
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
        }
    })
    //Verificar Atributos
    $("#DropDownProductos").on("change", function () {
        var id = $(this).val();
        if (id != "") {
            $.ajax({
                cache: false,
                url: "/Cotizacion/VerificarAtributos",
                type: "get",
                data: { id: id },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data == "CV") {
                        $("#txtCelocia").attr("required", "required");
                        $("#txtCelocia").removeAttr("disabled");
                        $('input[name=Vidrio]').removeAttr("disabled");
                    } else if (data == "C") {
                        consultarVidrio(null, "Paleta");
                        $('input[name=Vidrio]').attr("disabled", "disabled");
                        $("#txtCelocia").attr("disabled", "disabled");
                    }
                    else if (data == "PB") {
                        consultarVidrio(null, "Lamina");
                        $("#txtCelocia").attr("required", "required");
                        $('input[name=Vidrio]').attr("disabled", "disabled");
                        $("#txtCelocia").removeAttr("disabled");
                        $("#txtCelocia").attr("placeholder", "Laminas");
                        $("#DropDownPaletas").val("");
                        $('#DropDownPaletas').attr("disabled", "disabled");
                        $("#DropDownPaletas").removeAttr("required");
                    }
                    else {
                        $("#txtCelocia").attr("disabled", "disabled");
                        $("#txtCelocia").removeAttr("required");
                        $("#txtCelocia").val("");
                        $("#txtCelocia").attr("placeholder", "Celocia")
                        $('input[name=Vidrio]').removeAttr("disabled");
                        $("#DropDownPaletas").val("");
                        $('#DropDownPaletas').attr("disabled", "disabled");
                        $("#DropDownPaletas").removeAttr("required");
                    }

                },
                error: function (result) {
                    alert('ERROR ' + result.status + ' ' + result.statusText);
                }
            })
        }
    })
    //Cargar Combo Vidrios
    $('input[name=Vidrio]').on("change", function () {
        var id = $(this).val();
        consultarVidrio(id, "Categoria");
    })
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
        var para = { Nombre: $('#txtNombreCliente').val(), Telefono: $('#txtTelefonoCliente').val(), Correo: $('#txtCorreoCliente').val(), Direccion: $('#txtDireccionCliente').val()};
        $.ajax({
            cache: false,
            url: "/Cotizacion/GuardarCliente",
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
    //habilitar combo paleta
    $('#txtCelocia').on('change', function () {
        if ($("#DropDownTipoProductos").val() == 0)
        { 
        if ($('#txtCelocia').val() != "") {
            $('#DropDownPaletas').removeAttr("disabled");
            $('#DropDownPaletas').attr("required","required");
        }
        }

    });
    //Levantar modal de colores paleta 
    $('#DropDownPaletas').on('change', function () {
          if ($('#txtCelocia') != null) {
            $('#ModalColores').modal({ backdrop: 'static', keyboard: false })
            $("#ModalColores").modal("show");
        }        
    });
 
    //Validar Color paleta
    $("#btnGuardarColorPaleta").on("click", function () {
        if ($('#CPaleta')[0].checkValidity() == false) {
            $("#CPaleta").tooltip();
            $("#CPaleta").focus();
            return false;
        } 
        $("#ModalColores").modal("hide");
    })
    //Procesar Cotizacion
    $("#btnProcesar").on("click", function () {
        if ($('#txtClienteFinal')[0].checkValidity() == false) {
            $("#txtClienteFinal").tooltip();
            $("#txtClienteFinal").focus();
            return false;
        }
        var para = { IdCliente: $('#txtClienteFinal').data("cliente"), Comentario: $('#txtAreaComentario').val() };
        $.ajax({
            cache: false,
            url: "/Cotizacion/ProcesarCotizacion",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Cotizacion Procesada!") {
                    LimpiarCamposHeader();
                }
                $("#TextModalinfo").html(data);
                $("#HeaderModalInfo").html("Procesado");
                $('#ModalMensaje').modal("show");
            }
        })

    })
    //Guardar Cotizacion
    $("#btnGuardar").on("click", function () {
        if ($('#txtClienteFinal')[0].checkValidity() == false) {
            $("#txtClienteFinal").tooltip();
            $("#txtClienteFinal").focus();
            return false;
        }
        var para = { IdCliente: $('#txtClienteFinal').data("cliente"), Comentario: $('#txtAreaComentario').val() };
        $.ajax({
            cache: false,
            url: "/Cotizacion/GuardarCotizacion",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Cotizacion Guardada!") {
                    LimpiarCamposHeader();
                }
                $("#TextModalinfo").html(data);
                $("#HeaderModalInfo").html("Guardado");
                $('#ModalMensaje').modal("show");
             
            }
        })

    })
    //Procesar cotizacion despues de editar
    $("#btnProcesarEdit").on("click", function () {
        $('#txtClienteFinal').removeAttr("disabled");
        if ($('#txtClienteFinal')[0].checkValidity() == false) {
            $("#txtClienteFinal").tooltip();
            $("#txtClienteFinal").focus();
            return false;
        }
        var para = { Id:$("#IdCotizacion").val(), IdCliente: $('#txtClienteFinal').data("cliente"), Comentario: $('#txtAreaComentario').val() };
        $.ajax({
            cache: false,
            url: "/Cotizacion/ProcesarCotizacionEdit",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Cotizacion Procesada!") {
                    LimpiarCamposHeader();
                }
                $("#TextModalinfo").html(data);
                $("#HeaderModalInfo").html("Procesado");
                $('#ModalMensaje').modal("show");
            }
        })

    })
    //Guardar Cotizacion
    $("#btnGuardarEdit").on("click", function () {
        if ($('#txtClienteFinal')[0].checkValidity() == false) {
            $("#txtClienteFinal").tooltip();
            $("#txtClienteFinal").focus();
            return false;
        }
        var para = { Id: $("#IdCotizacion").val(), IdCliente: $('#txtClienteFinal').data("cliente"), Comentario: $('#txtAreaComentario').val() };
        $.ajax({
            cache: false,
            url: "/Cotizacion/GuardarCotizacionEdit",
            type: "get",
            data: para,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Cotizacion Guardada!") {
                    LimpiarCamposHeader();
                }
                $("#TextModalinfo").html(data);
                $("#HeaderModalInfo").html("Guardado");
                $('#ModalMensaje').modal("show");
             

            }
        })

    })
    //Agregar Producto 
    $("#formCotizar").on("click", "#btnAgregar", function () {
        $('#ListaProductos').html('<center><img src="/Content/Imagenes/loadinfo1.gif"/></center>');
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
        else if ($('#txtCelocia')[0].checkValidity() == false) {
            $("#txtCelocia").tooltip();
            $("#txtCelocia").focus();
            return false;
        } else if ($('#DropDownPaletas')[0].checkValidity() == false) {
            $("#DropDownPaletas").tooltip();
            $("#DropDownPaletas").focus();
            return false;
        }
        else if ($('#txtAncho')[0].checkValidity() == false) {           
            $("#txtAncho").tooltip();
            $("#txtAncho").focus();
            return false;
        } else if ($('#txtAlto')[0].checkValidity() == false) {           
            $("#txtAlto").tooltip();
            $("#txtAlto").focus();
            return false;
        } 
        else if ($('#txtCantidad')[0].checkValidity() == false) {
            $("#txtCantidad").tooltip();
            $("#txtCantidad").focus();
            return false;
        }

        var paraProd = { Idpro: $('#DropDownProductos').val(), Cvidrio: $("#DropDownCVidrio").val(), anchoCelocia: $('#txtCelocia').val(), CAluminio: $('#DropDownCAluminio').val(), Insta: $('#DropDownInstalacion').val(), Cant: $('#txtCantidad').val(), Ancho: $('#txtAncho').val(), Alto: $('#txtAlto').val(), vidrio: $('#DropDownVidrio').val(), ColorPaleta: $('input[name=ColoresPaleta]:checked').val(), IdPaleta: $("#DropDownPaletas").val() };
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
                        $("#HeaderModalInfo").html("Error");
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
                            $("#ListaProductos").fadeIn(1000).html();
                            $("#ListaProductos").append('<tr class="active"><th>Producto</th><th>Cantidad</th><th>Subtotal</th><th></th></tr>');

                        for (var i = 0; i < data.length; i++) {
                            $('#ListaProductos').append('<tr class="warning">' +
                                                  '<td>' + data[i].Nombre + '</td>' +
                                                   '<td>' + data[i].CantProducto + '</td>' +
                                                  '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                  '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                '</tr>');
                        }
                        CalcularTotal();
                        LimpiarCamposBoddy();
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
                $("#ListaProductos").append('<tr class="active"><th>Producto</th><th>Cantidad</th><th>Subtotal</th><th></th></tr>');

                for (var i = 0; i < data.length; i++) {
                    $('#ListaProductos').append('<tr class="warning">' +
                                          '<td>' + data[i].Nombre + '</td>' +
                                           '<td>' + data[i].CantProducto + '</td>' +
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
    var para = { id: id };
    $('#MostrarImagen').fadeOut().attr('src', "");
    $.ajax({
        cache: false,
        url: "/Cotizacion/ConsultarImagen",
        type: "get",
        data: para,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#MostrarImagen').fadeIn(1000).attr('src', "data:image/png;base64," + data);
            
        }
    })
}
function LimpiarCamposHeader() {
    $('#txtClienteFinal').val("");
    $('#txtAreaComentario').val("");
    $('#ListaProductos').html("");
    $("#txtTotal").html("0.00");
}
function LimpiarCamposBoddy() {
    $('#DropDownTipoProductos').val("");
    $('#DropDownProductos').val("");
    $('#DropDownCVidrio').val("");
    $('#DropDownCAluminio').val("");
    $('#DropDownInstalacion').val("");
    $('#DropDownVidrio').val("");
    $('#txtCantidad').val("");
    $('#txtAncho').val("");
    $('#txtAlto').val("");
    $('#txtCelocia').val("");
    $('input[name=ColoresPaleta]').attr('checked', false);
    $("#DropDownPaletas").val("");
}
function CargarListaProductos() {
    $.ajax({
        cache: false,
        url: "/Cotizacion/ConsultarListaProductos",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != "" && data !=null) {
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
                    $("#ListaProductos").empty();
                    $("#ListaProductos").append('<tr class="active"><th>Producto</th><th>Cantidad</th><th>Subtotal</th><th></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#ListaProductos').append('<tr class="warning">' +
                                              '<td>' + data[i].Nombre + '</td>' +
                                               '<td>' + data[i].CantProducto + '</td>' +
                                              '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                              '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                            '</tr>');
                    }
                    CalcularTotal();
                    LimpiarCamposBoddy();
                }

            }
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }

    })
}
function RefrescarLista() {
    $.ajax({
        cache: false,
        url: "/Cotizacion/LimpiarListas",
        type: "GET",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    });

}
function consultarVidrio(id, bool) {
    var para = { id: id, tipo: bool };
    $.ajax({
        cache: false,
        url: "/Cotizacion/ColsultarVidrio",
        type: "get",
        data: para,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var items = "<option value=''>Vidrios..</option>";
            for (var i = 0; i < data.length; i++) {

                items += "<option value='" + data[i].IdMaterial + "'>" + data[i].Nombre + "</option>";
            }

            $("#DropDownVidrio").html(items);

        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
    })
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