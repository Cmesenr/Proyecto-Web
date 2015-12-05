$(document).ready(function () {
    $(".pulse").click(function () {
        var id = $(this).attr("data-id");
        var monto=parseFloat($(this).attr("data-saldo"));
        $("#lblClienteNombre").html($(this).attr("data-nombre"));
        $("#lblSaldo").html("₡ " + monto.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
        $("#lblIdCotizacion").html(id);
        
        $("#ModalReciboDinero").modal("show");
    //    var id = $(this).attr("data-id");
    //    var WindowObject = window.open("/Cotizacion/Plaforma?id=" + id + "", "PrintWindow",
    //"width=310,height=500,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
    //    WindowObject.focus();

    });

    $("#btnGuardarModal").on("click", function () {
        if ($('#txtMontoCot')[0].checkValidity() == false) {
            $("#txtMontoCot").tooltip();
            $("#txtMontoCot").focus();
            return false;
        }

        var params = { Id: $("#lblIdCotizacion").html(), monto: $("#txtMontoCot").val() };
        $.ajax({
            cache: false,
            url: "/Cotizacion/Abonar",
            type: "GET",
            data: params,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                if (result == "ok") {
                    $("#TextModalinfo").html("Abono Registrado!");
                    $("#HeaderModalInfo").html("Correcto!");
                    $('#ModalMensaje').modal("show");
                    $("#txtMontoCot").val("");
                    $("#ModalReciboDinero").modal("hide");
                } else {
                    $("#TextModal").html(result);
                    $("#HeaderModalInfo").html("Error!");
                    $('#ModalMensaje').modal("show");
                }

            }
        });

    })
})
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