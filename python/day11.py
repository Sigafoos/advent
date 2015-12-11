import re
import sys

def increment(text):
	text = list(text)
	i = -1
	while text[i] == 'z':
		text[i] = 'a'
		i -= 1
	text[i] = chr(ord(text[i]) + 1)
	return ''.join(text)

def nice(text):
	if re.search('[iol]', text) is not None:
		return False
	if re.search(r'(.)\1.*([^\1])\2', text) is None:
		return False
	straight = 0
	mapped = map(ord, text)
	for i, letter in enumerate(mapped):
		if i + 1 == len(mapped):
			return False
		if mapped[i + 1] - letter == 1:
			straight += 1
			if straight == 2:
				return True
		else:
			straight = 0

password = sys.argv[1]
while not nice(password):
	password = increment(password)
print "Santa's next password is" , password
password = increment(password)
while not nice(password):
	password = increment(password)
print "Santa's next password is" , password
