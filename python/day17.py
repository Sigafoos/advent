import itertools
import sys

containers = []
for line in open('../input/input17.txt'):
	containers.append(int(line.strip()))

containers = sorted(containers)
for i in xrange(len(containers)):
	if sum(containers[:i + 1]) >= 150:
		break
combinations = itertools.permutations([key for key in xrange(len(containers))], i)

valid = set()
for combination in combinations:
	for i in xrange(len(combination)):
		permutation = [containers[x] for x in combination[:i + 1]]
		total = sum(permutation)
		if total > 150:
			break
		elif total == 150:
			valid.add(tuple(sorted(combination[:i + 1])))
			break
print len(valid)
