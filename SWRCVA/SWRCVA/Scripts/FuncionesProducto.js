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
            url:"Producto/CargarSubcategoria",
            type:"get",
            contentType: "application/json; charset=utf-8",
            data:{id: id},
            success: function(resultado){
                var  items = "<option value=''>Seleccione SubCategoria</option>";

                for (var i = 0; i < resultado.length; i++) {
                    items += "<option value='" + resultado.IdSubCatMat + "'>" + resultado.Nombre + "</option>";
                }
                $("#SubCatSelect").html(items);
                $("#SubCatSelect").show();
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