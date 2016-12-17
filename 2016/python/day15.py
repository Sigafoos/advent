discs = []
with open('../input/input15.txt') as fp:
	for line in fp:
		parsed = line.split()
		discs.append({
			'order': int(parsed[1][1:]),
			'mod': int(parsed[3]),
			'current': int(parsed[11][:-1])
			})

def pachinko(discs):
	times = [x for x in xrange(1, 10000000)]
	for disc in sorted(discs, key=lambda x: x['mod'], reverse=True):
		times = filter(lambda x: (x + disc['order'] + disc['current']) % disc['mod'] == 0, times)
	return min(times)

print 'Part 1: %s' % pachinko(discs)
discs.append({
	'order': len(discs) + 1,
	'mod': 11,
	'current': 0
	})
print 'Part 2: %s' % pachinko(discs)
