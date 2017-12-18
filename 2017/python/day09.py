import re

with open('../inputs/09.txt') as fp:
    instructions = fp.readline().strip()

instructions = re.sub(r'!.', '', instructions)
garbage = re.findall(r'<(.*?)>', instructions)
instructions = re.sub(r'<.*?>', '', instructions)
instructions = re.sub(r'[, ]', '', instructions)

depth = 0
score = 0
for char in instructions:
    if char is '{':
        depth += 1
    else:
        score += depth
        depth -= 1

print 'Part 1: %s' % score
print 'Part 2: %s' % sum(map(len, garbage))
