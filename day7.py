import re
import struct
import sys

def resolve(instructions):
	wires = {}
	while instructions:
		line = instructions.pop(0)
		if line['mod'] is not None:
			second = line['mod']
			if line['mod'] in wires:
				second = wires[line['mod']]
			elif second.isdigit():
				 second = int(second)
			else:
				instructions.append(line)
				continue

		if line['input'] is not None:
			if line['input'].isdigit():
				inputval = int(line['input'])
			elif line['input'] in wires:
				inputval = wires[line['input']]
			else:
				instructions.append(line)
				continue

			if line['op'] is not None:
				if line['op'] == 'RSHIFT':
					inputval = inputval >> second
				elif line['op'] == 'LSHIFT':
					inputval = inputval << second
				elif line['op'] == 'OR':
					inputval = inputval | second
				elif line['op'] == 'AND':
					inputval = inputval & second
				else:
					print line['op'] , 'is not configured'
					print wires
					sys.exit()
		elif line['op'] == 'NOT':
			inputval = ~second

		wires[line['out']] = inputval % 65536
	return wires

def this_sucks(line): # tell me how you really feel
	global a
	if line['out'] == 'b':
		line['input'] = str(a)
		line['op'] = None
		line['mod'] = None
	return line

instructions = []

fp = open('input7.txt', 'r')
for line in fp:
	matches = re.match('^((?P<input>\w+) )?((?P<op>[A-Z]+) (?P<mod>\w+) )?-> (?P<out>[a-z]+)', line)
	instructions.append({
		'input': matches.group('input'),
		'op': matches.group('op'),
		'mod': matches.group('mod'),
		'out': matches.group('out')
		})

second = instructions
wires = resolve(instructions[:])
print wires['a']
a = wires['a']
wires = resolve(map(this_sucks, instructions))
print wires['a']
