$(document).ready(function () {
    $("#ImageFile").change(function () {
        var data = new FormData();
        $('#ImageDiv').html('<img src="/Content/Imagenes/loading.gif"/>');
        $("#ImageDiv").fadeIn(1000).html();
        var files = $("#ImageFile").get(0).files;
        if (files.length > 0) {
            data.append("HelpSectionImages", files[0]);
            var reader = new FileReader();
            
            reader.onload = function (e) {
                $('#MostrarImagen')
                    .attr('src', e.target.result)               
            };
            
        }
        $.ajax({
            url: "/Producto/CargarImagen",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                $("#ImageDiv").fadeOut();
                reader.readAsDataURL(files[0]);
                
            },
            error: function (er) {
                alert(er);
            }

        });
    });
    $("#ProductMaterial").on("change", "#SubCatSelect", function () {
        CargarMateriales($("#DropDownCategoria").val(), $("#SubCatSelect").val(),$("#ColorMatselect").val());

    })
    $("#ProductMaterial").on("change", "#ColorMatselect", function () {
        CargarMateriales($("#DropDownCategoria").val(), $("#SubCatSelect").val(),$("#ColorMatselect").val());

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

    $(window).unload(function () {
        RefrescarLista();
    });
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
            alert("Debe selecionar el material!!");

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
                        alert(data);
                        $("#MaterialSelect").val("");
                    }
                    else {
                        $("#ListaMateriales").empty();
                        var divisor = 4;
                        var string = "";
                        for (var i = 0; i < data.length; i++) {
                            if (i == 0 || divisor == i) {
                                string += '<tr>';

                            }
                            string += '<td><h5><span class="label label-default">' + data[i].NombreMaterial + '&nbsp&nbsp<input type="button" id="eliminarmat" data-id=' + data[i].IdMaterial + ' class="btn-danger btn-xs" value="X" /></span></h5></td>';
                            if (i == (divisor - 1)) {
                                string += '</tr>';
                                divisor += 4;
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
                    var divisor = 4;
                    var string = "";
                    for (var i = 0; i < data.length; i++) {
                        if (i == 0 || divisor == i) {
                            string += '<tr>';

                        }
                        string += '<td><h5><span class="label label-default">' + data[i].NombreMaterial + '&nbsp&nbsp<input type="button" id="eliminarmat" data-id=' + data[i].IdMaterial + ' class="btn-danger btn-xs" value="X" /></span></h5></td>';
                        if (i == (divisor - 1)) {
                            string += '</tr>';
                            divisor += 4;
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