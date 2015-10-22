$(document).ready(function () {
    $('body').on('hidden.bs.modal', '.modal', function () {
        alert("aqui");
        $(this).removeData('bs.modal');
    });
});

$(".edit").click(function () {
       var id = $(this).attr("data-id");
   $("#modal").load("/Material/Editar/" + id, function () {
        $('#modal').modal("show");
    })
   /*window.showModalDialog("/Material/Editar/" + id, "", "dialogWidth:600px;dialogHeight:900px");*/
});