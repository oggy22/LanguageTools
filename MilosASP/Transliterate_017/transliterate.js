var waitingForTransliteration = false;
var selectedButtonID = '';
$(document).ready(function () {    
    
	// Sending a request to fill the ul with list of languages--
	GetLanguagesAjax();
	// --obtained from the server in response to a request	
    
	$('textarea').val('');
    $('#result_box').text('');
    $('textarea').focus();    
    
	// Changing the content of the left and right boxes and languages
    $("#arrows").button().click(function (event) {
        event.preventDefault();
        var strLeft = $('#source').val();
        var strRight = $('#result_box').text();
        var langLeft = $('button#rerun span').text();
        var langRight = $('button#rerune span').text();
        $('#source').val(strRight);
        $('#result_box').html(strLeft);
        $('button#rerun span').text(langRight);
        $('button#rerune span').text(langLeft);
    });
    
	// Selection of language on click on li element in unordered list and placement into appropriate button
    $("ul > li").live('click', function (e) {        
        $('#' + selectedButtonID + '> span').text($(this).text().split("-")[1]);
    });
    
	// Sending request
    $('#ot-submit').click(function () {
        TransliterateAjax($('#source').val());
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
    var executeAfterOneSecond = false;
    $('#source').bind('keydown cut paste', function () {
        setTimeout(function () {
            if ($('#source').val() != textBox) {
                if (executeAfterOneSecond == false) {
                    executeAfterOneSecond = true;
                    t = setTimeout(function () {
                        executeAfterOneSecond = false;
                        TransliterateAjax($('#source').val());
                    }, 800);
                }
                keyUpTime = $.now();
                setTimeout(function () {
                    if ($.now() - keyUpTime >= 300) {
                        clearTimeout(t);
                        executeAfterOneSecond = false;
                        TransliterateAjax($('#source').val());
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


function TransliterateAjax(str) {				

	// Fire off AJAX request.
    $.ajax(
		{

			// Define AJAX properties.
		    beforeSend: function () {
		        if (waitingForTransliteration == false) {
		            $('#result_box').html($('#result_box').html() + '...');
		            waitingForTransliteration = true;
		        }
		    },
		    url: "/TransliterationService.asmx/Transliterate",
		    type: "POST",
		    data: { text: str },
		    dataType: "xml",
		    timeout: (5 * 1000),


		    // Define the success method.
		    success: function (xml) {
		        var data = $('string', xml).text();
		        data = data.replace(/\n/g, '<br />');
		        $('#result_box').html(data);
		        waitingForTransliteration = false;
		        if ($('#result_box').text().length <= 50) {
		            $('#result_box').addClass('short_text');
		        } else {
		            $('#result_box').removeClass('short_text');
		        }
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError, message) {
		        $('#result_box').text(
					"Error occured. Try refreshing the page"
					);
		    }
		});
};

function GetLanguagesAjax() {

	// Fire off AJAX request.
    $.ajax(
		{

			// Define AJAX properties.
		    url: "/TransliterationService.asmx/GetLanguages",
		    type: "POST",
		    data: "{}",
		    dataType: "xml",
            timeout: (5 * 1000),


		    // Define the success method.
		    success: function (xml) {
		        $(xml).find("Language").each(function () {
		            var code = $(this).find("Code").text();
		            var name = $(this).find("Name").text();
		            var string = code + "-" + name;
		            $('ul.ui-menu').append('<li><a href="#">' + string + '</a></li>');		            
		        });
				$(".ui-menu").menu("refresh");
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError, message) {
		        $('#result_box').text(
					"Error occured. Try refreshing the page"
					);
		    }
		});
};