<?php
function add($input)
{
	if (!is_array($input))
	{
		return $input;
	}
	$total = 0;
	foreach ($input as $key => $item)
	{
		if (is_array($item))
		{
			$total += add($item);
		}
		else
		{
			$total += $item;
		}
	}
	return $total;
}

$json = json_decode(file_get_contents('../input/input12.txt'), true);
echo add($json) . PHP_EOL;
