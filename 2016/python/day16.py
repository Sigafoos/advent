import sys

def dragon(a, length):
	length = int(length)
	while len(a) < length:
		b = ''.join('0' if x == '1' else '1' for x in a[::-1])
		a += '0' +  b
	return a[:length]

def checksum(dragon):
	check = ''
	for i in xrange(0, len(dragon), 2):
		check += '1' if dragon[i] == dragon[i+1] else '0'
	if len(check) % 2 == 0:
		return checksum(check)
	else:
		return check


d = dragon(sys.argv[1], sys.argv[2])
print 'checksum: %s' % checksum(d)
