lcd = [ [ 0 for x in xrange(50) ] for i in xrange(6) ]

def rect(lcd, width, height):
	for y in xrange(height):
		for x in xrange(width):
			lcd[y][x] = 1
	return lcd

def row(lcd, row, amount):
	tmp = lcd[row][-amount:] + lcd[row][:len(lcd[row])-amount]
	lcd[row] = tmp
	return lcd

def column(lcd, col, amount):
	tmp = map(lambda x: x[col], lcd)
	tmp = tmp[-amount:] + tmp[:len(tmp)-amount]
	for i in xrange(len(lcd)):
		lcd[i][col] = tmp[i]
	return lcd

def visual(lcd):
	for y in lcd:
		for x in y:
			pixel = '.' if x == 0 else '#'
			print pixel,
		print ''

with open('../input/input8.txt') as fp:
	for line in fp:
		chunks = line.split()
		if chunks[0] == 'rect':
			dimensions = chunks[1].split('x')
			lcd = rect(lcd, int(dimensions[0]), int(dimensions[1]))
		elif chunks[0] == 'rotate' and chunks[1] == 'row':
			lcd = row(lcd, int(chunks[2][2:]), int(chunks[4]))
		elif chunks[0] == 'rotate' and chunks[1] == 'column':
			lcd = column(lcd, int(chunks[2][2:]), int(chunks[4]))

print 'Part 1: %s' % sum(map(len, map(lambda row: filter(lambda x: x == 1, row), lcd)))
print "\nPart 2:"
visual(lcd)
