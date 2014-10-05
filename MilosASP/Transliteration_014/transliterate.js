var threeDots = false;
var count = 0;
$(document).ready(function () {
    $('textarea').val('');
    $('textarea').focus();
    $('#arrows').attr('disabled', 'disabled');
    sendValueOne();

    $('#ot-submit').click(function () {
        sendValue($('#source').val());
    });
    $('#result_box').text('');

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

$(function() {
		$( "#rerun" )
			.button()
			.click(function() {
				alert( "Sa jezika: Srpski" );
			})
			.next()
				.button({
					text: false,
					icons: {
						primary: "ui-icon-triangle-1-s"
					}
				})
				.click(function() {
					var menu = $( this ).parent().next().show().position({
						my: "left top",
						at: "left bottom",
						of: this
					});
					$( document ).one( "click", function() {
						menu.hide();
					});
					return false;
				})
				.parent()
					.buttonset()
					.next()
						.hide()
						.menu();
						
		$( "#rerune" )
			.button()
			.click(function() {
				alert( "Sa jezika: Srpski" );
			})
			.next()
				.button({
					text: false,
					icons: {
						primary: "ui-icon-triangle-1-s"
					}
				})
				.click(function() {
					var menu = $( this ).parent().next().show().position({
						my: "left top",
						at: "left bottom",
						of: this
					});
					$( document ).one( "click", function() {
						menu.hide();
					});
					return false;
				})
				.parent()
					.buttonset()
					.next()
						.hide()
						.menu();

		/*$( "#switched" ).button({
            icons: {
                primary: "ui-icon-locked"
            },
            text: false
        })*/
		$( "#arrows" ).button({
            icons: {
                primary: "ui-icon-arrows"
            },
            text: false
        })
		$('#source').bind('keyup cut paste', function(){		
			if($('#source').val().length < 5) 
			{
				$('#arrows').attr({'aria-disabled': 'true'}).addClass('ui-button-disabled ui-state-disabled');
			}
			else
			{
				$('#arrows').attr('aria-disabled', 'false').removeClass('ui-button-disabled ui-state-disabled');
			}
		});
    					
	});


function sendValue(str) {
		if(threeDots == false){
			$('#result_box').html($('#result_box').html() + '...');
			threeDots = true;
		}
	// Fire off AJAX request.		
$.ajax(
		{
			// Define AJAX properties.
		    url: "/TransliterationService.asmx/Transliterate",
			type: "post",				
			data: { text: str },
			dataType: "xml",
 
 
			// Define the success method.
			success: function(xml) {
			    var data = $('string', xml).text();
				data = data.replace(/\n/g, '<br />');
				$('#result_box').html(data);
				threeDots = false;				
                if($('#result_box').text().length <= 50) {
					$('#result_box').addClass('short_text');
				} else {
					$('#result_box').removeClass('short_text');
				}
			},

			// Define the error method.
			error: function( objAJAXRequest, strError ) {
				$( "#response" ).text(
					"Error! Type: " +
					strError
					);
			}
	});
};

function sendValueOne() {
	// Fire off AJAX request.		
$.ajax(
		{
			// Define AJAX properties.
		    url: "/TransliterationService.asmx/GetLanguages",
			type: "POST",				
			data: "{}",
			dataType: "xml",
 
 
			// Define the success method.
			success: function(xml) {			    
                var data = $('string', xml).text();				
				data = data.replace(/\n/g, '<br />');
				//alert(data);
				/*$('#result_box').html(data);
				threeDots = false;				
                if($('#result_box').text().length <= 50) {
					$('#result_box').addClass('short_text');
				} else {
					$('#result_box').removeClass('short_text');
				}*/
			},

			// Define the error method.
			error: function( objAJAXRequest, strError ) {
				$( "#source" ).text(
					"Error! Type: " +
					strError
					);
			}
		});
};



/*function sendValue(str){
	var xmlString = "<textarea><text>" + escape(str) + "</text></textarea>";
	// Fire off AJAX request.		
	$.ajax(
		{
			// Define AJAX properties.
			url: "transliterate.php",
			type: "POST",
			data: xmlString,
			dataType: "text",
 
 
			// Define the success method.
			success: function(data){
				data = data.replace(/%0A/g, '<br />').replace(/%20/g, ' ');
				$('#result_box').html(data);
				if(data.length <= 50) {
					$('#result_box').addClass('short_text');
				}else{
					$('#result_box').removeClass('short_text');
				}
			},
 
 
			// Define the error method.
			error: function( objAJAXRequest, strError ){
				$( "#response" ).text(
					"Error! Type: " +
					strError
					);
			}
	});
};*/