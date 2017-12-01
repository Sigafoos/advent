import re
with open('../inputs/01.txt') as fp:
        captcha = fp.readline().strip()

pattern = re.compile(r'([\d])(?=\1)')
matches = pattern.findall(captcha)
if captcha[0] == captcha[-1]:
    matches.append(captcha[-1])
print "Part 1: %s" % sum(map(int, matches))

matches = []
length = len(captcha)
for i in xrange(length):
    if captcha[i] == captcha[(i + length/2) % length]:
        matches.append(captcha[i])
print "Part 2: %s" % sum(map(int, matches))
