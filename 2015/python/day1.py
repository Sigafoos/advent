floor = 0
basement = 0
i = 1
for line in open('../input/input1.txt', 'r'):
	for character in line:
		if (character == '('):
			floor += 1
		else:
			floor -= 1
			if basement is 0 and floor < 0:
				basement = i
		i += 1
print 'Santa will end on floor' , floor
print 'Santa first entered the basement on instruction' , basement
