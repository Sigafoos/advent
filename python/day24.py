import itertools
import sys

packages = []
for package in open('../input/input24.txt'):
	packages.append(int(package.strip()))
goal = sum(packages) / 3

good = []
for combo in itertools.permutations(packages):
	sleigh = [[], [], []]
	for package in combo:
		sleigh = sorted(sleigh, key=sum, reverse=True)
		for i in xrange(3):
			if sum(sleigh[i]) + package <= goal:
				sleigh[i].append(package)
				break
		else:
			break
	else:
		good.append(sleigh)

products = set()
length = 999999
for sleigh in good:
	sleigh = sorted(sleigh, key=len)
	product = reduce(lambda x, y: x * y, sleigh[0])
	if len(sleigh[0]) <= length:
		length = len(sleigh[0])
		products.add(product)

print min(products)
