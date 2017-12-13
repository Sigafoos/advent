import re

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

def branches(current, nodes):
	children = []
	if 'children' in nodes[current]:
		for child in nodes[current]['children']:
			children.append(branches(child, nodes))
	return {current: children}

#def weight(current, nodes):
	
		

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

tree = branches(trunk, nodes)
print tree
