import argparse

parser = argparse.ArgumentParser(description="Day 9 of Advent of Code 2018")
parser.add_argument("-m", "--marbles", type=int, required=True,
        help="The number of marbles to play with")
parser.add_argument("-p", "--players", type=int, required=True,
        help="The number of players")

args = parser.parse_args()

def str_circle(marbles, current, player):
    string = ''
    for i in range(len(marbles)):
        if i == current:
            string = '{} ({})'.format(string, marbles[i])
        else:
            string = '{} {}'.format(string, marbles[i])
    return '[{}] {}'.format(player, string)

player = 0
players = [0 for x in range(args.players)]
marbles = [x for x in range(1, args.marbles + 1)]
circle = [0]
i = 0

while len(marbles):
    marble = marbles.pop(0)
    if marble % 23 == 0:
        players[player] += marble
        i = (i - 7) % len(circle)
        players[player] += circle.pop(i)
    else:
        i = ((i + 1) % len(circle)) + 1
        circle = circle[:i] + [marble] + circle[i:]
    player = (player + 1) % len(players)

print('Part 1: {}'.format(max(players)))
