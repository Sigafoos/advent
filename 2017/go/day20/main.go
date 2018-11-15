package main

import (
	"bufio"
	"fmt"
	"io"
	"math"
	"os"
	"regexp"
	"strconv"
)

func NewPosition(x, y, z string) *Position {
	ix, err := strconv.Atoi(x)
	if err != nil {
		panic(err)
	}
	iy, err := strconv.Atoi(y)
	if err != nil {
		panic(err)
	}
	iz, err := strconv.Atoi(z)
	if err != nil {
		panic(err)
	}

	return &Position{
		x: ix,
		y: iy,
		z: iz,
	}
}

type Position struct {
	x int
	y int
	z int
}

func NewParticle(matches []string) *Particle {
	return &Particle{
		position:     NewPosition(matches[1], matches[2], matches[3]),
		velocity:     NewPosition(matches[4], matches[5], matches[6]),
		acceleration: NewPosition(matches[7], matches[8], matches[9]),
	}
}

type Particle struct {
	position     *Position
	velocity     *Position
	acceleration *Position
}

func (p *Particle) Update() {
	p.velocity.x += p.acceleration.x
	p.velocity.y += p.acceleration.y
	p.velocity.z += p.acceleration.z

	p.position.x += p.velocity.x
	p.position.y += p.velocity.y
	p.position.z += p.velocity.z
}

func (p *Particle) Manhattan() int {
	return int(math.Abs(float64(p.position.x)) + math.Abs(float64(p.position.y)) + math.Abs(float64(p.position.z)))
}

func NewBuffer(r io.Reader) *Buffer {
	Line := regexp.MustCompile("^p=< ?(-?[0-9]+), ?(-?[0-9]+), ?(-?[0-9]+)>, v=< ?(-?[0-9]+), ?(-?[0-9]+), ?(-?[0-9]+)>, a=< ?(-?[0-9]+), ?(-?[0-9]+), ?(-?[0-9]+)>$")
	scanner := bufio.NewScanner(r)
	var particles []*Particle
	for scanner.Scan() {
		particles = append(particles, NewParticle(Line.FindStringSubmatch(scanner.Text())))
	}
	return &Buffer{particles: particles}
}

type Buffer struct {
	particles []*Particle
}

func (b *Buffer) Tick() {
	for _, p := range b.particles {
		p.Update()
	}
}

func (b *Buffer) Closest() int {
	closest := int(^uint(0) >> 1)
	distance := closest
	for i, p := range b.particles {
		newDistance := p.Manhattan()
		if newDistance < distance {
			distance = newDistance
			closest = i
		}
	}

	return closest
}

func main() {
	fp, err := os.Open("../../inputs/20.txt")
	if err != nil {
		panic(err)
	}

	b := NewBuffer(fp)
	for i := 0; i < 1000; i++ {
		b.Tick()
	}
	fmt.Printf("Part 1: %v\n", b.Closest())
}
