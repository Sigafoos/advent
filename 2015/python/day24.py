import itertools
import sys
#sleigh = [(0, []), (1, []), (2, [])]
sleigh = [[0] for x in xrange(3)]
packages = []
for line in open('../input/input24.txt'):
	packages.append(int(line.strip()))
target = sum(packages) / 3
packages = sorted(packages, reverse=True)

#print 'going for' , target
#for package in packages:
#	print [sum(contents) for (section, contents) in sleigh]
	# sort section by weight asc
	# if it fits, put in lowest entry
	# else, uh, this won't work

good = []
for combination in itertools.permutations(packages):
	for package in combination:
		for i in xrange(3):
			if sum(sleigh[i]) + package <= target:
				sleigh[i].append(package)
				break
		else:
			break
	else:
		good.append(combination)

print good
