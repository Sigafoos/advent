import re
import sys

register = {'a': 0, 'b': 0}
i = 0
instructions = []
for line in open('../input/input23.txt'):
	instructions.append(line.strip())

if len(sys.argv) > 1 and sys.argv[1] == '-2':
		register['a'] = 1

while 1 == 1:
	try:
		matches = re.match('(\w+) (a|b)?(?:, )?([-\+]\d+)?', instructions[i])
	except IndexError:
		break
	if matches.group(1) == 'hlf':
		register[matches.group(2)] /= 2
	elif matches.group(1) == 'tpl':
		register[matches.group(2)] *= 3
	elif matches.group(1) == 'inc':
		register[matches.group(2)] += 1
	elif matches.group(1) == 'jmp':
		i += int(matches.group(3))
		continue
	elif matches.group(1) == 'jie':
		if register[matches.group(2)] % 2 == 0:
			i += int(matches.group(3))
			continue
	elif matches.group(1) == 'jio':
		if register[matches.group(2)] == 1:
			i += int(matches.group(3))
			continue
	i += 1

print register['b']
