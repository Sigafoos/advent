package main

import (
	"os"
	"strings"
	"bufio"
	"fmt"
	"io"
)

type Coordinate [2]int

func (c *Coordinate) Step(i int) *Coordinate {
	switch i {
	case UP:
		return &Coordinate{c[0], c[1]-1}
	case DOWN:
		return &Coordinate{c[0], c[1]+1}
	case LEFT:
		return &Coordinate{c[0]-1, c[1]}
	case RIGHT:
		return &Coordinate{c[0]+1, c[1]}
	}
	panic(fmt.Sprintf("%v is not a direction", i))
}

const (
	UP = iota
	LEFT = iota
	RIGHT = iota
	DOWN = iota
)

const (
	letters = "ABCDEFGHIJKLMNOPQRSTUVQXYZ"
)

func NewDiagram(r io.Reader) *Diagram {
	d := Diagram{ Map: make(map[Coordinate]rune) }
	scanner := bufio.NewScanner(r)
	for y := 0; scanner.Scan(); y++ {
		for x, v := range scanner.Text() {
			d.Map[Coordinate{x,y}] = v
		}
	}
	return &d
}

type Diagram struct {
	Map map[Coordinate]rune
}

func (d *Diagram) At(c *Coordinate) rune {
	return d.Map[*c]
}

func NewPacket(d *Diagram) *Packet {
	// the starting path is "just off the top of the diagram", so find the entrance
	var c Coordinate
	for x := 0; ; x++ {
		if d.At(&Coordinate{x, 0}) == '|' {
			c = Coordinate{x, 0}
			break
		}
	}
	return &Packet{
		seen: []rune{},
		position: c,
		direction: DOWN,
		diagram: d,
	}
}

type Packet struct {
	seen []rune
	diagram *Diagram
	position Coordinate
	direction int
	steps int
}

func (p *Packet) Space() rune {
	return p.diagram.At(&p.position)
}

func (p *Packet) ValidMove(direction int) bool {
	next := p.position.Step(direction)
	space := p.diagram.At(next)
	return space != ' ' && space != 0
}

func (p *Packet) Traverse() {
	for {
		p.steps++
		if strings.ContainsRune(letters, p.Space()) {
			p.seen = append(p.seen, p.Space())
		}

		if p.ValidMove(p.direction) {
			p.position = *p.position.Step(p.direction)
			continue
		}

		valid := false
		// try to find a new way to go
		for _, direction := range []int{UP, DOWN, LEFT, RIGHT} {
			// exclude the same direction and the opposite. uses bitwise math:
			// UP = 00
			// DOWN = 11
			// LEFT = 01
			// RIGHT = 10
			if direction == p.direction || direction ^ 3 == p.direction {
				continue
			}

			if p.ValidMove(direction) {
				p.direction = direction
				p.position = *p.position.Step(direction)
				valid = true
				break
			}
		}

		if !valid {
			break
		}
	}
}

func (p *Packet) Log() string {
	return string(p.seen)
}

func main() {
	r, err := os.Open("../../inputs/19.txt")
	if err != nil {
		panic(err)
	}

	p := NewPacket(NewDiagram(r))
	p.Traverse()
	fmt.Printf("Part 1: %s\n", p.Log())
	fmt.Printf("Part 2: %v\n", p.steps)
}
