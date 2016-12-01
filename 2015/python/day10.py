import re
import sys

def seeandsay(text):
	matches = re.findall(r'((.)\2*)', text)
	new = ''
	for match in matches:
		new += str(len(match[0])) + match[1]
	return new

text = sys.argv[1]
for i in range(40):
	text = seeandsay(text)
print 'Part 1:' , len(text)

for i in range(10):
	text = seeandsay(text)
print 'Part 2:' , len(text)
