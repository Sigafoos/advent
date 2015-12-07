import re
import sys

wires = {}
fp = open('input7.txt', 'r')
for line in fp:
	line = 'x -> y'
	matches = re.match('^([\w]+) (([A-Z]+) ([\w]+))? ?-> ([a-z]+)', line)
	# 0: x RSHIFT 2 -> y
	# 1: x
	# 2: RSHIFT 2 / None
	# 3: RSHIFT / None
	# 4: 2 / None
	# 5: y
	if matches.group(1).isdigit():
		inputval = matches.group(1)
	elif matches.group(1) in wires:
		inputval = wires[matches.group(1)]
	else:
		inputval = 0
	if (matches.group(2) is not None):
		if (matches.group(3) is 'RSHIFT'):
			inputval = inputval >> matches.group(4)
		else:
			print matches.group(3) + 'is not configured'
			print wires
			sys.exit()

	wires[matches.group(5)] = inputval
sys.exit()
