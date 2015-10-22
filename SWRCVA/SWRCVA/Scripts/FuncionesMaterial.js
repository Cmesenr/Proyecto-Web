
$(document).ready(function () {
    CambiarCat();
    $('#modal').on('hidden.bs.modal', function (e) {
        $("#modal").removeData('bs.modal');
        parent.document.location = parent.document.location;
    });
    $("#btnAgregarColor").on("click", function () {
        if ($("#DropDownColor").val() != "" && $("#txtCostoMat").val()!=""){
            var params = { IdCat: $("#DropDownColor").val(), Costo: $("#txtCostoMat").val() };
            var respuesta = true;
        }
        else {
            alert("Debe ingresar el Color y el Costo!");
        }
        
        if (respuesta) {
            $.ajax({
                cache: false,
                url: "/Material/AgregarCosto",
                type: "get",
                data: params,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $("#ListaColor").empty();
                    $("#ListaColor").append('<tr><th>Color</th><th>Costo</th><th></th></tr>');

                    for (var i = 0; i < data.length; i++) {
                        $('#ListaColor').append('<tr>' +
                                              '<td>' + data[i].NombreMaterial + '</td>' +
                                              '<td>' + data[i].Costo + '</td>' +
                                              '<td><input type="button" id="eliminarcolormat" class="btn-danger btn-sm" value="X" /></td>' +
                                            '</tr>'); 
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
});
function CambiarCat() {
    if ($('#DropDownCat').val() == "1") {
        $('#ColorMaterial').slideUp();
        $("#subCat").slideUp();
        $("#CostoMatterial").slideDown();
    }
    else {
        $("#CostoMatterial").slideUp();
        $('#ColorMaterial').slideDown();
        $("#subCat").slideDown();

    }
};

    


