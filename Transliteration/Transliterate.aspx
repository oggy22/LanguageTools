﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transliterate.aspx.cs" Inherits="Transliteration.Transliterate" %>

<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link rel="stylesheet" type="text/css" href="transliterate.css" />
<link rel="stylesheet" type="text/css" href="jquery-ui-1.9.2.custom.css" />
<script src="jquery_mini.js" type="text/javascript" charset="utf-8"></script>
<script src="transliterate.js" type="text/javascript" charset="utf-8"></script>
<script src="jquery.autosize.js" type="text/javascript" charset="utf-8"></script>
<script src="jquery-ui.custom.js" type="text/javascript" charset="utf-8"></script>
<title>Transliterator</title>
<style type="text/css">
	.ui-menu { position: absolute; width: 200px; z-index: 2; }	
</style>
</head>
<body class="">
<div id="ob" class="">
	<div id="obw">
		<div id="obzw" class="">			
			<div id="oby">
				<div id="obyc">
					<a id="obys">Transliterator</a>
				</div>
			</div>
		</div>
	</div>
	<div id="obx3" class=""></div>
</div>
<div id="ot-m" class="m-section">
	<div id="ot-middle-c">
		<div id="ot-middle">
			<div id="ot-main">
				<div id="ot-middle-elements">
					<div id="ot-button-bar">
						<div id="ot-button-bar-left" class="button-inline-block">
							<div id="ot-lang-src">
								<button id="rerun"></button>
								<button id="select">Choose a language</button>
								<ul id="ui-menu-left">
                                <li>EN-English</li>
                                <li>SR-English</li>
                                <li>MK-English</li>
                                <li>RU-English</li>

<%--        <%
            var rep = Transliteration.Global.repository;
            
            foreach (var lang in rep.ListLaguagesCached)
            {
                Response.Write("<li>" + lang.Code + " - " + lang.Name + "</li>" + Environment.NewLine);       
            } 
        %>
--%>
								</ul>
							</div>
							<div id="ot-swap">
								<button id="arrows">Swap languages</button>
							</div>
						</div>						
						<div id="ot-button-bar-right" class="button-inline-block">
							<div id="ot-lang-dst">
								<button id="rerune"></button>
								<button id="select1">Choose a language</button>
								<ul id="ui-menu-right">
                                					
								</ul>
							</div>
							<div id="ot-lang-submit">
								<button id="ot-submit" class="ot-button ot-button-action" type="button" style="tabindex: 0;">Transliterate</button>						
							</div>
						</div>					
					</div>
					<div id="ot-box-bar">											
						<div id="ot-src-box" class="o-unit">
							<div id="ot-src-p">								
								<div id="ot-src-wrap" class="" style="">
									<div style="width: 100%;">									
										<div class="ot-hl-layer" style="box-sizing: border-box; height: 125px; left: 0px; top: 0px; padding-left: 8px; padding-right: 20px; dir: ltr; ">
										</div>
										<textarea id="source" name="text" tabindex="0" dir="ltr" spellcheck="false" autocapitalize="off" autocomplete="off" autocorrect="off" style="overflow-y: hidden; overflow-x: auto; padding-right: 20px; " class="oggy-textarea">
										</textarea>
									</div>								
								</div>
							</div>
							<div id="ot-sample-box">								
							</div>
						</div>
						<div id="ot-dst-box" class="o-unit">
							<div id="ot-res-p">
								<div id="ot-res-data" style="">
									<div id="ot-tgt-lang-sugg" class="ot-tgt-lang-sugg-message" style=""></div>
									<div id="ot-res-wrap">
										<div id="ot-res-content" class="almost_half_cell" style="">
											<div dir="ltr" style="zoom:1">											
												<span id="result_box" lang>
												</span>
											</div>
										</div>														
									</div>
								</div>
							</div>
						</div>						
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</body>
</html>