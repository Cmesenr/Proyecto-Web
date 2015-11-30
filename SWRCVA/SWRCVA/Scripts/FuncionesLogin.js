function Login(valor) {
    var params = { id: valor };
        $.ajax({
            cache: false,
            url: "/Login/Login",
            type: "GET",
            data: params,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                parent.document.location = parent.document.location;
            }
        });
}
function CerrarSession() {
    $.ajax({
        cache: false,
        url: "/Login/CerrarSession",
        type: "GET",
        data: "",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            parent.document.location = parent.document.location;
        }
    });
}

function CambiarContraseña(valor) {
    var params = { id: valor };
    $.ajax({
        cache: false,
        url: "/Login/CambiarContraseña",
        type: "GET",
        data: params,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            parent.document.location = parent.document.location;
        }
    });
}
