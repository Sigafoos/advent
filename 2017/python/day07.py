from collections import Counter
import re
import sys

# super inefficient! yay!
def root(nodes):
	for node in nodes.iterkeys():
		found = False
		for (root, data) in nodes.items():
			if 'children' in data and node in data['children']:
				found = True
				continue
		if found:
			continue
		else:
			return node

# wound up not using this but hey
def branches(current, nodes):
	children = []
	if 'children' in nodes[current]:
		for child in nodes[current]['children']:
			children.append(branches(child, nodes))
	return {current: children}

def imbalance(current, nodes):
	w = nodes[current]['weight']
	children = {}
	weight = []
	if 'children' in nodes[current]:
		for child in nodes[current]['children']:
			children[child] = imbalance(child, nodes)
		weight = children.values()
		# the first time this happens is the highest up the chain, so it's what we want
		if min(weight) != max(weight):
			counted = Counter(weight)
			standard = max(counted, key=counted.get) # what all the cool kids have
			outlier = min(counted, key=counted.get) # the value of the unbalanced
			perpetrator = children.keys()[children.values().index(outlier)]
			corrected = nodes[perpetrator]['weight'] + (standard - outlier)
			print 'Part 2: %s' % corrected
			sys.exit() # ew
	return w + sum(weight)

nodes = {}
p = re.compile(r'(?P<node>[a-z]+) \((?P<weight>\d+)\)(?: -> (?P<children>[a-z, ]+))?')

with open('../inputs/07.txt') as fp:
	for line in fp:
		match = p.match(line.strip())
		nodes[match.group('node')] = {'weight': int(match.group('weight'))}

		if match.group('children') is not None:
			nodes[match.group('node')]['children'] = match.group('children').split(', ')

trunk = root(nodes)
print 'Part 1: %s' % trunk

print 'Part 2: %s' % imbalance(trunk, nodes)
