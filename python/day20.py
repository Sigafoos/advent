import sys

def divisors(x, two = False):
	if x < 50 or not two:
		start = 1
	else:
		start = x - 50
	for i in xrange(1, x/2 + 1): # got this from SO (yeah, I know)
		if x % i == 0:
			yield i
	yield x

def deliver(house, two = False):
	houses = divisors(house, two)
	if not two:
		multiplier = 10
	else:
		multiplier = 11
	return sum(houses) * multiplier

def part1(target):
	i = 1
	actual = deliver(i)
	# this takes basically forever on my server but it worked
	while actual < target:
		i += 1
		if sum([x*10 for x in xrange(i + 1)]) < target:
			continue # if every elf isn't enough, don't bother
		actual = deliver(i)
	return i

def part2(target):
	i = 1
	actual = deliver(i, True)
	# this takes basically forever on my server but it worked
	while actual < target:
		i += 1
		if sum([x*11 for x in xrange(1, ((i + 1) / 2) + 1)]) < target:
			continue # if every elf isn't enough, don't bother
		actual = deliver(i, True)
	return i

target = int(sys.argv[1])
#one = part1(target)
#print 'Part 1:' , one

two = part2(target)
print 'Part 2:' , two
