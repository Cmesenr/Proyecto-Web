
$(document).ready(function () {

});
function popup() {
    $("#modal").load("Material/Registrar", function () {
        $("#modal").modal();
    })
};
function CambiarCat() {
    if ($('#DropDownCat').val() == "1") {
        $('#ColorMaterial').slideUp();
    }
    else {
        $('#ColorMaterial').slideDown();
    }
};

