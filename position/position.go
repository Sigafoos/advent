package position

import (
	"fmt"
	"math"
)

type Position struct {
	X        int
	Y        int
	Velocity [2]int
}

func New(x, y int) *Position {
	return &Position{
		X: x,
		Y: y,
	}
}

func (p *Position) Manhattan(to *Position) int {
	dx := math.Abs(float64(p.X) - float64(to.X))
	dy := math.Abs(float64(p.Y) - float64(to.Y))
	return int(dx + dy)
}

func (p *Position) String() string {
	return fmt.Sprintf("(%v, %v)", p.X, p.Y)
}

func (p *Position) Move() {
	p.X += p.Velocity[0]
	p.Y += p.Velocity[1]
}
