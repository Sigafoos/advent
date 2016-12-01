import re
import sys

sues = {}
for line in open('../input/input16.txt'):
	number = re.match(r'Sue (\d+)', line).group(1)
	things = re.findall(r'(\w+): (\d+),?', line)
	sues[number] = {}
	for thing in things:
		sues[number][thing[0]] = thing[1]

giver = {
	'children': '3',
	'cats': '7',
	'samoyeds': '2',
	'pomeranians': '3',
	'akitas': '0',
	'vizslas': '0',
	'goldfish': '5',
	'trees': '3',
	'cars': '2',
	'perfumes': '1'
}

for (number, things) in sues.iteritems():
	for (thing, count) in things.iteritems():
		if giver[thing] != count:
			break
	else:
		print 'Part 1:' , number
		break

for (number, things) in sues.iteritems():
	for (thing, count) in things.iteritems():
		if thing in ['cats', 'trees']:
			if giver[thing] >= count:
				break
		elif thing in ['pomeranians', 'goldfish']:
			if giver[thing] <= count:
				break
		elif giver[thing] != count:
			break
	else:
		print 'Part 2:' , number
		break
