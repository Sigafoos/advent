import re
from string import ascii_lowercase

def trigger(string):
    for letter in ascii_lowercase:
        upper = letter.upper()
        string = string.replace('{}{}'.format(letter, upper), '')
        string = string.replace('{}{}'.format(upper, letter), '')
    return string

def react(string):
    n = trigger(string)
    while string != n:
        string = n
        n = trigger(n)
    return string

with open('../input/05.txt') as fp:
    polymer = fp.readlines()[0].strip()

after = react(polymer)
print('Part 1: {}'.format(len(after)))

removed = {}
for letter in ascii_lowercase:
    removed[letter] = len(react(re.sub(letter, '', polymer, flags=re.IGNORECASE)))

print('Part 2: {}'.format(min(removed.values())))
