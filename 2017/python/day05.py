from copy import deepcopy
instructions = []

with open('../inputs/05.txt') as fp:
	for line in fp:
		instructions.append(int(line.strip()))

def part1(instructions):
	steps = 0
	i = 0
	while i < len(instructions):
		val = instructions[i]
		instructions[i] += 1
		i += val
		steps += 1
	return steps
	
def part2(instructions):
	steps = 0
	i = 0
	while i < len(instructions):
		val = instructions[i]
		if val < 3:
			instructions[i] += 1
		else:
			instructions[i] -= 1
		i += val
		steps += 1
	return steps

print 'Part 1: %s' % part1(deepcopy(instructions))
print 'Part 2: %s' % part2(deepcopy(instructions))
