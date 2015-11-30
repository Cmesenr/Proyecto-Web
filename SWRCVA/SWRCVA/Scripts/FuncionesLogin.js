$(function () {
    $('#btnIngresar').on('click', function () {
        var contraseña = $("#Contraseña").val();
        var params = { contraseña: contraseña };
        $.ajax({
            cache: false,
            url: "/Login/CambiarContraseña",
            type: "GET",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "Exitoso!") {
                    $('#ModalMensaje').modal("show");
                }
            }
        });
    });
    $('#ModalMensaje').on('hidden.bs.modal', function () {
        $("#ModalMensaje").removeData('bs.modal');
        window.location.href = "/Home/index";
    });
})

