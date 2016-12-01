# I got this pretty much wholesale from
# https://www.reddit.com/r/adventofcode/comments/3x6cyr/day_17_solutions/cy1xoxg/
# but, in the interest of learning, rewrote it manually so I would understand it

combinations = 0
containers = []
goal = 150
for line in open('../input/input17.txt'):
	containers.append(int(line.strip()))

for permutation in xrange (1 << len(containers)): # bitmap of all permutations
	current = permutation
	volume = 0

	for container in containers:
		if current % 2 == 1: # if the current bit is included in this permutation
			volume += container
		current /= 2 # next bit
	if volume == goal:
		combinations += 1

print combinations
