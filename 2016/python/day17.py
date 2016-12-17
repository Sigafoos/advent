import hashlib
import sys

def draw(grid, current):
	print '#' * 9
	for y in xrange(len(grid)):
		print '#' + '|'.join(map(lambda x: 'X' if (x, y) == current else ' ', grid[y])) + '#'
		if y != len(grid) -1:
			print '#-' * 4 + '#'
	print '#' * 9

def valid_move(current, door):
	if current[1] == 0 and door == 0:
		return False
	elif current[1] == 3 and door == 1:
		return False
	elif current[0] == 0 and door == 2:
		return False
	elif current[0] == 3 and door == 3:
		return False
	return True


lookup = {0: 'U', 1: 'D', 2: 'L', 3: 'R'}
grid = [[x for x in xrange(4)] for y in xrange(4)]
current = ((0, 0), sys.argv[1])
nodes = [current]

paths = []
while True:
	to_append = []
	if len(nodes) == 0:
		break
	for current in nodes:

		md5 = hashlib.md5(current[1])
		hashed = md5.hexdigest()
		for i in xrange(4):
			if hashed[i] in 'bcdef' and valid_move(current[0], i) is True:
				if lookup[i] == 'U':
					new = (current[0][0], current[0][1] - 1)
				elif lookup[i] == 'D':
					new = (current[0][0], current[0][1] + 1)
				elif lookup[i] == 'L':
					new = (current[0][0] - 1, current[0][1])
				elif lookup[i] == 'R':
					new = (current[0][0] + 1, current[0][1])

				new_hashed = current[1] + lookup[i]
				if new == (3, 3):
					paths.append(new_hashed[len(sys.argv[1]):])
				else:
					to_append.append((new, new_hashed))
	nodes = to_append

print 'Part 1: %s' % min(paths, key=len)
print 'Part 2: %s' % len(max(paths, key=len))
