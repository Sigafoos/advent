from hashlib import md5
import sys

i = 0
while md5(sys.argv[1] + str(i)).hexdigest()[:5] != '00000':
	i+= 1

print 'Part 1:' , i

while md5(sys.argv[1] + str(i)).hexdigest()[:6] != '000000':
	i+= 1

print 'Part 2:' , i
