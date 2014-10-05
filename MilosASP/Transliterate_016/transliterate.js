var threeDots = false;  //oggy: preimenuj ovo u waitingForTransliteration jer ti ova promenljiva govori da li cekas od servera odgovor (tj transliteraciju teksta).
var count = 0;  //oggy: koliko vidim ne koristis ovu promenljivu nigde? Izbrise je onda.
var selectedButtonID = '';
$(document).ready(function () {    
    // Sending a request to fill the ul with list of languages--
	sendValueOne(); //oggy: pre svakog komentara mozes da odvojis jedan red radi prelgednosti, kao npr sledeci red
	// --obtained from the server in response to a request	
    $('textarea').val('');
    $('#result_box').text('');
    $('textarea').focus();
    $('#arrows').button({ disabled: true });
    // Disable or enable button depending on the number of characters   //oggy: what button? button for swithing lanugages?
    $('#source').bind('keyup cut paste', function () {
        if ($('#source').val().length < 5) {          //oggy: why 5? You probably don't need this as we discussed. Verovatno ce ti ovako nesto trebati za "Recognize Language", jer to treba da bude disabled ako je tekst kratak, npr manje od 5 karaktera. U svakom slucaju, switch button treba uvek da bude enabled
            $("#arrows").button("disable");
        }
        else {            
            $("#arrows").button("enable");
        }
    });
    // Changing the content of the left and right boxes and languages
    $("#arrows").button().click(function (event) {
        event.preventDefault();
        var strOne = $('#source').val();    //oggy: rename it to Left and Rigth instead of One and Two please
        var strTwo = $('#result_box').text();
        var langOne = $('button#rerun span').text();    //oggy: rename it to Left and Rigth instead of One and Two please
        var langTwo = $('button#rerune span').text();
        $('#source').val(strTwo);
        $('#result_box').html(strOne);
        $('button#rerun span').text(langTwo);
        $('button#rerune span').text(langOne);
    });
    // Selection of language on click on li element in unordered list and placement into appropriate button
    $("ul > li").live('click', function (e) {        
        $('#' + selectedButtonID + '> span').text($(this).text().split("-")[1]);
    });
    // Sending request
    $('#ot-submit').click(function () {
        sendValue($('#source').val());
    });
    // Appearance settings for different resolutions
    if ($(window).width() <= 1060 || $(window).height() <= 570) {
        $('body').addClass('e_sm');
        $('#ob').addClass('obes');
        $('#obx1').addClass('obes');
        $('#obx3').addClass('obes');
    } else if (($(window).width() > 1060 && $(window).width() <= 1250) || ($(window).height() > 570 && $(window).height() <= 590)) {
        $('body').addClass('e_md');
        $('#ob').addClass('obem');
        $('#obx1').addClass('obem');
        $('#obx1').addClass('obem');
    } else {
        $('body').addClass('e_lg');
    }
    $(window).resize(function () {
        if ($(window).width() <= 1060 || $(window).height() <= 570) {
            var cl = $('body').attr('class');
            var clob = $('#ob').attr('class');
            if (cl != 'e_sm') {
                $('body').removeClass(cl).addClass('e_sm');
                $('#ob').removeClass(clob).addClass('obes');
                $('#obx1').removeClass(clob).addClass('obes');
                $('#obx3').removeClass(clob).addClass('obes');
            }
        } else if (($(window).width() > 1060 && $(window).width() <= 1250) || ($(window).height() > 570 && $(window).height() <= 590)) {
            var cl = $('body').attr('class');
            var clob = $('#ob').attr('class');
            if (cl != 'e_md') {
                $('body').removeClass(cl).addClass('e_md');
                $('#ob').removeClass(clob).addClass('obem');
                $('#obx1').removeClass(clob).addClass('obem');
                $('#obx3').removeClass(clob).addClass('obem');
            }
        } else {
            var cl = $('body').attr('class');
            var clob = $('#ob').attr('class');
            if (cl != 'e_lg') {
                $('body').removeClass(cl).addClass('e_lg');
                $('#ob').removeClass(clob);
                $('#obx1').removeClass(clob);
                $('#obx3').removeClass(clob);

            }
        }
    });
    // Function to send requests
    var keyUpTime = 0;
    var t = 0;
    var textBox = '';
    var executeAfterOneSecond = false;  //oggy: what's that for? why are you waiting for a second?
    $('#source').bind('keydown cut paste', function () {
        setTimeout(function () {
            if ($('#source').val() != textBox) {
                if (executeAfterOneSecond == false) {
                    executeAfterOneSecond = true;
                    t = setTimeout(function () {
                        executeAfterOneSecond = false;
                        sendValue($('#source').val());
                    }, 800);
                }
                keyUpTime = $.now();
                setTimeout(function () {
                    if ($.now() - keyUpTime >= 300) {
                        clearTimeout(t);
                        executeAfterOneSecond = false;
                        sendValue($('#source').val());
                    }
                }, 300);
                textBox = $('#source').val();
            }
        }, 10)
    });
    $('#source').autosize();

});

