<?php
// Account and Password
$Account = $_REQUEST["Account"];
$Password = $_REQUEST["Password"];
//Php Only
$Hostname = "localhost";
$DBName = "accounts";
$User = "root";
$PasswordP = "";

mysql_connect($Hostname, $User, $PasswordP) or die ("Can't connect to DB");
mysql_select_db($DBName) or die ("Can't connect to DB");

if(!$Account || !$Password)
{
	echo"Empty";
}
else{
	$SQL = "SELECT * FROM accounts WHERE Account= '" . $Account . "'";
	$Result = @mysql_query($SQL) or die ("DB Error");
	$Total = mysql_num_rows($Result);
	if ($Total == 0)
	{
		$insert = "INSERT INTO `accounts` (`Account`, `Password`) VALUES ('" . $Account . "', MD5('" . $Password . "'))";
		$SQL1 = mysql_query($insert);
		echo"Success";
	}
	else{
		echo"AlreadyUsed";
	}
}
//Close MYSQL
mysql_close();
?>