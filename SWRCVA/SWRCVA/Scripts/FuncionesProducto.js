$(document).ready(function () {
    switch ($("#DropDownTipoProducto").val()) {
        case '0': {
            $("#Group_Ventana").show();
            $("#lblAtributos").show();
            break;
        }
        case '1': {
            $("#Group_Ventana5020").show();
            $("#lblAtributos").show();
            break;
        }
        case '6': {
            $("#Group_Portones").show();
            $("#lblAtributos").show();
            break;
        }
        case '5': {
            $("#Group_PuertaBano").show();
            $("#lblAtributos").show();
            break;
        }
        case '3': {
            $("#Group_Puertalujo").show();
            $("#lblAtributos").show();
            break;
        }
            

    }
})

$(document).ready(function () {
    $("#ImageFile").change(function () {
        var data = new FormData();
        $('#ImageDiv').html('<img src="/Content/Imagenes/loadinfo2.gif"/>');
        $("#ImageDiv").fadeIn(1000).html();
        var files = $("#ImageFile").get(0).files;
        if (files.length > 0) {
            data.append("HelpSectionImages", files[0]);
            var reader = new FileReader();
            
            reader.onload = function (e) {
                $("#ImageDiv").hide();
                $('#MostrarImagen')
                    .attr('src', e.target.result)               
            };
           
            reader.readAsDataURL(files[0]);
            
      }
        });
    $("#ProductMaterial").on("change", "#SubCatSelect", function () {
        CargarMateriales($("#DropDownCategoria").val(), $("#SubCatSelect").val(),$("#ColorMatselect").val());

    })
    $("#ProductMaterial").on("change", "#ColorMatselect", function () {
        CargarMateriales($("#DropDownCategoria").val(), $("#SubCatSelect").val(),$("#ColorMatselect").val());

    })
    $("#ProductMaterial").on("change", "#MaterialSelect", function () {
        $("#btnAgregarMaterial").removeAttr("disabled");
    })
    $("#ProductMaterial").on("change", "#DropDownCategoria", function () {
        var id = $("#DropDownCategoria").val();
                    
        $('#MaterialSelect').attr("disabled","disabled");
        if (id != 1&& id!=0) {
            $('#SubCatSelect').show();
        $.ajax({
            cache:false,
            url:"/Producto/CargarSubcategoria",
            type:"get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
             success: function (data) {
                var  items = "<option value=''>SubCategoria...</option>";
                for (var i = 0; i < data.length; i++) {
                   
                    items += "<option value='" + data[i].IdSubCatMat + "'>" + data[i].Nombre + "</option>";
                }
                $('#SubCatSelect').removeAttr("disabled");
                $("#SubCatSelect").html(items);
               
            },
            error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
            
        });
        }
        else {
            if (id == 1) {
                $('#MaterialSelect').removeAttr("disabled");
            }
            $('#SubCatSelect').hide();
            $('#ColorMatselect').hide();
            CargarMateriales($("#DropDownCategoria").val());
        }
    })

    $("#formProductotipo").on("change", "#DropDownTipoProducto", function () {
        var id = $("#DropDownTipoProducto").val();
        $("#Group_Ventana").hide();
        $("#Group_Ventana5020").hide();
        $("#Group_Ventana").hide();
        $("#lblAtributos").hide();
        $("#Group_PuertaBano").hide();
        $("#Group_Puertalujo").hide();
        
        switch (id) {
            case '0' : {           
                $("#Group_Ventana").show();
                $("#lblAtributos").show();
                break;
            }
            case '1': {
                $("#Group_Ventana5020").show();
                $("#lblAtributos").show();
                break;
            }
            case '6': {
                $("#Group_Ventana5020").show();
                $("#lblAtributos").show();
                break;
            }
            case '5': {
                $("#Group_PuertaBano").show();
                $("#lblAtributos").show();
                break;
            }
            case '3': {
                $("#Group_Puertalujo").show();
                $("#lblAtributos").show();
                break;
            }
                
     
        }

    })

})
$(document).ready(function (e) {
    $('#ModalConfirm').on('show.bs.modal', function (e) {
        var id = $(e.relatedTarget).data().id;
        var data = $(e.relatedTarget).data().info;
        $(e.currentTarget).find('#btnModalborrar').val(id);
        $(e.currentTarget).find('#TextModal').html("Esta seguro que desea borrar el Producto " + data + " ?");
    });
});
$(function()
{
    $("#FormProductos").on("click", "#btnAgregarMaterial", function () {

        if ($("#MaterialSelect").val() != "") {
            var params = { IdMat: $("#MaterialSelect").val() };
            var respuesta = true;
        }
        else {
            $("#TextModal").html("Debe selecionar el material!!");
            $('#ModalError').modal("show");
        }

        if (respuesta) {
            $.ajax({
                cache: false,
                url: "/Producto/AgregarMaterial",
                type: "get",
                data: params,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    if (typeof (data) == "string") {
                        $("#TextModal").html(data);
                        $('#ModalError').modal("show");
                        $("#MaterialSelect").val("");
                    }
                    else {
                        $("#ListaMateriales").empty();
                        var divisor = 2;
                        var string = "";
                        for (var i = 0; i < data.length; i++) {
                            if (i == 0 || divisor == i) {
                                string += '<tr>';

                            }
                            string += '<td><span class="label label-default">' + data[i].Nombre + '&nbsp&nbsp<input type="button" id="eliminarmat" data-id=' + data[i].IdMaterial + ' class="btn-danger btn-xs" value="X" /></span></td>';
                            if (i == (divisor - 1)) {
                                string += '</tr>';
                                divisor += 2;
                            }

                        }

                        $("#ListaMateriales").html(string);
                        $("#MaterialSelect").val("");

                    }
                },
                error: function (result) {
                    alert('ERROR ' + result.status + ' ' + result.statusText);
                }

            })
        } else {
            return false;
        }

    })

    $("#FormProductos").on("click", "#eliminarmat", function () {
        var id = $(this).attr("data-id");
        $.ajax({
            cache: false,
            url: "/Producto/EliminarMaterial",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (typeof (data) == "string") {
                    alert(data);
                    $("#MaterialSelect").val("");
                }
                else {
                    $("#ListaMateriales").empty();
                    var divisor = 2;
                    var string = "";
                    for (var i = 0; i < data.length; i++) {
                        if (i == 0 || divisor == i) {
                            string += '<tr>';

                        }
                        string += '<td><span class="label label-default">' + data[i].Nombre + '&nbsp&nbsp<input type="button" id="eliminarmat" data-id=' + data[i].IdMaterial + ' class="btn-danger btn-xs" value="X" /></span></td>';
                        if (i == (divisor - 1)) {
                            string += '</tr>';
                            divisor += 2;
                        }

                    }

                    $("#ListaMateriales").html(string);
                    $("#MaterialSelect").val("");

                }
            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }
        })

    })
})
function CargarMateriales(IdCat, IdSubcat, IdColor)
{
    if ($("#DropDownCategoria").val() != "" && $("#SubCatSelect").val() != "") {
        var params = { IdCat: IdCat, IdSubcat: IdSubcat };
 
    $.ajax({
        cache: false,
        url: "/Producto/CargarMateriales",
        type: "get",
        data: params,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var items = "<option value=''>Materiales...</option>";
            for (var i = 0; i < data.length; i++) {

                items += "<option value='" + data[i].IdMaterial + "'>" + data[i].Nombre + "</option>";
            }
            $('#MaterialSelect').show();
            $('#MaterialSelect').removeAttr("disabled");

            $("#MaterialSelect").html(items);

        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }

    });
    }
}

function RefrescarLista() {
    $.ajax({
        cache: false,
        url: "/Producto/RefrescarLista",
        type: "GET",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    });

}
function EliminarProducto(valor) {

    var params = { id: valor };
    $.ajax({
        cache: false,
        url: "/Producto/Borrar",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            parent.document.location = parent.document.location;
        }
    });

}