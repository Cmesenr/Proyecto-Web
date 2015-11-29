$(function () {
    var tipo = "Respaldo";

    $('input[name=Respaldo]').on("click", function () {
        tipo = $(this).attr("Value");
    })

    $('#Procesar').on("click", function () {
        if (tipo != "") {
            var param = {tipoOperacion : tipo}
            $.ajax({
                cache: false,
                url: "/Respaldo/RepaldarBD",
                type: "GET",
                data: param,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#ModalMensaje').modal("show");
                },
                error: function (result) {
                    alert('ERROR ' + result.status + ' ' + result.statusText);
                }
            })
        }
    })
});
