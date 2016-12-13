import sys

cosmological_constant = int(sys.argv[1])

def is_wall(x, y):
	global cosmological_constant
	return bin((x*x + 3*x + 2*x*y + y + y*y) + cosmological_constant).count('1') % 2 == 1

def generate_map(size):
	grid = []
	for y in xrange(size):
		row = []
		for x in xrange(size):
			cost = 999 if is_wall(x, y) else 1
			row.append(cost)
		grid.append(row)
	return grid

def draw_map(grid, current, visited, closed, goal):
	for y in xrange(len(grid)):
		row = ''
		for x in xrange(len(grid[y])):
			row += 'O' if (y, x) == goal else 'X' if (y, x) == current else 'x' if (y, x) in visited else '.' if grid[x][y] == 1 else '#'
		print row

def distance(start, end):
	return abs(end[0] - start[0]) + abs(end[1] - start[1])

def cost(start, goal, current, grid):
	return distance(start, current) + distance(current, goal) + grid[current[1]][current[0]]

def get_adjacent(current):
	for x in (1, -1):
		yield (current[0] + x, current[1])
		yield (current[0], current[1] + x)

def pathfind(start, goal, grid):
	open_nodes = []
	closed_nodes = []
	visited_nodes = []
	current = start
	while current != goal:
		# check in on Yelp to record our visit
		visited_nodes.append(current)
		closed_nodes.append(current)
		if current in open_nodes:
			open_nodes.remove(current)

		open_nodes += filter(lambda node: 0 <= node[0] < len(grid[0]) and 0 <= node[1] < len(grid) and node not in closed_nodes, get_adjacent(current))

		lowest = []
		lowest_nodes = []
		for node in open_nodes:
			price = cost(start, goal, node, grid)
			if price < lowest:
				lowest = price
				lowest_nodes = [node]
			elif price == lowest:
				lowest_nodes.append(node)

		if lowest_nodes == []:
			print 'ack, nowhere to go'
			draw_map(grid, current, visited_nodes, closed_nodes, goal)
			sys.exit()

		current = lowest_nodes[0]

	if sorted(visited_nodes) != sorted(open_nodes): # we had branches
		print 'flip it, reverse it'
		new_grid = []
		for y in xrange(len(grid)):
			row = []
			for x in xrange(len(grid[y])):
				price = 999 if (x, y) not in visited_nodes else 1
				row.append(price)
			new_grid.append(row)

		draw_map(new_grid, (), (), (), ())
		return pathfind(current, start, new_grid)
	return visited_nodes


grid = generate_map(50)
part1 = pathfind((1, 1), (31, 39), grid)
print 'Part 1: %s' % len(part1)

print draw_map(grid, (31, 39), part1, (), (31, 39))
