import re
import sys

def parse(filename):
	reindeer = {}
	for line in open(filename):
		matches = re.search('(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.', line)
		reindeer[matches.group(1)] = {
			'speed': int(matches.group(2)),
			'duration': int(matches.group(3)),
			'rest': int(matches.group(4))
		}
	return reindeer

def race(reindeer, length):
	results = {}
	for (name, stats) in reindeer.iteritems():
		cycle = stats['duration'] + stats['rest']
		full = length / cycle
		mod = length % cycle
		if mod < stats['duration']:
			results[name] = (full * stats['speed'] * stats['duration']) + (stats['speed'] * mod)
		else:
			results[name] = ((full + 1) * stats['speed'] * stats['duration'])
	return results


reindeer = parse('../input/input14.txt')
results = race(reindeer, int(sys.argv[1]))
print 'Part 1:' , results[max(results, key=results.get)]
