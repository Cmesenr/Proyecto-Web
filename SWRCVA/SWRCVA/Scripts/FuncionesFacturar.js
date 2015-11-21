$(document).ready(function () {
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
                                      '<td>' + data[i].Costo.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                      '<td> <button type="button" id="SeleccionarMaterial" class="btn btn-default btn-sm" data-costo="' + data[i].Costo + '"  data-myvalue="' + data[i].Id + '"><span class="glyphicon glyphicon-ok" /></button></td>' +
                                    '</tr>');

            }
            $("#TableMateriales").append('</tbody>');
            $('#TableMateriales').DataTable();
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
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json"
        }
    } );
   
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
    $("#txtProducto").on("click", function () {
        $("#ModalMateriales").modal("show");
    })
    //Selecionar El Material
    $("#headerPrincipal").on("click", "#SeleccionarMaterial", function (e) {
        $("#txtProducto").val($(this).data("myvalue"));
        $("#txtProducto").data("costo", $(this).data("costo"));
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

        var paraProd = { Idpro: $('#txtProducto').val(), Cant: $("#txtCantidad").val(), costo: $('#txtProducto').data("costo"), extra: $('#txtExtra').val() };
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
                        $("#ListaProductos").append('<tbody>');
                        for (var i = 0; i < data.length; i++) {
                            $('#ListaProductos').append('<tr class="trTableFact warning">' +
                                                  '<td>' + data[i].Nombre + '</td>' +
                                                   '<td>' + data[i].CantProducto + '</td>' +
                                                  '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                  '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                '</tr>');
                        }
                        $("#ListaProductos").append('</tbody>');
                        CalcularTotal();
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
                $("#ListaProductos").fadeIn(1000).html();
                $("#ListaProductos").append('<tbody>');


                for (var i = 0; i < data.length; i++) {
                    $('#ListaProductos').append('<tr class="trTableFact warning">' +
                                          '<td>' + data[i].Nombre + '</td>' +
                                           '<td>' + data[i].CantProducto + '</td>' +
                                          '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                          '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                        '</tr>');
                }
                $("#ListaProductos").append('</tbody>');
                CalcularTotal();
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        })

    })
})
function CargarListaProductos() {
    $('#ListaProductos tbody').html('<center><img src="/Content/Imagenes/loadinfo1.gif"/></center>');
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
                        $("#ListaProductos tbody").fadeIn(1000).html();
                        $("#ListaProductos tbody").empty();
                        $("#ListaProductos").append('<tbody>');

                  
                        for (var i = 0; i < data.length; i++) {
                                $('#ListaProductos').append('<tr class="trTableFact warning">' +
                                                      '<td>' + data[i].Nombre + '</td>' +
                                                       '<td>' + data[i].CantProducto + '</td>' +
                                                      '<td>' + data[i].Subtotal.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                                      '<td><input type="button" id="eliminarProducto" data-id=' + data[i].IdProducto + ' class="btn-danger btn-xs" value="X" /></td>' +
                                                    '</tr>');
                        }
                        $("#ListaProductos").append('</tbody>');
                        $('#ListaProductos').DataTable({
                            "paging": false,
                            "ordering": false,
                            "info": false,
                            "searching":false,
                            "scrollY": '200px',
                            "scrollX": false,
                            "scrollCollapse": false
                        });
                        CalcularTotal();
                    }

                }
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }

    })
}
function CalcularTotal() {
    $.ajax({
        cache: false,
        url: "/Factura/CalcularTotal",
        type: "get",
        data: {},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#txtTotal").html("₡ " + data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
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