// Defining functions with clicking on the buttons with 'rerun' id and 'select' id  
$(function () {
    $("#rerun")
			.button()
			.click(function (event) {
			    event.preventDefault();
			})
			.next()
				.button({
				    text: false,
				    icons: {
				        primary: "ui-icon-triangle-1-s"
				    }
				})
				.click(function () {
				    var menu = $(this).parent().next().show().position({
				        my: "left top",
				        at: "left bottom",
				        of: this
				    });
				    $(document).one("click", function () {
				        menu.hide();
				    });
				    // Assigning a value to a global variable to define 
                    // which button should be assigned the value of the 
                    // li element when selecting a language                    			    
				    selectedButtonID = 'rerun';
				    return false;
				})
				.parent()
					.buttonset()
					.next()
						.hide()
						.menu();
	// Defining functions with clicking on the buttons with 'rerune' id and 'select1' id
    $("#rerune")
			.button()
			.click(function (event) {
			    event.preventDefault();
			})
			.next()
				.button({
				    text: false,
				    icons: {
				        primary: "ui-icon-triangle-1-s"
				    }
				})
				.click(function () {
				    var menu = $(this).parent().next().show().position({
				        my: "left top",
				        at: "left bottom",
				        of: this
				    });
				    $(document).one("click", function () {
				        menu.hide();
				    });
				    // Assigning a value to a global variable to define 
				    // which button should be assigned the value of the 
				    // li element when selecting a language 
				    selectedButtonID = 'rerune';
				    return false;
				})
				.parent()
					.buttonset()
					.next()
						.hide()
						.menu();

    $("#arrows").button({
        icons: {
            primary: "ui-icon-arrows"
        },
        text: false
    })

});


function sendValue(str) {   //oggy: rename it to something more meaningfull, e.g. TransliterateAjax
		if(threeDots == false){
			$('#result_box').html($('#result_box').html() + '...');
			threeDots = true;
		}
	// Fire off AJAX request.
		$.ajax(
		{
		    // Define AJAX properties.
		    url: "/TransliterationService.asmx/Transliterate",
		    type: "post",   //oggy: capital letters "POST"
		    data: { text: str },
		    dataType: "xml",


		    // Define the success method.
		    success: function (xml) {
		        var data = $('string', xml).text();
		        data = data.replace(/\n/g, '<br />');
		        $('#result_box').html(data);
		        threeDots = false;
		        if ($('#result_box').text().length <= 50) {
		            $('#result_box').addClass('short_text');
		        } else {
		            $('#result_box').removeClass('short_text');
		        }
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError) {
		        $("#response").text(
					"Error! Type: " +
					strError
					);
		    }
		});
};

function sendValueOne() {   //oggy: rename it to something more meaningfull, e.g. GetLanguagesAjax. Also be consistent with another function (TransliterateAjax) in naming them
	// Fire off AJAX request.
    $.ajax(
		{
		    // Define AJAX properties.
		    url: "/TransliterationService.asmx/GetLanguages",
		    type: "POST",
		    data: "{}", //oggy: proveri dal ti ovo ovde uopste treba posto je prazno. Znaci probaj da ga maknes i da vidis dal radi bez njega.
		    dataType: "xml",


		    // Define the success method.
		    success: function (xml) {
		        $(xml).find("Language").each(function () {
		            var code = $(this).find("Code").text();
		            var name = $(this).find("Name").text();
		            var string = code + "-" + name;
		            $('ul.ui-menu').append('<li><a href="#">' + string + '</a></li>');
		            $(".ui-menu").menu("refresh");  //oggy: I don't think you need to refresh everytime you add a new item. Try refreshing at the end, once you have added all of them
		        });	
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError) {//oggy: ako se kojim slucajem ovo desi, kako ce da se manifestuje?
		        $("#source").text(
					"Error! Type: " +   //oggy: mozes da stavis nesto kao "Error occured. Try refreshing the page"
					strError
					);
		    }
		});
};