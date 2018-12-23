package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"

	"github.com/Sigafoos/advent/position"
)

type Grid struct {
	Coordinates []*position.Position
}

func NewGrid() *Grid {
	return &Grid{
		Coordinates: []*position.Position{},
	}
}

func (g *Grid) Add(c *position.Position) {
	g.Coordinates = append(g.Coordinates, c)
}

func (g *Grid) Max() *position.Position {
	x := 0
	y := 0
	for _, p := range g.Coordinates {
		if p.X > x {
			x = p.X
		}
		if p.Y > y {
			y = p.Y
		}
	}

	return position.New(x, y)
}

func (g *Grid) Min() *position.Position {
	x := 99999
	y := 99999
	for _, p := range g.Coordinates {
		if p.X < x {
			x = p.X
		}
		if p.Y < y {
			y = p.Y
		}
	}

	return position.New(x, y)
}

func (g *Grid) Closest(to *position.Position) *position.Position {
	lowest := 9999
	var closest *position.Position
	tied := false
	for _, p := range g.Coordinates {
		if distance := p.Manhattan(to); distance < lowest {
			tied = false
			lowest = distance
			closest = p
		} else if distance == lowest {
			tied = true
		}
	}

	if tied {
		return nil
	}
	return closest
}

func (g *Grid) Areas() map[position.Position][]*position.Position {
	min := g.Min()
	max := g.Max()
	regions := make(map[position.Position][]*position.Position)

	for x := min.X + 1; x < max.X; x++ {
		for y := min.Y + 1; y < max.Y; y++ {
			examining := position.New(x, y)
			point := g.Closest(examining)
			if point == nil {
				continue
			}

			if point.X <= min.X || point.X >= max.X || point.Y <= min.Y || point.Y >= max.Y {
				continue
			}

			if _, exists := regions[*point]; exists {
				regions[*point] = append(regions[*point], examining)
			} else {
				regions[*point] = []*position.Position{examining}
			}
		}
	}

	return regions
}

func (g *Grid) Safe(distance int) []*position.Position {
	min := g.Min()
	max := g.Max()
	safe := []*position.Position{}
	// I included points outside the min/max but they didn't have any effect
	// (beyond slowing runtime down a *lot*)
	for x := min.X; x < max.X; x++ {
		for y := min.Y; y < max.Y; y++ {
			point := position.New(x, y)
			total := 0

			for _, c := range g.Coordinates {
				total += point.Manhattan(c)
				if total >= distance {
					break
				}
			}

			if total < distance {
				safe = append(safe, point)
			}
		}
	}

	return safe
}

func main() {
	fp, err := os.Open("../../input/06.txt")
	if err != nil {
		panic(err)
	}

	grid := NewGrid()

	scanner := bufio.NewScanner(fp)
	for scanner.Scan() {
		coords := strings.Split(scanner.Text(), ", ")
		x, err := strconv.Atoi(coords[0])
		if err != nil {
			panic(err)
		}
		y, err := strconv.Atoi(coords[1])
		if err != nil {
			panic(err)
		}
		grid.Add(position.New(x, y))
	}

	largest := []*position.Position{}
	for _, v := range grid.Areas() {
		if len(v) > len(largest) {
			largest = v
		}
	}

	fmt.Printf("Part 1: %v\n", len(largest))
	fmt.Printf("Part 2: %v\n", len(grid.Safe(10000)))
}
