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

d = dance(d, instructions)
print('Part 1: {}'.format(d))

pattern = [d.index(i) for i in [x for x in 'abcdefghijklmnop']]
iterated = [x for x in str(d)]
seen = [iterated]
offset = 0
for i in range(1000000000):
    tmp = [iterated[x] for x in pattern]
    if tmp in seen:
        offset = seen.index(tmp)
        break
    seen.append(tmp)
    iterated = tmp
    print('using iterated: {}'.format(''.join(tmp)))
    second = dance(d, instructions)
    print('using dance(): {}'.format(second))
    print('pattern: {}'.format(pattern))
    import sys
    sys.exit(1)

print('second: {}'.format(second))
print('third: {}'.format(dance(second, instructions)))
print('\n'.join(map(lambda x: ''.join(x), seen[0:3])))
print('---last---')
print('\n'.join(map(lambda x: ''.join(x), seen[-3:])))
print('repeat in iteration {} (originally {})'.format(i, offset))
print('Part 2: {}'.format(''.join(seen[(1000000000 % i) + offset])))
