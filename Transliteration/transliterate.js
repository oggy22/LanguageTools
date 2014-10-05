var waitingForTransliteration = false;
var executeAfterOneSecond = false;
var keyUpTime = 0;
var t = 0;
var textBox = '';
var selectedButtonID = '';
var button = [];
srcLang = { code: "", name: "" };
dstLang = { code: "", name: "" };

$(document).ready(function () {

// Sending a request to fill the ul with list of languages--
    GetLanguagesAjax();
// --obtained from the server in response to a request
	
    $('textarea').val('');
    $('#result_box').text('');
    $('textarea').focus();
	$(window).resize(GetButtonWidth);
	
	$("button#rerun, button#rerune").addClass("ui-corner-left");
	$("button#select, button#select1").addClass("ui-corner-right");
	
// Selection of language on click on li element in unordered list and placement into appropriate button
    $("ul").menu({
        select: function (event, ui) {
            if (ui.item.attr("class").match(/selected/g) == "selected") {
            } else {
                var selected = ui.item.text().split("-");
                if (ui.item.parent().attr("id") == "ui-menu-left") {
                    srcLang.code = selected[0];
                    srcLang.name = selected[1]; 
					CallOrNotListTextSamples();
                } else {
                    dstLang.code = selected[0];
                    dstLang.name = selected[1];                    
                }
				if($('#source').val() != "") {
					TransliterateAjax($("#source").val(), srcLang.code, dstLang.code);
				}
                $('#' + selectedButtonID + '> span').text(selected[1]);
                ui.item.parent().find(".selected").removeClass("selected");
                ui.item.addClass("selected");				
            }
        }
    });

// Sending request
	$('#ot-del-textbox').click(function () {
		$("#source").val("").trigger('autosize.resize');
        TransliterateAjax($("#source").val(), srcLang.code, dstLang.code);
    });

    $('#ot-submit').click(function () {
        TransliterateAjax($("#source").val(), srcLang.code, dstLang.code);
    });
	
	$("button#ot-del-textbox").hover(function(){
		$(this).addClass("ot-del-button-hover");
	},
	function(){
		$(this).removeClass("ot-del-button-hover");
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
	$('#source').on('input keydown', function() {
		handler();
	});	
	
	$('#source').bind('cut paste', function() {
		setTimeout(function () { 
			TransliterateAjax($('#source').val(), srcLang.code, dstLang.code);
		}, 0);
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
				    $("#ui-menu-left").show().position({
				        my: "left top",
				        at: "left bottom",
				        of: this
				    });
					$("#ui-menu-right").hide();
				    $(document).one("click", function () {
				        $("#ui-menu-left").hide();
				    });

				    // Assigning a value to a global variable to define 
				    // which button should be assigned the value of the 
				    // li element when selecting a language                    			    
				    selectedButtonID = 'rerun';

				    return false;
				})					
				.next()
					.hide()
					.menu()
					.parent()
					.buttonset();

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
				    $("#ui-menu-right").show().position({
				        my: "left top",
				        at: "left bottom",
				        of: this
				    });
					$("#ui-menu-left").hide();
				    $(document).one("click", function () {
				        $("#ui-menu-right").hide();
				    });

				    // Assigning a value to a global variable to define 
				    // which button should be assigned the value of the 
				    // li element when selecting a language 
				    selectedButtonID = 'rerune';

				    return false;
				})				
				.next()
					.hide()
					.menu()
					.parent()
					.buttonset();

    $("#swap").button({
        icons: {
            primary: "ui-icon-arrows"
        },
        text: false
    })// Changing the content of the left and right boxes and languages
    .click(function (event) {
        //event.preventDefault();
        var textRight = $('#result_box').html();
		textRight = textRight.replace(/<br\s*[\/]?>/gi, "\n");
        var swapName = dstLang.name;
		var swapCode = dstLang.code;
        dstLang.name = srcLang.name;
        srcLang.name = swapName;
		dstLang.code = srcLang.code;
		srcLang.code = swapCode;
        $('#source').val(textRight).trigger('autosize.resize');
        $('button#rerun span').text(srcLang.name);
        $('button#rerune span').text(dstLang.name);
        $("#ui-menu-left").find(".selected").removeClass("selected").end()
			.find("li:contains('" + srcLang.name + "')").addClass("selected");
        $("#ui-menu-right").find(".selected").removeClass("selected").end()
			.find("li:contains('" + dstLang.name + "')").addClass("selected");
		TransliterateAjax($('#source').val(), srcLang.code, dstLang.code);
		CallOrNotListTextSamples();
    });

});


