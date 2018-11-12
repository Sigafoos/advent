import sys

class spinlock:
    def __init__(self, steps):
        self.buffer = [0]
        self.position = 0
        self.value = 0
        self.steps = steps

    def spin(self):
        self.value += 1
        self.position = ((self.steps + self.position) % len(self.buffer)) + 1
        self.buffer.insert(self.position, self.value)

    def index(self, i):
        return self.buffer.index(i)

    def __repr__(self):
        return self.buffer

    def __str__(self):
        string = list(map(str, self.buffer))
        string[self.position] = '({})'.format(string[self.position])
        return ' '.join(string)

    def __getitem__(self, i):
        return self.buffer[i]

if len(sys.argv) < 2 or not sys.argv[1].isdigit():
    raise ValueError('no number of steps passed in argv')

spinner = spinlock(int(sys.argv[1]))
for i in range(2017):
    spinner.spin()
print('Part 1: {}'.format(spinner[spinner.index(2017)+1]))
