
$(document).ready(function(){
   	
    $('#chk1').click(function() {
    if( $(this).is(':checked')) {
        $("#autoUpdate").show();
    } else {
        $("#autoUpdate").hide();
    }
	}); 
});