import re
import sys

def unescape(line):
	print '***'
	print 'Before:' , line , len(line)
	line = re.sub(r'\A"(.*)"\Z', r'\1', line) # remove quotes
	line = re.sub(r'\\x[0-9A-Fa-f]{2}', 'Q', line) # remove hex
	line = re.sub(r'\\"', '"', line) # remove quote
	line = re.sub(r'\\\\', 'Q', line) # remove slash
	print 'After:' , line , len(line)
	return line

raw = 0
escaped = 0
for line in open('input8.txt', 'r'):
	line = line.strip()
	raw += len(line)
	escaped += len(unescape(line.strip()))

print 'Before:' , raw
print 'After:' , escaped
print 'Difference:' , str(raw - escaped)
