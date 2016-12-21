import itertools

def encode(password, instructions):
	for instruction in instructions:
		password = scramble(password, instruction)
	return password

def scramble(password, instruction):
	instruction = instruction.split()
	if instruction[0] == 'swap':
		if instruction[1] == 'position':
			pos = [int(instruction[2]), int(instruction[5])]
		elif instruction[1] == 'letter':
			pos = [password.index(instruction[2]), password.index(instruction[5])]
		pos = sorted(pos)
		new = password[:pos[0]]
		new += password[pos[1]]
		new += password[(pos[0] + 1):pos[1]]
		new += password[pos[0]]
		new += password[(pos[1] + 1):]
		password = new
	elif instruction[0] == 'reverse':
		start = int(instruction[2])
		end = int(instruction[4])
		if start == 0:
			new = password[end::-1]
		else:
			new = password[:start] + password[end:start - 1:-1]
		new += password[(end + 1):]
		password = new
	elif instruction[0] == 'rotate':
		if instruction[1] == 'based':
			start = password.index(instruction[6])
			rotate = start + 1 if start < 4 else start + 2
			rotate = rotate % len(password)
			password = password[-rotate:] + password[:-rotate]
		else:
			rotate = int(instruction[2]) % len(password)
			if instruction[1] == 'right':
				rotate = -rotate
			password = password[rotate:] + password[:rotate]
	elif instruction[0] == 'move':
		first = int(instruction[2])
		second = int(instruction[5])
		tmp = password[first]
		if first == 0:
			new = password[1:]
		elif first == len(password)-1:
			new = password[:-1]
		else:
			new = password[:first] + password[(first + 1):]
		if second == 0:
			password = tmp + new
		elif second == len(password) - 1:
			password = new + tmp
		else:
			password = new[:second] + tmp + new[second:]

	return password

instructions = []
with open('../input/input21.txt') as fp:
	for line in fp:
		instructions.append(line.strip())

print 'Part 1: %s' % encode('abcdefgh', instructions)

# this sucks, and I accidentally spoiled myself by reading this method on reddit
# I'd like to do it better eventually
password = 'fbgdceah'
for i in itertools.permutations(password):
	i = ''.join(i)
	if encode(i, instructions) == password:
		print 'Part 2: %s' % i
		break
