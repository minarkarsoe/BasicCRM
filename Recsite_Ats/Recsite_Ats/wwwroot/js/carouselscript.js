

$(".navbar-toggler").click(function(){

  $(".navbar-toggler").toggleClass("showtoggle");

});



$(".closed").click(function(){

  $(".navbar-collapse").removeClass("show");

});   


    $(".jobsbtn a").click(function(){
        $(".topjobbox").slideToggle("slow");
    });


    $(".searh_bx a").click(function(){
        $(".topjobbox").slideToggle("slow");
    });


$(".createNew").click(function(){
        $("#createNewModal").toggleClass("show");
        $(".backShdow").toggleClass("act");
    });
$(".btn-close").click(function(){
        $("#createNewModal").removeClass("show");
        $(".backShdow").removeClass("act");
    });  
 $(".backShdow").click(function(){
        $("#createNewModal").removeClass("show");
        $(".backShdow").removeClass("act");
    });    
    

$(window).scroll(function() {    
    var scroll = $(window).scrollTop();
    if (scroll >= 50) {
        $(".header_sec").addClass("fixed");

    } else {
        $(".header_sec").removeClass("fixed");
    }
    
});

	



$(document).ready(function() {
var owl = $('.job_sec .owl-carousel');
  owl.owlCarousel({
    margin:30,
    dots: false,
    dotsEach: false,
    autoplay: false,
    nav: true,
    loop: true,
    responsive: {
      1: {	
      	margin:20,
      	autoHeight: true,
        items: 1
      },
      575: {	
      	margin:20,
      	autoHeight: true,
        items: 1
      },
      640: {	
      	margin:20,
        items: 2
      },
      768: {
      	margin:20,
        items: 2
      },         
      992: {
      	margin:20,
        items: 3
      },
      1200: {
        items: 3
      }
    }
  });
});

$(document).ready(function() {
var owl = $('.abtcaro');
  owl.owlCarousel({
    margin:0,
    dots: true,
    dotsEach: false,
    autoplay: true,
    nav: false,
    loop: true,
    responsive: {
      1: {	
        items: 1
      },
      1200: {
        items: 1
      }
    }
  });
});



$(document).ready(function(){
	
	$("#extend").click(function(e){
		e.preventDefault();
		$("#extend-field").append('<div class="addFld"><input type="text"><a href="#" class="remove-extend-field plus"><i class="fa-solid fa-circle-minus"></i></a>');
	});

	
	$("#extend-field").on("click",".add-text-field",function(e){
		e.preventDefault();
		$("#extend-field").append('<div class="addFld"><a href="#" class="remove-extend-field plus"><i class="fa-solid fa-circle-minus"></i></a>')
		

	});

	$("#extend-field").on("click",".remove-extend-field",function(e){
		e.preventDefault();
		$(this).parent('div').remove();
	});

	
});




$(document).ready(function(){
	
	$("#extend1").click(function(e){
		e.preventDefault();
		$("#extend-field1").append('<div class="addFld"><input type="text"><a href="#" class="remove-extend-field plus"><i class="fa-solid fa-circle-minus"></i></a>');
	});

	
	$("#extend-field1").on("click",".add-text-field",function(e){
		e.preventDefault();
		$("#extend-field").append('<div class="addFld"><a href="#" class="remove-extend-field plus"><i class="fa-solid fa-circle-minus"></i></a>')
		

	});

	$("#extend-field1").on("click",".remove-extend-field",function(e){
		e.preventDefault();
		$(this).parent('div').remove();
	});

	
});

