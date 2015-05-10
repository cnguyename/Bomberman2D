<?php
$Hostname = "localhost";
$DBName = "accounts";
$User = "root";
$PasswordP = "";

mysql_connect($Hostname, $User, $PasswordP) or die ("Can't connect to DB");
mysql_select_db($DBName) or die ("Can't connect to DB");

$Account = $_REQUEST["Account"];
$Password = $_REQUEST["Password"];

if (!$Account || !$Password)
{
	echo "Login or password cant be empty.";
}
else {
	$SQL = "SELECT * FROM accounts WHERE Account= '" . $Account . "'";
	$result_id = @mysql_query($SQL) or die ("DB1 Error");
	$total = mysql_num_rows($result_id);
	if ($total) {
		$datas = @mysql_fetch_array($result_id);
		if(MD5($Password) === $datas["Password"]) {
			echo"Success";
		}
		else{
			echo "WrongPassword";
		}
	}
	else {
		echo "AccountDoesNotExist";
	}
}

mysql_close();
?>