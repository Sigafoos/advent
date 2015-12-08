<?php
$fp = fopen('input8.txt', 'r');
while ($line = fgets($fp))
{
	$string = `php -r 'echo $string;'`;
	echo $string;
}
fclose($fp);
?>
