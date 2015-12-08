paper = 0
ribbon = 0

for line in open('../input/input2.txt', 'r'):
	dimensions = line.strip().split('x')
	dimensions = [int(side) for side in dimensions]
	dimensions.sort()
	paper += 2 * dimensions[0] * dimensions[1] + 2 * dimensions[1] * dimensions[2] + 2 * dimensions[0] * dimensions[2] + dimensions[0] * dimensions[1]
	ribbon += 2 * (dimensions[0] + dimensions[1]) + dimensions[0] * dimensions[1] * dimensions[2]

print paper , 'square feet of paper'
print ribbon , 'feet of ribbon'
