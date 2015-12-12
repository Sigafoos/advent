import json

def add(thing, red = True):
	total = 0
	if type(thing) is dict:
		if not red and 'red' in thing.values():
			return 0
		for (key, item) in thing.items():
			if not red and key == 'red':
				continue
			total += add(item, red)
	elif type(thing) is list:
		for item in thing:
			total += add(item, red)
	elif type(thing) is int or (type(thing) is str and thing.isdigit()):
		return int(thing)
	return total

fp = open('../input/input12.txt')
json = json.load(fp)
print 'Part 1:' , add(json)
print 'Part 2:' , add(json, False)
