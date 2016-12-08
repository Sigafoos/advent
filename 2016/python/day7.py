import re

def is_ssl(hypernets, aba):
    for match in aba:
        for section in hypernets:
            if section.find(match[1] + match[0] + match[1]) != -1:
                return True
    return False

tls = 0
ssl = 0
subsections = re.compile(r"[\[\]]") # split on supernets
re_abba = re.compile(r"([a-z])(?!\1)([a-z])\2\1")
re_aba = re.compile(r"([a-z])(?!\1)([a-z])\1")
with open('../input/input7.txt') as fp:
    for line in fp:
        sections = subsections.split(line.strip())
        supernets = sections[::2]
        hypernets = sections[1::2]

        # Part 1
        for section in hypernets:
            if re_abba.search(section) is not None:
                break
        else:
            for section in supernets:
                if re_abba.search(section) is not None:
                    tls += 1
                    break

        # Part 2
        aba = []
        for section in supernets:
            start = 0
            while True:
                matches = re_aba.search(section, start)
                if matches is None:
                    break
                else:
                    aba.append(matches.groups())
                    start = matches.start() + 1
        if is_ssl(hypernets, aba):
            ssl += 1

print 'Part 1: %s' % tls
print 'Part 2: %s' % ssl
