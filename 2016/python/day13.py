import sys

def is_wall(x, y):
	return bin((x*x + 3*x + 2*x*y + y + y*y) + int(sys.argv[1])).count('1') % 2 == 1

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
			row += '\033[41mO\033[0m' if (y, x) == goal else '\033[35mX\033[0m' if (y, x) == current else '\033[32mx\033[0m' if (y, x) in visited else '.' if grid[x][y] == 1 else '\033[34m#\033[0m' if grid[x][y] not in closed else '\033[31m=\033[0m'
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
	trace = {}
	while current != goal:
		# check in on Yelp to record our visit
		visited_nodes.append(current)
		closed_nodes.append(current)
		if current in open_nodes:
			open_nodes.remove(current)

		open_nodes += filter(lambda node: 0 <= node[0] < len(grid[0]) and 0 <= node[1] < len(grid) and node not in closed_nodes and node not in open_nodes, get_adjacent(current))

		for node in get_adjacent(current):
			if str(node) not in trace:
				trace[str(node)] = current

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
			# didn't come up in the challenge, but if you extended...
			print 'ack, nowhere to go'
			draw_map(grid, current, visited_nodes, closed_nodes, goal)
			sys.exit()

		current = lowest_nodes[0]

	steps = 0
	while current != start:
		current = trace[str(current)]
		steps += 1
	return steps

def meander(grid, limit):
	visited = set()
	to_search = []
	current = (1, 1)
	distances = {str(current): 0}
	while True:
		visited.add(current)
		neighbors = filter(lambda node: 0 <= node[0] < len(grid[0]) and 0 <= node[1] < len(grid) and node not in visited and node not in to_search and grid[node[1]][node[0]] == 1, get_adjacent(current))
		distance = distances[str(current)]
		for neighbor in neighbors:
			if distance + 1 <= limit:
				to_search.append(neighbor)
				if str(neighbor) in distances and distance + 1 < distances[str(neighbor)]:
					print 'WE SHOULD REDO THINGS'
					# this wasn't an issue in the challenge but it could be
					sys.exit()
					distances[str(neighbor)] = distance + 1
				elif str(neighbor) not in distances:
					distances[str(neighbor)] = distance + 1
		if len(to_search) == 0:
			return visited
		current = to_search.pop(0)

grid = generate_map(50)
print 'Part 1: %s' % pathfind((1, 1), (31, 39), grid)
print 'Part 2: %s' % len(meander(generate_map(30), 50))
