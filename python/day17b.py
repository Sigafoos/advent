# this one I did myself, though obviously using the previous method

goal = 150
containers = []
for line in open('../input/input17.txt'):
	containers.append(int(line.strip()))

containers = sorted(containers)

# find the min number of containers
for i in xrange(len(containers)):
	if sum(containers[-(i + 1):]) >= 150:
		break
minimum = i + 1 # beware the off-by-1 dragon

valid = 0
for i in xrange(1 << len(containers)):
	count = 0
	volume = 0
	current = i

	for container in containers:
		if current % 2 == 1:
			volume += container
			count += 1
		current /= 2

	if volume == goal and count == minimum:
		valid += 1

print valid
