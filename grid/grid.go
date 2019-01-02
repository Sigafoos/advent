package grid

import (
	"fmt"
	"math"

	"github.com/Sigafoos/advent/position"
)

type Grid struct {
	Coordinates []*position.Position
	mapped      map[[2]int]bool
	Iteration   int
}

func New() *Grid {
	return &Grid{
		Coordinates: []*position.Position{},
		mapped:      make(map[[2]int]bool),
		Iteration:   1,
	}
}

func (g *Grid) Add(c *position.Position) {
	g.Coordinates = append(g.Coordinates, c)
	g.mapped[[2]int{c.X, c.Y}] = true
}

func (g *Grid) Update(printable bool) {
	g.Iteration++
	g.mapped = make(map[[2]int]bool)
	for _, p := range g.Coordinates {
		p.Move()
		g.mapped[[2]int{p.X, p.Y}] = true
	}

	// if we want it to update until it's printable, do that
	if printable {
		min := g.Min()
		max := g.Max()

		if math.Abs(float64(min.X-max.X)) > 100 || math.Abs(float64(min.Y-max.Y)) > 100 {
			g.Update(true)
		}
	}
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

func (g *Grid) Sprint(min, max *position.Position) string {
	m := ""

	for y := min.Y; y <= max.Y; y++ {
		for x := min.X; x <= max.X; x++ {
			if _, ok := g.mapped[[2]int{x, y}]; ok {
				m = fmt.Sprintf("%s#", m)
			} else {
				m = fmt.Sprintf("%s.", m)
			}
		}
		m = fmt.Sprintf("%s\n", m)
	}

	return m
}

func (g *Grid) Print(min, max *position.Position) {
	fmt.Println(g.Sprint(min, max))
}
