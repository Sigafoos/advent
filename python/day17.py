import itertools

containers = []
for line in open('../input/input17.txt'):
	containers.append(int(line.strip()))

valid = 0
for combination in itertools.permutations(containers):
	total = 0
	for item in combination:
		total += item
		if total > 150:
			break
		elif total == 150:
			valid += 1
			break
print valid
