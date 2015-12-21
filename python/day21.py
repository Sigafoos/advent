import itertools
import re
import sys

def fight(player, boss):
	while player['hp'] > 0 and boss['hp'] > 0:
		boss['hp'] -= player['damage'] - boss['armor']
		if boss['hp'] <= 0:
			return True
		player['hp'] -= boss['damage'] - player['armor']
	return False

boss = {}
for line in open('../input/input21.txt'):
	matches = re.search('([\w\s]+): (\d+)', line)
	if matches.group(1) == 'Armor':
		boss['armor'] = int(matches.group(2))
	elif matches.group(1) == 'Damage':
		boss['damage'] = int(matches.group(2))
	elif matches.group(1) == 'Hit Points':
		boss['hp'] = int(matches.group(2))
bosshp = boss['hp']

with open('../input/input21-shop.txt') as fp:
	raw = fp.read()
	matches = re.findall('((\w+):.+?\n(.+\n?)+)\n?', raw)
	shops = {}
	for shop in matches:
		shops[shop[1]] = []
		# I tried to do it in one go. didn't work
		items = re.findall('(\w+(?:\s\+\d)?)\s+(\d+)\s+(\d+)\s+(\d+)', shop[0])
		for item in items:
			shops[shop[1]].append({
				'Name': item[0],
				'Cost': int(item[1]),
				'Damage': int(item[2]),
				'Armor': int(item[3]),
			})
	fp.close()

perms = itertools.product(xrange(len(shops['Weapons'])), xrange(-1, len(shops['Armor'])), xrange(-1, len(shops['Rings'])), xrange(-1, len(shops['Rings'])))

winning = set()
losing = set()
for option in perms:
	player = {
		'armor': 0,
		'damage': 1,
		'hp': 100
	}
	boss['hp'] = bosshp
	gold = 0
	if option[2] == option[3] and option[2] != -1:
		continue # can't buy two of the same rings
	player['damage'] = shops['Weapons'][option[0]]['Damage']
	gold += shops['Weapons'][option[0]]['Cost']
	if option[1] != -1:
		player['armor'] += shops['Armor'][option[1]]['Armor']
		gold += shops['Armor'][option[1]]['Cost']
	if option[2] != -1:
		player['armor'] += shops['Rings'][option[2]]['Armor']
		player['damage'] += shops['Rings'][option[2]]['Damage']
		gold += shops['Rings'][option[2]]['Cost']
	if option[3] != -1:
		player['armor'] += shops['Rings'][option[3]]['Armor']
		player['damage'] += shops['Rings'][option[3]]['Damage']
		gold += shops['Rings'][option[3]]['Cost']
	if fight(player, boss):
		winning.add(gold)
	else :
		losing.add(gold)

print 'Part 1:' , min(winning)
print 'Part 2:' , max(losing)
