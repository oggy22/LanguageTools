var threeDots = false;
var count = 0;
var selectedButtonID = '';
$(document).ready(function () {    
    sendValueOne();
    $('textarea').val('');
    $('textarea').focus();
    $('#arrows').button({ disabled: true });
    $('#source').bind('keyup cut paste', function () {
        if ($('#source').val().length < 5) {
            $('#arrows').attr({ 'aria-disabled': 'true' }, { 'disabled': 'disabled' }).addClass('ui-button-disabled ui-state-disabled');
            $('#arrows').button("option", "disabled", true);
        }
        else {
            $('#arrows').attr({ 'aria-disabled': 'false' }, { 'disabled': 'enabled' }).removeClass('ui-button-disabled ui-state-disabled');
            $('#arrows').button("option", "disabled", false);
        }
    });

    $("#arrows").button().click(function (event) {
        event.preventDefault();
        var strOne = $('#source').val();
        var strTwo = $('#result_box').text();
        var langOne = $('button#rerun span').text();
        var langTwo = $('button#rerune span').text();
        $('#source').val(strTwo);
        $('#result_box').html(strOne);
        $('button#rerun span').text(langTwo);
        $('button#rerune span').text(langOne);
    });

    $("ul > li").live('click', function (e) {        
        $('#' + selectedButtonID + '> span').text($(this).text().split("-")[1]);
    });

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
				    selectedButtonID = 'rerun';
				    return false;
				})
				.parent()
					.buttonset()
					.next()
						.hide()
						.menu();

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
		    success: function (xml) {
		        alert('kljlkjlk');
		        var data = $('string', xml).text();
		        data = data.replace(/\n/g, '<br />');
		        $('#result_box').html(data + 'dodato');
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
		    success: function (xml) {
		        $(xml).find("Language").each(function () {
		            var code = $(this).find("Code").text();
		            var name = $(this).find("Name").text();
		            var string = code + "-" + name;
		            $('ul.ui-menu').append('<li><a href="#">' + string + '</a></li>');
		            $(".ui-menu").menu("refresh");
		        });	
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError) {
		        $("#source").text(
					"Error! Type: " +
					strError
					);
		    }
		});
};