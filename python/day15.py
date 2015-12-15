import itertools
import re
import sys

def total(ingredients, recipe, counting_calories = False):
	score = {}
	for prop in ingredients.itervalues().next():
		score[prop] = 0
	for (ingredient, properties) in ingredients.iteritems():
		if ingredient in recipe:
			for (name, value) in properties.iteritems():
				score[name] += (value * recipe[ingredient])
	if counting_calories is True and score['calories'] != 500:
		return 0
	del score['calories']
	multiplied = 1
	for item in score.itervalues():
		if item < 1:
			return 0
		multiplied *= item
	return multiplied

def best(ingredients, counting_calories = False):
	scores = set()
	for combination in itertools.product(xrange(101), repeat=len(ingredients)):
		if sum(combination) is not 100:
			continue
		recipe = {}
		for (i, ingredient) in enumerate(ingredients):
			recipe[ingredient] = combination[i]
		scores.add(total(ingredients, recipe, counting_calories))
	return max(scores)

ingredients = {}
for line in open('../input/input15.txt'):
	matches = re.search('(?P<name>\w+): capacity (?P<capacity>-?\d+), durability (?P<durability>-?\d+), flavor (?P<flavor>-?\d+), texture (?P<texture>-?\d+), calories (?P<calories>-?\d+)', line)
	ingredients[matches.group('name')] = {
		'capacity': int(matches.group('capacity')),
		'durability': int(matches.group('durability')),
		'flavor': int(matches.group('flavor')),
		'texture': int(matches.group('texture')),
		'calories': int(matches.group('calories'))
	}

print 'Part 1:' , best(ingredients)
print 'Part 2:' , best(ingredients, True)
