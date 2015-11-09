$(document).ready(function () {
    
    $("#formCotizar").on("click", "#btnCotizar", function () {

         if ($('#DropDownProductos')[0].checkValidity() == false) {
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
        } else if ($('#txtCantidad')[0].checkValidity() == false) {
            $("#txtCantidad").tooltip();
            $("#txtCantidad").focus();           
            return false;
        } else if ($('#txtAncho')[0].checkValidity() == false) {           
            $("#txtAncho").tooltip();
            $("#txtAncho").focus();
            return false;
        } else if ($('#txtAlto')[0].checkValidity() == false) {           
            $("#txtAlto").tooltip();
            $("#txtAlto").focus();
            return false;
        }
         var paraProd = { Idpro: $('#DropDownProductos').val(), Cvidrio: $("#DropDownCVidrio").val(), CAluminio: $('#DropDownCAluminio').val(), Insta: $('#DropDownInstalacion').val(), Cant: $('#txtCantidad').val(), Ancho: $('#txtAncho').val(), Alto: $('#txtAlto').val() };
               $.ajax({
                cache: false,
                url: "/Cotizacion/AgregarProducto",
                type: "get",
                data: paraProd,
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
      
    })

})