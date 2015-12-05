$(document).ready(function () {
    $("#formRecibo").on("click", ".pulse",function () {
        var id = $(this).attr("data-id");
        var monto = parseFloat($(this).attr("data-saldo"));
        $("#txtSaldo").val(monto);
        $("#lblClienteNombre").html($(this).attr("data-nombre"));
        $("#lblSaldo").html("₡ " + monto.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));     
        $("#lblIdCotizacion").html(id);
        
        $("#ModalReciboDinero").modal("show");

    });

    $("#formRecibo").on("click", "#btnGuardarModal", function () {
        var monto = $("#txtSaldo").val();
        if ($('#txtMontoCot')[0].checkValidity() == false) {
            $("#txtMontoCot").tooltip();
            $("#txtMontoCot").focus();
            return false;
       } //else if($("#txtMontoCot").val()>monto){
        //    $("#contenError").html('<div id="AlertError" class="alert alert-danger alert-dismissible" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button><strong>Error!</strong> El monto no puede ser mayor al saldo.</div>');
        //    return false;
        //}

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
                    $("#ModalReciboDinero").modal("hide");
                    PrintContent($("#lblIdCotizacion").html(), $("#txtMontoCot").val());
                    WindowObject.focus();
                } else {
                    $("#TextModal").html(result);
                    $('#ModalError').modal("show");
                    $("#txtMontoCot").val("");
                }

            }
        });

    })
    $('#ModalError').on('hidden.bs.modal', function () {
        $("#ModalError").removeData('bs.modal');
        $("#txtMontoCot").focus();
    });
    $('#ModalMensaje').on('hidden.bs.modal', function () {
        $("#ModalMensaje").removeData('bs.modal');
        window.location.href = "/Cotizacion/Recibo";
        $("#txtMontoCot").val("");
    });
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
var WindowObject = new Object();
function PrintContent(id, monto) {
    WindowObject = window.open("/Cotizacion/ReciboDinero/" + id + "/" + monto + "", "PrintWindow",
    "width=310,height=500,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes");
        WindowObject.focus();
}