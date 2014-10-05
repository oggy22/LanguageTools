<?php 

$xml_post = file_get_contents('php://input');
$newXML = new SimpleXMLElement($xml_post);
echo $newXML->text;  

?>