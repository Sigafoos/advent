fp = open('../input/input9.txt')
raw = fp.read().strip()

def part1(raw):
	parsed = ''
	while len(raw):
		marker = raw.find('(')

		if marker == -1: # all done with markers
			parsed += raw
			break
		elif marker != 0:
			parsed += raw[0:marker] # things from the last chomp til the next marker
			raw = raw[marker:] # remove that

		split = raw.split(')', 1)
		repeat = map(int, split[0][1:].split('x')) # '(8x1' becomes (8, 1)
		parsed += split[1][:repeat[0]] * repeat[1]

		raw = split[1][repeat[0]:]
	return parsed

def part2(raw):
	chunked = []
	while len(raw):
		marker = raw.find('(')

		if marker == -1: # all done with markers
			chunked.append(len(raw))
			break
		elif marker != 0:
			chunked.append(marker)
			raw = raw[marker:] # remove that

		split = raw.split(')', 1)
		repeat = map(int, split[0][1:].split('x')) # '(8x1' becomes (8, 1)

		substr = split[1][:repeat[0]]
		if substr.find('(') != -1: # still markers in this section
			chunked.append(repeat[1] * part2(substr))
		else:
			chunked.append(reduce(lambda x, y: x*y, repeat))

		raw = split[1][repeat[0]:]
	return sum(chunked)

print 'Part 1: %s' % len(part1(raw))
print 'Part 2: %s' % part2(raw)
