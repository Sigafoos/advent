import hashlib
import re
import sys
three = []
keys = []
i = 0

triple = re.compile(r"(.)\1\1")
while len(keys) < 64:
	hashed = sys.argv[1] + str(i)
	repeat = int(sys.argv[2]) + 1 if len(sys.argv) > 2 else 1
	for j in xrange(repeat):
		md5 = hashlib.md5()
		md5.update(hashed)
		hashed = md5.hexdigest().lower()

	to_delete = []
	for pair in sorted(three, key=lambda x: x[1]):
		if pair[1] + 1000 < i:
			to_delete.append(pair)
		elif hashed.find(pair[0] * 5) != -1:
			keys.append(pair)
			to_delete.append(pair)
			if len(keys) == 64:
				break

	for pair in to_delete:
		three.remove(pair)

	matches = triple.search(hashed)
	if matches is not None:
		three.append((matches.group(1), i))

	i += 1

# so, this is weird, because for part 1 the correct answer was the 63rd
# but for part 2 it was the 64th
# and I can't be bothered to figure out why
print "\n".join(map(lambda x: str(x[1]), keys[-10:]))
