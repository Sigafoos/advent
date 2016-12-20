blocked = []
with open('../input/input20.txt') as fp:
	for line in fp:
		blocked.append(map(int, line.strip().split('-')))

blocked = sorted(blocked, key=lambda x: x[0])

ips = []
trying = 0
while len(blocked) > 0:
	current = blocked.pop(0)
	while trying < current[0]:
		ips.append(trying)
		trying += 1
	if current[1] > trying:
		trying = current[1] + 1

print 'Part 1: %s' % ips[0]
print 'Part 2: %s' % len(ips)
