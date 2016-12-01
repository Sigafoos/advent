import itertools
import sys

def step(lights):
	new = []
	for (y, x) in itertools.product(xrange(len(lights)), xrange(len(lights[0]))):
		neighbors = 0
		for (neighbor_y, neighbor_x) in itertools.product([y-1, y, y+1], [x-1, x, x+1]):
			if (neighbor_x == x and neighbor_y == y) or neighbor_x < 0 or neighbor_y < 0 or neighbor_x == len(lights[0]) or neighbor_y == len(lights):
				continue
			if lights[neighbor_y][neighbor_x] == 1:
				neighbors += 1

		if len(new) == y:
			new.append([])
		if (lights[y][x] == 1 and neighbors in [2, 3]) or (lights[y][x] == 0 and neighbors == 3):
			new[y].append(1)
		else:
			new[y].append(0)
	return new

def grid(lights):
	for line in lights:
		for light in line:
			if light == 1:
				sys.stdout.write('#')
			else:
				sys.stdout.write('.')
		sys.stdout.write('\n')

def animate(lights, iterations, stuck = False):
	for i in xrange(iterations):
		if stuck:
			for (y, x) in itertools.product([0, len(lights) - 1], [0, len(lights[0]) - 1]):
				lights[y][x] = 1
		lights = step(lights)
	if stuck:
		for (y, x) in itertools.product([0, len(lights) - 1], [0, len(lights[0]) - 1]):
			lights[y][x] = 1

	on = 0
	for (y, x) in itertools.product(xrange(len(lights)), xrange(len(lights[0]))):
		if lights[y][x] == 1:
			on += 1
	return on

lights = []
for line in open('input18.txt'):
	tmp = []
	for char in line.strip():
		if char == '#':
			tmp.append(1)
		else:
			tmp.append(0)
	lights.append(tmp)

print 'Part 1: There are' , animate(lights, int(sys.argv[1])) , 'lights on'
print 'Part 2: There are' , animate(lights, int(sys.argv[1]), True) , 'lights on'
