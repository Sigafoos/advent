import itertools
import re
import sys

def seating(filename, you = False):
	people = {}
	happiness = set()
	for line in open(filename):
		matches = re.search('^(?P<guest>\w+) would (?P<direction>gain|lose) (?P<value>\d+) happiness units by sitting next to (?P<neighbor>\w+).$', line)
		if matches.group('direction') == 'lose':
			value = -int(matches.group('value'))
		else:
			value = int(matches.group('value'))
		if matches.group('guest') not in people:
			people[matches.group('guest')] = {}
		people[matches.group('guest')][matches.group('neighbor')] = value
	if you:
		people['You'] = {}
		for person in people.keys():
			people[person]['You'] = 0
			people['You'][person] = 0
	options = itertools.permutations(people.keys())
	for chart in options:
		temp = 0
		for i in range(len(chart)):
			temp += people[chart[i]][chart[(i + 1) % len(chart)]] + people[chart[i]][chart[(i - 1) % len(chart)]]
		happiness.add(temp)
	return max(happiness)

print 'Part 1:' , seating('../input/input13.txt')
print 'With you:' , seating('../input/input13.txt', True)
