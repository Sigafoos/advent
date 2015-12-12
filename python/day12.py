import json

def add(thing):
	total = 0
	if type(thing) is dict:
		for (key, item) in thing.items():
			total += add(item)
	elif type(thing) is list:
		for item in thing:
			total += add(item)
	elif type(thing) is int or (type(thing) is str and thing.isnumeric()):
		return int(thing)
	return total

fp = open('../input/input12.txt')
json = json.load(fp)
print add(json)
