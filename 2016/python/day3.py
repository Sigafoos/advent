def get_triangles(numbers):
    return filter(lambda x: x[0] + x[1] > x[2], map(sorted, numbers))

numbers = []
with open('../input/input3.txt') as fp:
    for line in fp:
        numbers.append(map(int, line.split()))

print 'Part 1: %s' % len(get_triangles(numbers))

part2 = []
for i in xrange(len(numbers[0])):
    for j in xrange(0, len(numbers), 3):
        part2.append(map(lambda x: x[i], numbers[j:j+3]))

print 'Part 2: %s' % len(get_triangles(part2))
