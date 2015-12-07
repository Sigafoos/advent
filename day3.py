housemap = set()
housemap.add((0,0)) # otherwise it reduces to [0]
robomap = set()
robomap.add((0,0))
houses = 1
robohouses = 1
x = 0
y = 0
santax = 0
santay = 0
robox = 0
roboy = 0
i = 0
for line in open('input3.txt', 'r'):
	for character in line.strip():
		if character == '^':
			y += 1
			if i % 2 == 0:
				santay += 1
			else:
				roboy += 1
		elif character == 'v':
			y -= 1
			if i % 2 == 0:
				santay -= 1
			else:
				roboy -= 1
		elif character == '>':
			x += 1
			if i % 2 == 0:
				santax += 1
			else:
				robox += 1
		elif character == '<':
			x -= 1
			if i % 2 == 0:
				santax -= 1
			else:
				robox -= 1

		if (x,y) not in housemap:
			housemap.add((x,y))
			houses += 1

		if i % 2 == 0 and (santax,santay) not in robomap:
			robomap.add((santax,santay))
			robohouses += 1
		elif (robox,roboy) not in robomap:
			robomap.add((robox,roboy))
			robohouses += 1
		i += 1

print 'Santa alone would visit' , houses , 'houses'
print 'Santa and Robo Santa would visit' , robohouses , 'houses'
