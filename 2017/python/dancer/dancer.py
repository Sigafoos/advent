class dancer:
    dancers = []

    def __init__(self):
        self.dancers = [d for d in 'abcdefghijklmnop']

    def __repr__(self):
        return ''.join(self.dancers)

    def spin(self, x):
        self.dancers = self.dancers[-x:] + self.dancers[:-x]

    def exchange(self, a, b):
        tmp = self.dancers[a]
        self.dancers[a] = self.dancers[b]
        self.dancers[b] = tmp

    def partner(self, a, b):
        first = self.dancers.index(a)
        second = self.dancers.index(b)
        self.dancers[first] = b
        self.dancers[second] = a

    def index(self, x):
        return self.dancers.index(x)
