<?php
$fp = fopen('day5.txt', 'r');
$nice = 0;
while ($line = trim(fgets($fp)))
{
	if (nice($line))
	{
		$nice++;
	}
}
echo 'There are ' . $nice . ' nice lines' . PHP_EOL;

function nice($line)
{
	global $argv; // don't you judge me, this is quick and dirty
	preg_match_all('/(.)([^\1]).*\1\2/', $line, $matches);
	if (count($matches[0]) === 0)
	{
		if (!empty($argv[1]) && $argv[1] == '-d')
		{
			echo $line . ' fails part 1' . PHP_EOL;
		}
		return false;
	}

	unset($matches);
	preg_match_all('/(.).\1/', $line, $matches);
	if (count($matches[0]) === 0)
	{
		if (!empty($argv[1]) && $argv[1] == '-d')
		{
			echo $line . ' fails part 2' . PHP_EOL;
		}
		return false;
	}

	return true;
}
?>
