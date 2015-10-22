
$(document).ready(function () {
    CambiarCat();
    var $modal = $('#modal');
    $modal.on('hidden.bs.modal', function (e) {
        $("#modal").removeData('bs.modal');
        parent.document.location = parent.document.location;
    });
   
});
function CambiarCat() {
    if ($('#DropDownCat').val() == "1") {
        $('#ColorMaterial').slideUp();
        $("#subCat").slideUp();
        $("#CostoMatterial").slideDown();
    }
    else {
        $("#CostoMatterial").slideUp();
        $('#ColorMaterial').slideDown();
        $("#subCat").slideDown();

    }
};