function TransliterateAjax(text, srcLang, dstLang) {				

	// Fire off AJAX request.
    $.ajax({

			// Define AJAX properties.
		    beforeSend: function () {
		        if (waitingForTransliteration == false) {
		            $('#result_box').html($('#result_box').html() + '...');
		            waitingForTransliteration = true;
		        }
		    },
		    url: "/TransliterationService.asmx/Transliterate",
		    type: "POST",
		    data: { "text": text, "srcLang": srcLang, "dstLang": dstLang },
		    dataType: "xml",
			//timeout: (1 * 10),


		    // Define the success method.
		    success: function (xml) {
				if($('#error_box').text() != ''){
					$('#error_box').text('');
				}
		        var data = $('string', xml).text();
		        data = data.replace(/\n/g, '<br />');
		        $('#result_box').html(data);
		        waitingForTransliteration = false;
		        if ($('#result_box').text().length <= 50) {
		            $('#result_box').addClass('short_text');
		        } else {
		            $('#result_box').removeClass('short_text');
		        }
				var resultBoxHeight = parseInt($('#result_box').css('height')) + parseInt($('#result_box').css('padding-bottom')) + parseInt($('#result_box').css('padding-top')) + 'px';
				var sourceBoxHeight = $('#source').css('height');
				console.log(resultBoxHeight);
				console.log(sourceBoxHeight);
				if(resultBoxHeight >= sourceBoxHeight) {										
					$('div.ot-hl-layer').css('min-height', resultBoxHeight);
					console.log(sourceBoxHeight);
					console.log($('#source').css('min-height'));
				}else{	
					console.log(sourceBoxHeight);
					$('#result_box').css('min-height', $('#source').css('height'));
				}
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError, message) {
		        $('#error_box').text(
					"Something went wrong. Please reload the page!"
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
		    dataType: "xml",
		    //timeout: (1 * 1000),


		    // Define the success method.
		    success: function (xml) {
				if($('#error_box').text() != ''){
					$('#error_box').text() = '';
				}
		        $(xml).find("Language").each(function () {
		            var code = $(this).find("Code").text();
		            var name = $(this).find("Name").text();
		            if (code == 'EN') {
		                $('#rerun span').text(name);
		                srcLang.code = code;
		                srcLang.name = name;
		            }
		            else if (code == 'SR') {
		                $('#rerune span').text(name);
		                dstLang.code = code;
		                dstLang.name = name;
		            }
		            var string = code + "-" + name;
		            $('ul.ui-menu').append('<li><a>' + string + '</a></li>');
		        });
		        $(".ui-menu").menu("refresh");		        
		        $("#ui-menu-left").find("li:contains('" + srcLang.code + "')").addClass("selected");
		        $("#ui-menu-right").find("li:contains('" + dstLang.code + "')").addClass("selected");
				ListTextSamples(srcLang.code);
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError, message) {
		        $("#error_box").text(
					"Something went wrong. Please reload the page!"
					);
		    }
		});
};

function ListTextSamples(str) {

	// Fire off AJAX request.
    $.ajax(
		{

		    // Define AJAX properties.
		    url: "/TransliterationService.asmx/ListTextSamples",
		    type: "POST",
			data: { 'code': str },
		    dataType: "xml",
		    //timeout: (1 * 10),


		    // Define the success method.
		    success: function (xml) {
				if($('#error_box').text() != ''){
					$('#error_box').text() = '';
				}
				var title, text, obj;
				var inbutton = [];
		        $(xml).find("TextSample").each(function () {		            
					title = $(this).find("Title").text();
		            text  = $(this).find("Text").text();
					obj	  = {};
					obj['title'] = title;
					obj['text']  = text;
					inbutton.push(obj);
		        });
				obj	  = {};
				obj = { 'lang': srcLang.code, 'value': inbutton };
				button.push(obj);
				CreateButtons(inbutton);
		    },

		    // Define the error method.
		    error: function (objAJAXRequest, strError, message) {
		        $("#error_box").text(
					"Something went wrong. Please reload the page!"
					);
		    }
		});
};

function CallOrNotListTextSamples() {
	var a = 1000;
	var i = 0;
	while(i < button.length) {
		if(srcLang.code == button[i].lang) {
			a = i;
		}		
		i++;
	}
	if(a != 1000) {
		CreateButtons(button[a].value);
	}else {
		ListTextSamples(srcLang.code);
	}
}

function CreateButtons(inbutton) {
	//$('#ot-sample-box').css("visibility", "hidden");
	var size = inbutton.length;
	//var delimited = 100/size;
	$(function() {	
		var container = $("<div></div>").attr('id', 'ot-sample-button');
		var inner, outter//, difference = 0, width = 0, padding = 0;
		for(i=0; i<size; i++) {
			outter = 	$("<div/>", {
							"class": "outter",
							//width: delimited + "%"
						});
			inner = $("<div/>", {
						role: "button",
						value: srcLang.code,
						tabindex: "0",
						"class": "sample-button sample-button-standard"
					}).text(inbutton[i].title).data('fraze', inbutton[i].text);
			if(i != 0) {
				inner.addClass("sample-button-collapse-left");
			}
			//alert(inner.width());
			outter.append(inner);
			container.append(outter);	
		}
		//alert(inner.width());
		$('#ot-sample-box').html(container);
		GetButtonWidth();
		
		/*difference = $('#ot-sample-button').width() - width;
		paddingLeft  = parseInt($('.sample-button-standard').css("padding-left").replace("px", '')) + difference / (size * 2) + "px";
		alert(paddingLeft);
		alert(difference / (size * 2));
		paddingRight = parseInt($('.sample-button-standard').css("padding-right").replace("px", '')) + difference / (size * 2) + "px";
		//alert($('#ot-sample-button').width()+ "\n" + width);
		$('.sample-button-standard').css({"padding-left":paddingLeft, "padding-right":paddingRight});*/
		
		//$('#ot-sample-box').html(container).css("visibility", "visible");
		
		//sample-button
		$(".sample-button").hover(function(){
			$(this).addClass("sample-button-hover");
		},
		function(){
			$(this).removeClass("sample-button-hover");
		})
		.click(function(){
			$('#source').val($(this).data('fraze')).trigger('autosize.resize');
			if($('#source').css('height') > $('div.ot-hl-layer').css('min-height')){
				$('div.ot-hl-layer').css('min-height', $('#source').css('height'));
			}
			if($('#source').val() != "") {
				TransliterateAjax($('#source').val(), srcLang.code, dstLang.code);
			}
		});					
		
	});
}

function GetButtonWidth() {
	var size=0, width = 0, difference = 0, padding = 0;
	for(i=0; i<button.length; i++) {
		if(button[i].lang == srcLang.code) {
			size = button[i].value.length;
		}
	}
	$('#ot-sample-box .outter').each(function(){
			width += $(this).width();
	});
	difference = $('#ot-sample-button').width() - width;
	padding = difference / (size * 2);
	$('#ot-sample-box .outter').each(function(){
		width = ($(this).width() + padding * 2) / $('#ot-sample-button').width() * 100;
		$(this).css("width", width + "%");
	});
}

function handler() {
    if (document.getElementById('source').value != textBox) {
        if (executeAfterOneSecond == false) {
            executeAfterOneSecond = true;
            t = setTimeout(function () {
                executeAfterOneSecond = false;
                TransliterateAjax(document.getElementById('source').value, srcLang.code, dstLang.code);
                }, 800);
        };				
        keyUpTime = Date.now();
        setTimeout(function () {
            if (Date.now() - keyUpTime >= 290) {
                clearTimeout(t);
                executeAfterOneSecond = false;
                TransliterateAjax(document.getElementById('source').value, srcLang.code, dstLang.code);
            };
        }, 300);
        textBox = document.getElementById('source').value;
    };
};