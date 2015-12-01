$(function () {
    $('#btnIngresar').on('click', function () {
        if ($('#Contraseña')[0].checkValidity() == false) {
            $("#Contraseña").tooltip();
            $("#Contraseña").focus();
            return false;
        }
        else if ($('#newContraseña')[0].checkValidity() == false) {
            $("#newContraseña").tooltip();
            $("#newContraseña").focus();
            return false;
        }
        if ($("#Contraseña").val() != $("#newContraseña").val()) {
            $("#Contraseña").attr("title", "Contraseñas deben Coincidir");
            $("#Contraseña").tooltip();
            $("#Contraseña").focus();
            return false;
        }
        var contraseña = $("#Contraseña").val();
        var params = { contraseña: contraseña };
        $.ajax({
            cache: false,
            url: "/Login/JCambiarContraseña",
            type: "GET",
            data: params,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "ok") {
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

