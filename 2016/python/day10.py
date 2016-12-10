bots = {}
outputs = {}
instructions = []
def give(targets, target, value):
    value = int(value)
    if target in targets:
        targets[target].append(value)
    else:
        targets[target] = [value]
    return targets

with open('../input/input10.txt') as fp:
    for line in fp:
        command = line.strip().split()
        if command[0] == 'value':
            bots = give(bots, command[5], command[1])
        elif command[0] == 'bot':
            instructions.append({
                'bot': command[1],
                'low': [command[5], command[6]],
                'high': [command[10], command[11]]
            })

part1 = None
while len(instructions) > 0:
    instruction = instructions.pop(0)
    if instruction['bot'] not in bots or len(bots[instruction['bot']]) < 2: # don't have enough, try later
        instructions.append(instruction)
        continue

    values = sorted(bots[instruction['bot']])
    if part1 is None and values == [17, 61]:
        part1 = instruction['bot']
    bots[instruction['bot']] = []

    for value in ('high', 'low'):
        target = bots if instruction[value][0] == 'bot' else outputs
        target = give(target, instruction[value][1], values.pop())

print 'Part 1: %s' % part1
print 'Part 2: %s' % reduce(lambda x, y: x*y, map(lambda z: z[0], [outputs['0'], outputs['1'], outputs['2']]))
