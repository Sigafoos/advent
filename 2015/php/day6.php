<?php
$day6 = new Day6();
$fp = fopen('../input/input6.txt', 'r');
while ($line = trim(fgets($fp)))
{
	$day6->parse($line);
}
echo $day6->count() . PHP_EOL;

class Day6
{
	private $_lights = array();

	/**
	 * Set up the array
	 */
	public function __construct()
	{
		for ($i = 0; $i < 1000; $i++)
		{
			for ($j = 0; $j < 1000; $j++)
			{
				$this->_lights[$i][$j] = 0;
			}
		}
		self::_debug('initialized');
	}

	/**
	 * Switch a light on
	 *
	 * If it is already on, nothing will change.
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _turnOn($x, $y)
	{
		$this->_lights[$x][$y] = 1;
		self::_debug('light ' . $x . ',' . $y . ' turned on');
	}

	/**
	 * Switch a light off.
	 *
	 * If it is already off, nothing will change.
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _turnOff($x, $y)
	{
		$this->_lights[$x][$y] = 0;
		self::_debug('light ' . $x . ',' . $y . ' turned off');
	}

	/**
	 * Flip a light's state
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _toggle($x, $y)
	{
		($this->_lights[$x][$y] === 1) ? self::_turnOff($x, $y) : self::_turnOn($x, $y);
	}

	/**
	 * Make a light 1 unit brighter
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _brighten($x, $y)
	{
		$this->_lights[$x][$y]++;
	}

	/**
	 * Make a light 2 units brighter
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _superBrighten($x, $y)
	{
		$this->_lights[$x][$y] += 2;
	}

	/**
	 * Make a light 1 unit dimmer, to a minimum of 0
	 *
	 * @param int $x The x position of the light
	 * @param int $y The y position of the light
	 */
	private function _darken($x, $y)
	{
		($this->_lights[$x][$y] > 0) ? $this->_lights[$x][$y]-- : $this->_lights[$x][$y] = 0;
	}

	/**
	 * Take a line from the day 6 input and do Santa's bidding
	 *
	 * @param string $line The line of input
	 */
	public function parse($line)
	{
		preg_match('/([a-z ]+) ([0-9]+),([0-9]+) through ([0-9]+),([0-9]+)/', $line, $matches);
		//self::_debug(var_export($matches, true));
		for ($x = $matches[2]; $x <= $matches[4]; $x++)
		{
			for ($y = $matches[3]; $y <= $matches[5]; $y++)
			{
				switch ($matches[1])
				{
					case 'turn on':
						//self::_turnOn($x, $y);
						self::_brighten($x, $y);
						break;
					case 'turn off':
						//self::_turnOff($x, $y);
						self::_darken($x, $y);
						break;
					case 'toggle':
						//self::_toggle($x, $y);
						self::_superBrighten($x, $y);
						break;
					default:
						throw new Exception($matches[1] . ' is not a known command' . PHP_EOL);
				}
			}
		}
	}

	/**
	 * Return a count of how many lights are on
	 *
	 * @return int The number of lights that have been turned on
	 */
	public function count()
	{
		$count = 0;

		for ($x = 0; $x < 1000; $x++)
		{
			for ($y = 0; $y < 1000; $y++)
			{
				$count += $this->_lights[$x][$y];
			}
		}
		return $count;
	}

	/**
	 * Print a debug message
	 *
	 * string $message What to print
	 */
	private function _debug($message)
	{
		global $argv;
		if (!empty($argv[1]) && $argv[1] == '-d')
		{
			echo $message . PHP_EOL;
		}
	}
}
