import itertools
import sys
from operator import mul

def day24(compartments):
	packages = []
	for line in open('../input/input24.txt'):
		packages.append(int(line.strip()))
	packages.sort(reverse=True)

	target = sum(packages) / compartments

	smallest = []
	for i in xrange(len(packages) - compartments - 1):
		for combination in itertools.combinations(packages, i):
			if sum(combination) == target:
				smallest.append(reduce(mul, combination))

		if len(smallest):
			return min(smallest)

# from the reddit megathread this isn't necessary, but I wouldn't know that
# if I hadn't needed help
def unused():
	valid = []
	for combination in smallest:
		print 'starting combination'
		remaining = filter(lambda x: x not in combination, packages) # get what's left
		for permutation in itertools.permutations(remaining):
			print 'permutation:'
			print permutation
			sleigh = [0, 0]
			current = 0
			quantum = 0
			for package in permutation:
				if sleigh[current] == target:
					if current == len(sleigh) - 1: # everything fits
						print 'adding'
						quantum = reduce(lambda x, y: x*y, combination)
						break
					else:
						print 'current is now %s' % current
						current += 1
				elif sleigh[current] > target: # overloaded
					print 'sleigh %s is overloaded: NEXT!' % current
					break
				sleigh[current] += package

			if quantum > 0:
				print 'on to the next'
				valid.append(quantum) # we know this fits perfectly, move on to the next
				break
			print 'next permutation'

print day24(3)
print day24(4)
