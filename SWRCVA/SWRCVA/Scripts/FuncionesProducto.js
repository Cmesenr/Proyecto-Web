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
            $('#ColorMatselect').show();
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

        $.ajax({
            cache: false,
            url: "/Producto/CargarColores",
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
                $("#ColorMatselect").html(items);

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
})

function CargarMateriales(IdCat, IdSubcat, IdColor)
{
    if ($("#DropDownCategoria").val() != "" && $("#SubCatSelect").val() != "" && $("#ColorMatselect").val() != "") {
        var params = { IdCat: IdCat, IdSubcat: IdSubcat, IdColor: IdColor };
 
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