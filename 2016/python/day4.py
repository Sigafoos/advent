import re
import string

valid = []
with open('../input/input4.txt') as fp:
    for line in fp:
        pattern = r"(?P<name>(?:[a-z]+-)+)(?P<sector>[0-9]+)\[(?P<checksum>[^\]]+)\]"
        match = re.match(pattern, line)
        room = {
            'name': match.group('name'),
            'sector': int(match.group('sector')),
            'checksum': match.group('checksum')
        }
        counted = {}
        for letter in set(room['name'].replace('-', '')):
            count = room['name'].count(letter)
            if count in counted:
                counted[count] += letter
            else:
                counted[count] = letter

        # this can also be written as:
        # checksum = ''.join(map(lambda x: ''.join(sorted(counted[x])), sorted(counted.keys(), reverse=True)))
        # but while more condensed, I think this is more readable:
        checksum = ''
        for i in sorted(counted.keys(), reverse=True):
            checksum += ''.join(sorted(counted[i]))

        if room['checksum'] == checksum[:len(room['checksum'])]:
            valid.append(room)

print 'Part 1: %s\n' % sum(map(lambda x: x['sector'], valid))

for room in valid:
    decrypted = ''.join(map(lambda x: string.ascii_lowercase[(string.ascii_lowercase.find(x) + room['sector']) % 26] if x != '-' else ' ', room['name']))
    if (decrypted.find('orth') != -1): # actually 'northpole object storage'
        print 'Part 2: %s (%s)' % (room['sector'], decrypted.strip())
