
    $(document).ready(function () 
    {    
        $('#ListaParametro').on("change", function () {

            if ($('#ListaParametro').val() == 5) {
                $('#Listacategoria').slideDown();
                $('#Porcentaje').slideUp();
            }
            if ($('#ListaParametro').val() == 6) {
                $('#Porcentaje').slideDown();
                $('#Listacategoria').slideUp();
            }
            if($('#ListaParametro').val() !=5&&$('#ListaParametro').val() != 6){
                $('#Porcentaje').slideUp();
                $('#Listacategoria').slideUp();
                }
            
           
            
        });
    });
