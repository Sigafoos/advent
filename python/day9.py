import itertools

cities = {}
for line in open('../input/input9.txt'):
	line = line.strip()
	parts = line.split(' ')
	for i in [0, 2]:
		if parts[i] not in cities:
			cities[parts[i]] = {}
	cities[parts[0]][parts[2]] = int(parts[4])
	cities[parts[2]][parts[0]] = int(parts[4])

trips = []
for trip in itertools.permutations(cities):
	distance = 0
	for i in range(len(trip) - 1):
		distance += cities[trip[i]][trip[i + 1]]
	trips.append(distance)
print 'The shortest distance is' , min(trips)
print 'The longest distance is' , max(trips)
