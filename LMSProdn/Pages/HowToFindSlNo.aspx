<!DOCTYPE html
    PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3c.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Aruba License Management</title>
	<link rel="stylesheet" href="../CSS/Style.css" type="text/css" media="screen" />
	<link rel="stylesheet" href="../CSS/Style.css" type="text/css" media="print" />
	<style type="text/css">
		<!--

body {
	padding: 50px 10px 10px;
}

#helpcontent {
	width: 500px;
	margin: auto;
}

p {
	line-height: 1.5em;
	text-align: justify;
}
dt {
	font-size: 14px;
	font-weight: bold;
	margin-top: 10px;
	color: #587993;
}

dd {
	margin-bottom: 25px;
}

code {
	font-family: "Andale Mono", monospace;
	background-color: #FFC080;
	border: 1px solid #bccad6;
	color: #000;
	padding: 5px;
	width: 90%;
	display: block;
	margin: 10px auto;
}

.helpimage {
	position: relative;
	text-align: center;
	margin: 15px auto;
}

.zoom {
	background: url(../Images/zoom.png) no-repeat;
	position: absolute;
	bottom: -23px;
	right: -25px;
	display: block;
	padding-top: 46px;
	height: 0 !important;
	width: 85px;
	overflow: hidden;
}

		-->
	</style>
	<script type="text/javascript" src="/lib/js/global.js"></script>

</head>
<body>
	<div id="helpcontent" style="width: 887px">
	<asp:Image ID="Image1" runat="server" Height="70px" Width="196px" ImageUrl="~/Images/header-newlogo-aruba.jpeg" />
<br /><br />
<p>There are 3 different ways to retrieve the serial number of your switch:</p>

<dl>
<dt>Physical Label:</dt>
<dd><p>The system is physically labeled with its serial number when it leaves
the factory.  The serial number to use depends on the type of switch
that you have.</p>

<p>For a 5000 or a 6000 model switch, you will need to retrieve the serial
number of the Supervisor Card (SC) card that you are applying the license to.  To do this,
physically remove the SC card from the system chassis and look on the
motherboard near the front panel of the card on the side opposite to the
compact flash slot.  You should see text similar to "ARUBA NETWORKS (c)
2002 S/N#" and then a sticker with the serial number and bar code
printed on it.  For example <em>C00007244</em>.  This is the serial number to
input into the system.</p>

<p>For all other switch models, you can find the serial number on the back
of your switch in the top left of the regulatory compliance sticker,
where it will have text similar to-
"ARUBA WIRELESS NETWORKS INC.  MODEL: 2400   S/N: A200000100"</p>

<p>The serial number is everything after the "S/N:".  In the above example,
the serial number is <em>A200000100</em></p>
</dd>

<dt>Web GUI:</dt>
<dd><p>There are currently two methods.</p>
<h4 style="margin-top: 10px;">Method #1</h4>
<p>Connect to the switch via your web browser and login.  After logging in,
click on the "Maintenance" tab.  Go to "Switch &gt;&gt; License Management."  On that page, in the "License Information" section, there is a statement "You will need the following..."  The serial number to use to generate licenses for this switch is shown as </p>

<div class="helpimage" style="width: 387px;">
<img src="../Images/serial_loc_maintenance.png" alt="Serial Number location in Web GUI" /><br />

<a class="zoom" style="right: -23px; bottom: -21px" href="../Images/serial_loc_maintenance_lg.png" target="_blank"> Zoom</a>
</div>

<h4 style="margin-top: 10px;">Method #2</h4>
<p>Connect to the switch via your web browser and login.  After logging in,
click on "Switch &gt;&gt; Inventory" and detailed hardware information will be
presented.</p>

<p>The serial number to use depends on the type of switch that you have.
The model of the switch is listed in the output as "Switch Model" under
the "General" information section of "Software Information" further down
the page.</p>

<p>For a 5000 or a 6000, use the serial number listed as the "SC Serial#",
for example <em>P00000662</em> in the following output-</p>
<p><code>SC      Serial#                 : P00000662 (Date:08/20/03)</code></p>

<p>For all other switch models, use the serial number listed as the "System
Serial#", for example <em>A100000100</em> is the following output</p>
<p><code>System Serial#                  : A10000100</code></p>
</dd>

<dt>Switch CLI:</dt>
<dd><p>Connect to the switch either through the serial interface or via a
telnet/ssh session.  After logging into your switch, enter the following command</p>

<p><code>&gt; show inventory</code></p>

<p>The serial number to use depends on the type of switch that you have.
In order to find the switch model, you can use the "show version"
command and it is listed in the second line of the output.</p>

<p>For a 5000 or a 6000, use the serial number listed as the "SC Serial#",
for example <em>P00000662</em> in the following output-</p>

<p><code>SC      Serial#                 : P00000662 (Date:08/20/03)</code></p>

<p>For all other switch models, use the serial number listed as the "System
Serial#", for example <em>A100000100</em> is the following output</p>
<p><code>System Serial#                  : A10000100</code></p>
</dd>

</dl>
</div>

</body>
</html>