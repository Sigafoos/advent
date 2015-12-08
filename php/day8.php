<?php
$fp = fopen('../input/input8.txt', 'r');
$raw = 0;
$parsed = 0;
while ($line = trim(fgets($fp)))
{
	$raw += strlen($line);
	eval('$tmp = ' . $line . ';');
	$parsed += strlen($tmp);
}
echo 'Part 1: ' . ($raw - $parsed) . PHP_EOL;

$raw = 0;
$parsed = 0;
rewind($fp);
while ($line = trim(fgets($fp)))
{
	$raw += strlen($line);
	$parsed += strlen(addcslashes($line, '"\\')) + 2; // you encode the quotes, then add them again?
}
echo 'Part 2: ' . ($parsed - $raw) . PHP_EOL;

fclose($fp);
?>
