from copy import deepcopy
from dancer import dancer
import re

instructions = []
with open('../inputs/16.txt') as fp:
    for line in fp:
        instructions = instructions + line.strip().split(',')

d = dancer.dancer()
r = re.compile(r'^(?P<i>[sxp])(?P<first>[a-p0-9]+)(?:/(?P<second>[a-p0-9]+))?$')

def dance(d, instructions):
    for instruction in deepcopy(instructions):
        m = r.search(instruction)
        if m.group('i') == 's':
            d.spin(int(m.group('first')))
        elif m.group('i') == 'x':
            d.exchange(int(m.group('first')), int(m.group('second')))
        elif m.group('i') == 'p':
            d.partner(m.group('first'), m.group('second'))
        else:
            raise ValueError('{} is not a recognized command'.format(m.group('i')))
    return d

def transform(base, letters, transformation):
    transformed = []
    for (i, letter) in enumerate(letters):
        n = letters[(letters.index(letter) + transformation[i]) % 26]
        transformed.append(n)
    return ''.join(transformed)

letters = 'abcdefghijklmnop'

d = dance(d, instructions)
print('Part 1: {}'.format(d))

#two = dance(d, instructions)
#print('again:  {}'.format(two))
#seen = [str(d)]

transformation = [letters.index(letter) - i for (i, letter) in enumerate(str(d))]
raw = transform(letters, letters, transformation)
print('Part 1: {}'.format(raw))
d = dance(d, instructions)
print(d)
print(transform(letters, raw, transformation))


#offset = 0
#for i in range(1000000000):
#    d = dance(d, instructions)
#    if d in seen:
#        offset = seen.index(str(d))
#        break
#    seen.append(str(d))
#
#print('repeat in iteration {} (originally {})'.format(i, offset))
#print('Part 2: {}'.format(''.join(seen[(1000000000 % i) + offset])))
