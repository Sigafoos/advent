from copy import deepcopy

def walk(part = 1):
	position = [0, 0]
	facing = 'N'
	visited = []

	with open('../input/input1.txt') as fp:
		for line in fp: # only one
			directions = line.split(', ')

	for direction in directions:
		turn = direction[0]
		steps = int(direction[1:])
		if (facing == 'N' and turn == 'R') or (facing == 'S' and turn == 'L'):
			for i in xrange(steps):
				position[0] += 1
				if position not in visited:
					visited.append(deepcopy(position))
				elif part == 2:
					return position
			facing = 'E'
		elif (facing == 'N' and turn == 'L') or (facing == 'S' and turn == 'R'):
			for i in xrange(steps):
				position[0] -= 1
				if position not in visited:
					visited.append(deepcopy(position))
				elif part == 2:
					return position
			facing = 'W'
		elif (facing == 'E' and turn == 'R') or (facing == 'W' and turn == 'L'):
			for i in xrange(steps):
				position[1] -= 1
				if position not in visited:
					visited.append(deepcopy(position))
				elif part == 2:
					return position
			facing = 'S'
		elif (facing == 'E' and turn == 'L') or (facing == 'W' and turn == 'R'):
			for i in xrange(steps):
				position[1] += 1
				if position not in visited:
					visited.append(deepcopy(position))
				elif part == 2:
					return position
			facing = 'N'

	return position

print [sum(map(abs, walk(x))) for x in [1,2]]
