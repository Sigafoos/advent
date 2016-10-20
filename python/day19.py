import collections
import re
import sys

molecules = {}
medicine = ''
for line in open('../input/input19.txt'):
	matches = re.match('(?P<molecule>\w{1,2}) => (?P<replacement>\w+)', line)
	if matches is not None:
		if matches.group('molecule') not in molecules:
			molecules[matches.group('molecule')] = set() # set([matches.group('molecule')])
		molecules[matches.group('molecule')].add(matches.group('replacement'))
	else:
		matches = re.match('(\w+)', line)
		if matches is not None:
			medicine = matches.group(1)
			break

def step(medicine, molecules):
	options = set()
	for match in re.finditer('(' + '|'.join(molecules.keys()) + ')', medicine):
		atom = match.group(1)
		span = match.span()
		for replacement in molecules[atom]:
			options.add(medicine[:span[0]] + replacement + medicine[span[1]:])
	return options

def generate(current, goal, molecules, i = 0):
	if current != goal:
		i += 1
		options = step(current, molecules)
		if options == set():
			return False
		options = sorted(options, key=len)
		j = 0
		best = generate(options[j], goal, molecules, i)
		while best is False and j < len(options) - 1:
			j += 1
			best = generate(options[j], goal, molecules, i)
		return best
	else:
		return i


#options = step(medicine, molecules)
#print 'Part 1:' , len(options)

flipped = {}
for (key, values) in molecules.iteritems():
	for value in values:
		if value not in flipped:
			flipped[value] = []
		flipped[value].append(key)

steps = generate(medicine, 'e', flipped)
print 'Part 2:' , steps
