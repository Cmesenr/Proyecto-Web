

$(function () {
    $("#EditarMaterialform").on('submit', function (e) {
        $("#DropDownCat").removeAttr("disabled");
    });
});
$(document).ready(function (e) {
    CambiarCat();
    $('#modal').on('hidden.bs.modal', function (e) {
        $("#modal").removeData('bs.modal');     
        parent.document.location = parent.document.location;
       RefrescarLista();
    });

    $(window).unload(function () {
        RefrescarLista();
    });

});
$("#FormRegistrarMat").on("change", "#DropDownCat", function () {
    var id = $("#DropDownCat").val();
    if (id != 1&& id!="") {
        $.ajax({
            cache: false,
            url: "/Material/CargarSubcategoria",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var items = "<option value=''>SubCategoria...</option>";
                for (var i = 0; i < data.length; i++) {

                    items += "<option value='" + data[i].IdSubCatMat + "'>" + data[i].Nombre + "</option>";
                }
                $('#dropSubCat').removeAttr("disabled");
                $("#dropSubCat").html(items);

            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }

        });

        $.ajax({
            cache: false,
            url: "/Material/CargarColores",
            type: "get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var items = "<option value=''>Colores...</option>";
                for (var i = 0; i < data.length; i++) {

                    items += "<option value='" + data[i].IdColor + "'>" + data[i].Nombre + "</option>";
                }
                $('#ColorMatselect').removeAttr("disabled");
                $('#txtCostoMat').removeAttr("disabled");
                $('#btnAgregarColor').removeAttr("disabled");
                $("#ColorMatselect").html(items);

            },
            error: function (result) {
                alert('ERROR ' + result.status + ' ' + result.statusText);
            }

        });
    }
    else {
        $('#ColorMatselect').attr("disabled","disabled");
        $('#txtCostoMat').attr("disabled", "disabled");
        $('#btnAgregarColor').attr("disabled", "disabled");
        $('#dropSubCat').attr("disabled", "disabled");
    }
})
$("#ColorMaterial").on("click", "#btnAgregarColor", function () {
    if ($("#ColorMatselect").val() != "" && $("#txtCostoMat").val() != "") {
        var params = { IdColor: $("#ColorMatselect").val(), Costo: $("#txtCostoMat").val() };
        var respuesta = true;
    }
    else {
        alert("Debe ingresar el Color y el Costo!");
    }

    if (respuesta) {
        $.ajax({
            cache: false,
            url: "/Material/AgregarColor",
            type: "get",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
               
                if (typeof (data) == "string") {
                    alert(data);
                    $("#ColorMatselect").val("");
                    $("#txtCostoMat").val("");
                }
                else {
                    $("#ListaColor").empty();
                    $("#ListaColor").append('<tr><th>Color</th><th>Costo</th><th></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#ListaColor').append('<tr>' +
                                              '<td>' + data[i].NombreMaterial + '</td>' +
                                              '<td class="Listacol2">' + data[i].Costo.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                              '<td><input type="button" id="eliminarcolormat" data-id=' + data[i].IdColorMat + ' class="btn-danger btn-xs" value="X" /></td>' +
                                            '</tr>');
                    }
                    
                    $("#ColorMatselect").val("");
                    $("#txtCostoMat").val("");
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

$("#ColorMaterial").on("click", "#eliminarcolormat", function () {
    var id = $(this).attr("data-id");
    $.ajax({
        cache: false,
        url: "/Material/EliminarColor",
        type: "get",
        data: { id: id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#ListaColor").empty();
            $("#ListaColor").append('<tr><th>Color</th><th>Costo</th><th></th></tr>');

            for (var i = 0; i < data.length; i++) {
                $('#ListaColor').append('<tr>' +
                                      '<td>' + data[i].NombreMaterial + '</td>' +
                                      '<td>' + data[i].Costo.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + '</td>' +
                                      '<td><input type="button" id="eliminarcolormat" data-id=' + data[i].IdColorMat + ' class="btn-danger btn-xs" value="X" /></td>' +
                                    '</tr>');
            }
        },
        error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
    })

})
function CambiarCat() {
    if ($('#DropDownCat').val() == "1") {
        $('#ColorMaterial').slideUp();
        $("#subCat").slideUp();
        $("#CostoMatterial").slideDown();
        $("#txtCosto").attr("required", "required"); 
        $("#dropSubCat").removeAttr("required");
    }
    else {
        $("#CostoMatterial").slideUp();
        $('#ColorMaterial').slideDown();
        $("#subCat").slideDown();
        $("#txtCosto").removeAttr("required")
        $("#dropSubCat").attr("required", "required");

    }
};
function RefrescarLista() {
    $.ajax({
        cache: false,
        url: "/Material/RefrescarLista",
        type: "GET",
        data: {},
        contentType: "application/json; charset=utf-8",
        success: function (result) {

        }
    });

}



    


