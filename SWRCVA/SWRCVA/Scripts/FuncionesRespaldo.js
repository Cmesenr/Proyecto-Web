$(function () {
    var tipo = "Respaldo";

    $('input[name=Respaldo]').on("click", function () {
        tipo = $(this).attr("Value");
    })

    $('#Procesar').on("click", function () {
        if (tipo != "") {
            var param = { tipoOperacion: tipo }
            $('#Cargando').html('<img src="/Content/Imagenes/loading3.gif"/><p>Espere un momento...</p>');
            $.ajax({
                cache: false,
                url: "/Respaldo/RepaldarBD",
                type: "GET",
                data: param,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#ModalMensaje').modal("show");
                    $('#Cargando').html('');
                },
                error: function (result) {
                    alert('ERROR ' + result.status + ' ' + result.statusText);
                }
            })
        }
    })
});
