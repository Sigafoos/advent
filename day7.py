import re
import struct
import sys

wires = {}
instructions = []

fp = open('input7.txt', 'r')
for line in fp:
	matches = re.match('^((?P<input>\w+) )?((?P<op>[A-Z]+) (?P<mod>\w+) )?-> (?P<out>[a-z]+)', line)
	instructions.append(matches)

while instructions:
	line = instructions.pop(0)
	if line.group('mod') is not None:
		second = line.group('mod')
		if line.group('mod') in wires:
			second = wires[line.group('mod')]
		elif second.isdigit():
			 second = int(second)
		else:
			instructions.append(line)
			continue

	if line.group('input') is not None:
		if line.group('input').isdigit():
			inputval = int(line.group('input'))
		elif line.group('input') in wires:
			inputval = wires[line.group('input')]
		else:
			instructions.append(line)
			continue

		if line.group('op') is not None:
			if line.group('op') == 'RSHIFT':
				inputval = inputval >> second
			elif line.group('op') == 'LSHIFT':
				inputval = inputval << second
			elif line.group('op') == 'OR':
				inputval = inputval | second
			elif line.group('op') == 'AND':
				inputval = inputval & second
			else:
				print line.group('op') , 'is not configured'
				print wires
				sys.exit()
	elif line.group('op') == 'NOT':
		inputval = ~second

	wires[line.group('out')] = inputval % 65536

print wires['a']
