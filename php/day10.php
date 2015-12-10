<?php

function seeandsay($string)
{
	preg_match_all('/(.)\1*/', $string, $matches);
	$string = '';
	foreach ($matches[0] as $match)
	{
		$string .= strlen($match) . substr($match, 0, 1);
	}
	return $string;
}

$text = '1113122113';
for ($i = 0; $i < 40; $i++)
{
	$text = seeandsay($text);
}

echo 'Part 1: ' . strlen($text) . PHP_EOL;

for ($i = 0; $i < 10; $i++)
{
	$text = seeandsay($text);
}

echo 'Part 2: ' . strlen($text) . PHP_EOL;
