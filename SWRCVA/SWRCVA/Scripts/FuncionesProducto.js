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
    $("#SubCatSelect").hide();
    $("#ProductMaterial").on("change", "#DropDownCategoria", function () {
        var id = $("#DropDownCategoria").val();
        
        $.ajax({
            cache:false,
            url:"/Producto/CargarSubcategoria",
            type:"get",
            data: { id: id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
             success: function (data) {
                if (typeof (data) == "string") {
                    alert(data);
                }
                var  items = "<option value=''>SubCategoria..</option>";
                for (var i = 0; i < data.length; i++) {
                   
                    items += "<option value='" + data[i].IdSubCatMat + "'>" + data[i].Nombre + "</option>";
                }
                $("#SubCatSelect").html(items);
                $("#SubCatSelect").show();
            },
            error: function (result) {
            alert('ERROR ' + result.status + ' ' + result.statusText);
        }
            
        });
    })
})

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