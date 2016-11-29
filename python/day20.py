# yet again I got some algorithm help from the subreddit
import sys

def part1(target):
	houses = {}
	for i in xrange(1, target / 10): # each elf
		for j in xrange(i, target / 10, i): # each house the elf visits
			if j not in houses:
				houses[j] = 0
			houses[j] += i * 10

	for (number, presents) in houses.iteritems():
		if presents >= target:
			return number

def part2(target):
	houses = {}
	for i in xrange(1, target / 11): # each elf
		for j in xrange(i, i * 50, i): # each house the elf visits
			if j > target / 11: # too big! abort!
				continue
			if j not in houses:
				houses[j] = 0
			houses[j] += i * 11

	for (number, presents) in houses.iteritems():
		if presents >= target:
			return number


target = int(sys.argv[1])
one = part1(target)
print 'Part 1:' , one

two = part2(target)
print 'Part 2:' , two
