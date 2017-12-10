import collections
import sys

class infinigrid:
    q = collections.deque()
    i = 0
    maxwidth = 1
    y = 0
    d = 'r'

    def __init__(self):
        self.q.append(collections.deque())

    def printgrid(self):
        intlen = len(str(self.i))
        for i in xrange(len(self.q)):
            for j in xrange(len(self.q[i])):
                print '[%s]' % str(self.q[i][j]).rjust(intlen),
            print ''

    def increment(self):
        self.i += 1
        if self.d == 'r':
            if len(self.q[self.y]) <= self.maxwidth:
                self.q[self.y].append(self.i)
            if len(self.q[self.y]) > self.maxwidth:
                self.maxwidth += 1
                self.d = 'u'
        elif self.d == 'u':
            if self.y > 0:
                self.y -= 1
                self.q[self.y].append(self.i)
            else:
                self.q.appendleft(collections.deque())
                self.d = 'l'
                self.q[self.y].appendleft(self.i)
        elif self.d == 'l':
            self.q[self.y].appendleft(self.i)
            if len(self.q[self.y]) > self.maxwidth:
                self.maxwidth += 1
                self.y += 1
                self.d = 'd'
        elif self.d == 'd':
            if self.y < len(self.q):
                self.q[self.y].appendleft(self.i)
                self.y += 1
            if self.y == len(self.q):
                self.q.append(collections.deque())
                self.d = 'r'

    def manhattan(self, target):
        positions = [None, None]
        y = 0
        while positions[0] is None or positions[1] is None:
            listed = list(self.q[y])
            if positions[0] is None:
                try:
                    x = listed.index(1)
                    positions[0] = (x, y)
                except ValueError:
                    pass
            if positions[1] is None:
                try:
                    x = listed.index(target)
                    positions[1] = (x, y)
                except ValueError:
                    pass
            y += 1
        #print '1 is at (%s, %s)\n%s is at (%s, %s)' % (positions[0][0], positions[0][1], target, positions[1][0], positions[1][1])
        return abs(positions[0][0] - positions[1][0]) + abs(positions[0][1] - positions[1][1])

if len(sys.argv) == 1:
    print '*** error: you need to provide a target number, doofus ***'
    sys.exit()

i = 0
q = infinigrid()
stop = int(sys.argv[1])

while i < stop or q.d == 'd' or q.d == 'l':
    i += 1
    q.increment()

#q.printgrid()
print q.manhattan(stop)
