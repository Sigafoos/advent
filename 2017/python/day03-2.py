import collections
import sys

class infinigrid:
    q = collections.deque()
    maxwidth = 1
    y = 0
    d = 'r'
    highest = 0

    def __init__(self):
        self.q.append(collections.deque())
        self.q[0].append(1)

    def printgrid(self):
        for i in xrange(len(self.q)):
            for j in xrange(len(self.q[i])):
                print '[%s]' % str(self.q[i][j]).rjust(len(str(self.highest))),
            print ''

    def increment(self):
        val = 0
        x = len(self.q[self.y]) - 1
        y = self.y
        if self.d == 'r': # UL U UR L
            x = x + 1
            if self.y > 0:
                if x > 0:
                    val += self.q[y-1][x-1] # UL
                if x < self.maxwidth:
                    val += self.q[y-1][x] # U
                if len(self.q[y-1]) > x + 1:
                    val += self.q[y-1][x+1] # UR
            if x > 0:
                val += self.q[y][-1] # L

            self.q[self.y].append(val)
            if len(self.q[self.y]) > self.maxwidth:
                self.maxwidth += 1
                self.d = 'u'
        elif self.d == 'u': # UL L DL D
            if y == 0:
                val += self.q[0][-1] # D
                val += self.q[0][-2] # DL

                self.q.appendleft(collections.deque())
                self.d = 'l'
            else:
                self.y -= 1
                y = self.y

                if y > 0: # UL
                    val += self.q[y-1][x-1]
                if x > 1:
                    val += self.q[y][x-1] # L
                val += self.q[y+1][x] # D
                val += self.q[y+1][x-1] # DL

            self.q[self.y].append(val)

        elif self.d == 'l': # R DL D DR
            width = x + 1 # why did I name it this
            x = self.maxwidth - width - 1

            val += self.q[y][0] # R
            val += self.q[y+1][x+1] # DR
            if x >= 0:
                val += self.q[y+1][x] # D
            if x > 0:
                val += self.q[y+1][x-1] # DL
            self.q[self.y].appendleft(val)
            if width == self.maxwidth:
                self.maxwidth += 1
                self.y += 1
                self.d = 'd'
        elif self.d == 'd': # U UR R DR?
            val += self.q[y-1][0] # U
            val += self.q[y-1][1] # UR
            val += self.q[y][0] # R
            if y + 1 < len(self.q):
                val += self.q[y+1][0] # DR

            if self.y < len(self.q):
                self.q[self.y].appendleft(val)
                self.y += 1
            if self.y == len(self.q):
                self.q.append(collections.deque())
                self.d = 'r'
        if val > self.highest:
            self.highest = val
        return val

if len(sys.argv) == 1:
    print '*** error: you need to provide a target number, doofus ***'
    sys.exit()

val = 0
q = infinigrid()
stop = int(sys.argv[1])

while val < stop:
    val = q.increment()

print 'Part 2: %s' % val
