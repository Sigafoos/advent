package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

var r = regexp.MustCompile("^#([0-9]+) @ ([0-9]+),([0-9]+): ([0-9]+)x([0-9]+)")

type Position [2]int

type Claim struct {
	ID   int
	Area []*Position
}

func NewClaim(raw string) *Claim {
	p := r.FindStringSubmatch(raw)
	parsed := make([]int, len(p))
	for i, v := range p[1:] {
		pv, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		parsed[i] = pv
	}

	var positions []*Position
	for x := 0; x < parsed[3]; x++ {
		for y := 0; y < parsed[4]; y++ {
			positions = append(positions, &Position{parsed[1] + 1 + x, parsed[2] + 1 + y})
		}
	}

	return &Claim{
		ID:   parsed[0],
		Area: positions,
	}
}

func (c *Claim) Overlaps(positions map[Position]int) bool {
	for _, p := range c.Area {
		if count, exists := positions[*p]; exists && count > 1 {
			return true
		}
	}

	return false
}

type Fabric struct {
	claims []*Claim
}

func NewFabric() *Fabric {
	return &Fabric{
		claims: []*Claim{},
	}
}

func (f *Fabric) AddClaim(c *Claim) {
	f.claims = append(f.claims, c)
}

func (f *Fabric) Overlapping() map[Position]int {
	positions := make(map[Position]int)
	for _, c := range f.claims {
		for _, p := range c.Area {
			if _, exists := positions[*p]; exists {
				positions[*p]++
			} else {
				positions[*p] = 1
			}
		}
	}

	return positions
}

func (f *Fabric) OpenClaim() *Claim {
	positions := f.Overlapping()

	for _, c := range f.claims {
		if !c.Overlaps(positions) {
			return c
		}
	}

	panic("no matching claim")
}

func main() {
	fp, err := os.Open("../../input/03.txt")
	if err != nil {
		panic(err)
	}

	f := NewFabric()

	scanner := bufio.NewScanner(fp)
	for scanner.Scan() {
		f.AddClaim(NewClaim(scanner.Text()))
	}

	overlapping := 0
	for _, v := range f.Overlapping() {
		if v > 1 {
			overlapping++
		}
	}

	fmt.Printf("Part 1: %v\n", overlapping)
	fmt.Printf("Part 2: %v\n", f.OpenClaim().ID)
}
