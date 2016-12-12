# chips are 1-5, generators are 11-15
import re

def safe(floor):
    global bad_combinations
    if floor in bad_combinations: # don't reinvent the wheel
        return False
    chips = []
    generators = []
    for item in floor:
        if item < 10:
            chips.append(item)
        else:
            generators.append(item - 10)
    for chip in chips:
        if chip not in generators and len(generators) > 0:
            bad_combinations.append(floor)
            return False
    return True

def combinations(floor):
    global known_combinations

    if str(floor) in known_combinations:
        yield known_combinations[str(floor)]

    combinations = []
    for permutation in xrange(1, 1 << len(floor)):
        if bin(permutation).count('1') > 2:
            continue
        contents = []
        current = permutation
        for item in floor:
            if current % 2 == 1:
                contents.append(item)
                if len(contents) == 2: # if we have two we won't have more
                    break
                current /= 2
        combinations.append(contents)
    known_combinations[str(floor)] = combinations
    yield combinations

floors = []
lookup = []
known_combinations = {}
bad_combinations = []
with open('../input/input11.txt') as fp:
    for line in fp:
        floor = []
        contents = re.findall(r'(a ([a-z]+)(?:-compatible)? ([a-z]+)(?:[,\. and]+?))+', line.strip())
        for item in contents:
            if item[1] not in lookup:
                lookup.append(item[1])
            index = lookup.index(item[1])
            if item[2] == 'microchip':
                floor.append(index + 1)
            else:
                floor.append(index + 11)
        floors.append(floor)

#lowest = 2**31 - 1 # 32 bit sys.maxint
lowest = 250

# where you've moved to, what you've brought, how long it's been
def move(current_floor = 0, contents = [], steps = 0):
    global floors
    global lowest
    global lookup

    steps += 1
    if steps == lowest or safe(floors[current_floor]) is False:
        return False

    if current_floor == 3 and len(floors[current_floor]) == len(lookup): # we did it!
        lowest = steps
        print 'Did it in %s' % lowest
        return True

    for contents in combinations(floors[current_floor]):
        for i in xrange(-1, 2, 2):
            target_floor = current_floor + i
            if target_floor < 0 or target_floor > 3: # invalid floor
                continue
            move(target_floor, contents, steps)

move()
print 'Part 1: %s' % lowest
