import hashlib
import sys

door = sys.argv[1]

passwords = ['', {}]
valid = [x for x in xrange(8)]
for i in xrange(sys.maxint):
	md5 = hashlib.md5()
	md5.update(door + str(i))
	hashed = md5.hexdigest()
	if hashed[:5] == '00000':
		if len(passwords[0]) < 8:
			passwords[0] += hashed[5]

		try:
			key = int(hashed[5])
			if len(passwords[1]) < 8 and key in valid and key not in passwords[1]:
				passwords[1][key] = hashed[6]
		except ValueError: # not an int
			pass
	if len(passwords[0]) == 8 and len(passwords[1]) == 8:
		break

print 'Part 1: %s' % passwords[0]
print 'Part 2: %s' % ''.join(map(lambda x: passwords[1][x], valid))
