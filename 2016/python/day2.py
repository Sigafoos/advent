directions = []
with open('../input/input2.txt') as fp:
	for line in fp: # only one
		directions.append(line.strip())

def get_code(pad, directions):
	x = 1
	y = 1

	code = ''
	for line in directions:
		for char in line:
			# yes, the way the lists are set up x/y are flipped
			if char == 'U' and x > 0:
				newx = x - 1
				x = newx if pad[newx][y] is not None else x
			elif char == 'D' and x + 1 < len(pad):
				newx = x + 1
				x = newx if pad[newx][y] is not None else x
			elif char == 'L' and y > 0:
				newy = y - 1
				y = newy if pad[x][newy] is not None else y
			elif char == 'R' and y + 1 < len(pad[x]):
				newy = y + 1
				y = newy if pad[x][newy] is not None else y

		code += str(pad[x][y])

	return code

pad1 = [
	[1, 2, 3],
	[4, 5, 6],
	[7, 8, 9]
]
print 'Part 1: %s' % get_code(pad1, directions)

pad2 = [
	[None, None, 1, None, None],
	[None, 2, 3, 4, None],
	[5, 6, 7, 8, 9],
	[None, 'A', 'B', 'C', None],
	[None, None, 'D', None, None]
]
print 'Part 2: %s' % get_code(pad2, directions)
