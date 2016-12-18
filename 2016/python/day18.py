def next_floor(floor):
    new = ''
    for i in xrange(len(floor)):
        l = '.' if i == 0 else floor[i-1]
        c = floor[i]
        r = '.' if i + 1 == len(floor) else floor[i+1]
        if (l == '^' and c == '^' and r == '.') or (l == '.' and c == '^' and r == '^') or (l == '^' and c == '.' and r == '.') or (l == '.' and c == '.' and r == '^'):
            new += '^'
        else:
            new += '.'
    return new

floor = []
with open('../input/input18.txt') as fp:
    floor.append(fp.readline().strip())

while len(floor) < 400000:
    floor.append(next_floor(floor[-1]))

print 'Part 1: %s' % sum(map(lambda x: x.count('.'), floor[:40]))
print 'Part 2: %s' % sum(map(lambda x: x.count('.'), floor))
