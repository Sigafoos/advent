import re

LINERE = re.compile(r'^\[(?P<date>\d{4}-\d{2}-\d{2}) (?P<hour>\d{2}):(?P<minute>\d{2})\] (?P<action>.*)$')
GUARDRE = re.compile('^Guard #(?P<id>\d+) begins shift$')

guards = {}
with open('../input/04-sorted.txt') as fp:
    guard = None
    started = None
    for line in fp:
        p = LINERE.match(line.strip())
        newguard = GUARDRE.match(p.group('action'))
        if newguard is not None and newguard.group('id') is not None:
            guard = newguard.group('id')
        elif guard is not None:
            if started is not None and p.group('action') == 'wakes up' and p.group('minute').isdigit():
                if guard not in guards:
                    guards[guard] = []
                guards[guard] += [m for m in range(started, int(p.group('minute')))]
                started = None
            elif started is None and p.group('action') == 'falls asleep' and p.group('minute').isdigit():
                started = int(p.group('minute'))
            else:
                raise ValueError('** ERROR! guard: {}, started: {}, line: {}\n{}'.format(guard, started, p.groups(), guards))
        else:
            raise ValueError('trying to set a sleep pattern when we dont have a guard')

sleepy = None
sleepymin = []
naps = {}

part2 = {
        'guard': None,
        'minute': 0,
        'count': 0
        }

for guard, minutes in guards.items():
    if sleepy is None or len(minutes) > len(guards[sleepy]):
        sleepy = guard

    nap = {}
    for minute in minutes:
        if minute in nap:
            nap[minute] += 1
        else:
            nap[minute] = 1
    naps[guard] = nap

    for minute, count in nap.items():
        if count > part2['count']:
            part2 = {
                    'guard': int(guard),
                    'minute': int(minute),
                    'count': count
                    }


the_minute = max(naps[sleepy], key=naps[sleepy].get)
print('Part 1: {}'.format(int(sleepy) * the_minute))
print('Part 2: {}'.format(part2['guard'] * part2['minute']))
