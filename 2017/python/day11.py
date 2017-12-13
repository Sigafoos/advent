with open('../inputs/11.txt') as fp:
	instructions = fp.readline().strip().split(',')

# http://keekerdc.com/2011/03/hexagon-grids-coordinate-systems-and-distance-calculations/
# I searched "hex grid algorithm", yeah
pos = [0, 0]
farthest = -999

for d in instructions:
	if d == 'nw':
		pos[0] -= 1
		#pos[2] -= 1
	elif d == 'n':
		pos[0] -= 1
		pos[1] += 1
	elif d == 'ne':
		pos[1] += 1
		#pos[2] -= 1
	elif d == 'se':
		pos[0] += 1
		#pos[2] -= 1
	elif d == 's':
		pos[0] += 1
		pos[1] -= 1
	elif d == 'sw':
		pos[1] -= 1
		#pos[2] += 1
	else:
		raise Exception('what the h*ck is %s' % d)
	distance = max(map(abs, pos))
	if distance > farthest:
		farthest = distance

print 'Part 1: %s' % max(map(abs, pos))
print 'Part 2: %s' % farthest
