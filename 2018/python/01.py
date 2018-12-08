with open('../input/01.txt') as fp:
    x = sum([int(l) for l in fp])
    print('Part 1: {}'.format(x))

with open('../input/01.txt') as fp:
    instructions = [int(l) for l in fp]

frequency = 0
i = 0
seen = [frequency]
while True:
    frequency += instructions[i]

    if frequency in seen:
        print('Part 2: {}'.format(frequency))
        break

    seen.append(frequency)
    i = (i + 1) % len(instructions)
