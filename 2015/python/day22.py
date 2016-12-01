import re
import sys

def debug(self, message):
	if len(sys.argv) == 2 and sys.argv[1] == '-d':
		print message

class Player:
	armor = 0
	hp = 50
	mana = 500
	mana_spent = 0
	shield = 0 # timer
	poison = 0 # timer
	recharge = 0 # timer

	def missile(self, boss):
		if self.mana < 53:
			return False
		self.mana -= 53
		self.mana_spent += 53
		boss.hp -= 4
		debug('Player casts Magic Missile, dealing 4 damage.')

	def drain(self, boss):
		if self.mana < 73:
			return False
		self.mana -= 73
		self.mana_spent += 73
		boss.hp -= 2
		self.hp += 2
		debug('Player casts Drain, dealing 2 damage, and healing 2 hit points.')

	def shield(self):
		if self.mana < 113 or self.shield != 0:
			return False
		self.mana -= 113
		self.mana_spent += 113
		self.armor += 7
		self.shield = 6
		self.debug('Player casts Shield, increasing armor by 7.')

	def drain_shield(self):
		if self.shield > 0:
			self.shield -= 1
			debug("Shield's timer is now " + str(self.shield) + ".")
			if self.shield == 0:
				self.armor -= 7
				debug('Shield wears off, decreasing armor by 7.')

	def poison(self):
		if self.mana < 173 or self.poison != 0:
			return False
		self.mana -= 173
		self.mana_spent += 173
		poison = 6
		debug('Player casts Poison.')

	def drain_poison(self, boss):
		if self.poison > 0:
			self.poison -= 1
			debug('Poison deals 3 damage; its timer is now ' + str(self.poison) + '.')
			boss.hp -= 3

	def recharge(self):
		if self.mana < 229 or self.recharge != 0:
			return False
		self.mana -= 229
		self.mana_spent += 229
		self.recharge = 5
		debug('Player casts Recharge.')

	def drain_recharge(self):
		if self.recharge > 0:
			self.mana += 101
			self.recharge -= 1
			debug('Recharge provides 101 mana; its timer is now ' + str(self.recharge) + '.')

	def turn(self, action, boss):
		self.drain_recharge()
		self.drain_poison(boss)
		self.drain_shield()
		if action == 'missile':
			self.missile(boss)
		elif action == 'drain':
			self.drain(boss)
		elif action == 'shield':
			self.shield()
		elif action == 'poison':
			self.poison
		elif action == 'recharge':
			self.recharge

	def hit(self, amount): # be attacked
		self.hp -= amount - self.armor
		debug('Boss attacks for ' + str(amount) + ' - ' + str(self.armor) + ' = ' + str(amount - self.armor))
		if self.hp <= 0:
			debug('The player has been killed')

class Boss:
	attack = 0
	hp = 0

	def __init__(self):
		for line in open('../input/input22.txt'):
			matches = re.search('([\w\s]+): (\d+)', line)
			if matches.group(1) == 'Armor':
				self.armor = int(matches.group(2))
			elif matches.group(1) == 'Damage':
				self.attack = int(matches.group(2))
			elif matches.group(1) == 'Hit Points':
				self.hp = int(matches.group(2))

	def attack(self, player): #attack someone else
		player.drain_recharge()
		player.drain_poison(self)
		player.drain_recharge()
		player.hit(self.attack)

	def hit(self, amount): # be attacked
		self.hp -= amount
		if self.hp <= 0:
			debug('The boss has been killed')

me = Player()
you = Boss()

while me.hp > 0 and you.hp > 0:
	print '-- Player turn --'
	print '- Player has' , me.hp , 'hit points,' , me.armor , 'armor,' , me.mana, ' mana'
	print '- Boss has' , you.hp , 'hit points'
	action = 
