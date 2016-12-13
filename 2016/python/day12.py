reg = {'a': 0, 'b': 0, 'c': 0, 'd': 0}
commands = []
with open('../input/input12.txt') as fp:
	for line in fp:
		commands.append(line.strip().split())

reg['c'] = 1 # remove for part 2
import sys
i = 0
while i < len(commands):
	command = commands[i]
	if command[0] == 'cpy' and command[1].isdigit():
		reg[command[2]] = int(command[1])
	elif command[0] == 'cpy':
		reg[command[2]] = reg[command[1]]
	elif command[0] == 'inc':
		reg[command[1]] += 1
	elif command[0] == 'dec':
		reg[command[1]] -= 1
	elif command[0] == 'jnz':
		val = reg[command[1]] if command[1] in reg else int(command[1])
		if val != 0:
			i += int(command[2])
			continue

	i += 1

print reg['a']
