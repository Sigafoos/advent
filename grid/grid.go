package grid

import (
	"github.com/Sigafoos/advent/position"
)

type Grid struct {
	Coordinates []*position.Position
}

func New() *Grid {
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
