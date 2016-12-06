import string
from copy import deepcopy

letters = dict(zip(string.ascii_lowercase, [0]*26))
message = [deepcopy(letters) for s in xrange(8)]

with open('../input/input6.txt') as fp:
	for line in fp:
		for i in xrange(len(line.strip())):
			message[i][line[i]] += 1

print 'Part 1: %s' % ''.join(map(lambda x: max(x, key=x.get), message))
print 'Part 2: %s' % ''.join(map(lambda x: min(x, key=x.get), message))
