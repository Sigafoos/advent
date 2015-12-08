import re
import sys

def unescape(line):
	line = re.sub('\A"(.*)"\Z', r'\1', line) # remove quotes
	line = re.sub(r'\\"', '"', line) # remove quote
	line = re.sub(r'\\\\', 'S', line) # remove slash
	line = re.sub(r'\\x[0-9A-Fa-f]{2}', 'H', line) # remove hex
	return line

def compute(filename):
	raw = 0
	escaped = 0
	for line in open(filename, 'r'):
		line = line.strip()
		raw += len(line)
		line = unescape(line)
		escaped += len(line)
	return raw - escaped

print 'sanity check...'
sanity = compute('../example/example8.txt')
if sanity is not 12:
	sys.exit('** sanity check failed with ' + str(sanity) + ' **')
print 'sanity check passed'

print 'part 1:' , compute('../input/input8.txt')
