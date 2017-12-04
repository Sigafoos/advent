import re

p = re.compile(r'^[a-zA-Z ]+$')

def is_passphrase(password, part2 = False):
    split = password.split()
    if p.search(password) is None:
        return False
    if len(split) != len(set(split)):
        return False
    if part2 and len(split) != len(set(map(lambda x: ''.join(sorted(x)), split))):
        return False
    return True

passwords = []

with open('../inputs/04.txt') as fp:
    for line in fp:
        passwords.append(line.strip())

part1 = []
part2 = []
for password in passwords:
    if is_passphrase(password):
        part1.append(password)

        if is_passphrase(password, True):
            part2.append(password)

print 'Part 1: %s' % len(part1)
print 'Part 2: %s' % len(part2)
