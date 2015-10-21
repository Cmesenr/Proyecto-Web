
$(document).ready(function () {

});
$(".edit").click(function () {
    var id = $(this).attr("data-id");
    $("#modal").load("/Material/Editar/" + id, function () {
        $("#modal").modal();
    })
});
function CambiarCat() {
    if ($('#DropDownCat').val() == "1") {
        $('#ColorMaterial').slideUp();
    }
    else {
        $('#ColorMaterial').slideDown();
    }
};

