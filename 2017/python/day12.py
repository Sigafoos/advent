import re

p = re.compile(r'(\d+) <-> ([\d, ]+)')
mapped = {}

with open('../inputs/12.txt') as fp:
	for line in fp:
		matches = p.match(line)
		mapped[matches.group(1)] = matches.group(2).split(', ')

known = []
groups = {} # key should be the lowest in the group
for k in map(str, sorted(map(int, mapped.iterkeys()))):
	if k in known:
		continue
	
	options = mapped[k]
	local = [k] + mapped[k]
	
	while options:
		current = options.pop(0)
		for child in mapped[current]:
			if child not in local+known:
				local.append(child)
				options.append(child)
	known += local
	if k != str(min(map(int, local))):
		raise ValueError('wha')
	groups[k] = local

print 'Part 1: %s' % len(groups['0'])
print 'Part 2: %s' % len(groups)
# 243 is too high
