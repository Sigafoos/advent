<?php
$houses = 1;
$xpos = array(
	'santa' => 0,
	'robo' => 0,
);
$ypos = array(
	'santa' => 0,
	'robo' => 0,
);
$grid = array(
	0 => array(
		0 => true,
	),
);
$i = 0;

$fp = fopen('../input/input3.txt', 'r');

$directions = str_split(trim(fgets($fp)));
foreach ($directions as $direction)
{
	$key = ($i % 2 == 0) ? 'santa' : 'robo';
	switch ($direction)
	{
	case '^':
		$ypos[$key]++;
		break;
	case 'v':
		$ypos[$key]--;
		break;
	case '<':
		$xpos[$key]--;
		break;
	case '>':
		$xpos[$key]++;
		break;
	default:
		die("I don't know how to handle " . $direction);
	}

	if (empty($grid[$xpos[$key]][$ypos[$key]]))
	{
		$grid[$xpos[$key]][$ypos[$key]] = true;
		$houses++;
	}
	$i++;
}
echo 'Santa visited ' . $houses . ' distinct houses' . PHP_EOL;
?>
