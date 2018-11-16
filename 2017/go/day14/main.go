package main

import (
	"fmt"
	"os"
	"strings"

	"github.com/Sigafoos/advent/2017/go/knothash"
	"github.com/Sigafoos/advent/binary"
)

type Point struct {
	x      int
	y      int
	On     bool
	grid   *Grid
	region *Region
}

func (p *Point) Neighbors() []*Point {
	var neighbors []*Point
	coordinates := [][2]int{
		[2]int{p.x - 1, p.y},
		[2]int{p.x + 1, p.y},
		[2]int{p.x, p.y - 1},
		[2]int{p.x, p.y + 1},
	}

	for _, coordinate := range coordinates {
		if c := p.grid.Get(coordinate[0], coordinate[1]); c != nil && c.On {
			neighbors = append(neighbors, c)
		}
	}
	return neighbors
}

type Region struct {
	points []*Point
}

func (r *Region) Append(p *Point) {
	r.points = append(r.points, p)
}

func (r *Region) MoveTo(n *Region) {
	if r == n {
		return
	}

	for _, p := range r.points {
		p.region = n
		n.Append(p)
	}
	r.points = []*Point{}
}

func NewGrid() *Grid {
	return &Grid{
		points: make(map[int]map[int]*Point),
	}
}

type Grid struct {
	points map[int]map[int]*Point
}

func (g *Grid) Append(p *Point) {
	if _, exists := g.points[p.y]; !exists {
		g.points[p.y] = make(map[int]*Point)
	}
	g.points[p.y][p.x] = p
}

func (g *Grid) Get(x, y int) *Point {
	if _, exists := g.points[y]; exists {
		if p, exists := g.points[y][x]; exists {
			return p
		}
	}
	return nil
}

func (g *Grid) CalculateRegions() {
	for y := 0; y < len(g.points); y++ {
		for x := 0; x < len(g.points[y]); x++ {
			p := g.Get(x, y)
			if !p.On {
				continue
			}

			neighbors := p.Neighbors()
			regionMap := make(map[*Region]bool)
			var regions []*Region
			for _, n := range neighbors {
				if n.region != nil {
					if _, exists := regionMap[n.region]; !exists {
						regionMap[n.region] = true
						regions = append(regions, n.region)
					}
				}
			}
			switch len(regions) {
			case 0:
				p.region = &Region{}
				p.region.Append(p)
			case 1:
				p.region = regions[0]
				p.region.Append(p)
			default:
				p.region = regions[0]
				p.region.Append(p)
				for i := 1; i < len(regions); i++ {
					regions[i].MoveTo(regions[0])
				}
			}
		}
	}
}

func (g *Grid) Regions() []*Region {
	r := make(map[*Region]bool)
	for y := 0; y < len(g.points); y++ {
		for x := 0; x < len(g.points[y]); x++ {
			p := g.Get(x, y)
			if _, exists := r[p.region]; !exists {
				r[p.region] = true
			}
		}
	}

	var regions []*Region
	for region, _ := range r {
		regions = append(regions, region)
	}

	return regions
}

func (g *Grid) Print() {
	for y := 0; y < len(g.points); y++ {
		var row string
		for x := 0; x < len(g.points[y]); x++ {
			var c string
			p := g.Get(x, y)
			if p.On {
				c = "#"
			} else {
				c = "."
			}
			row = fmt.Sprintf("%s%s", row, c)
		}
		fmt.Println(row)
	}
}

func (g *Grid) PrintRegions() {
	regionMap := make(map[*Region]int)
	for i, r := range g.Regions() {
		regionMap[r] = i
	}

	for y := 0; y < len(g.points); y++ {
		for x := 0; x < len(g.points[y]); x++ {
			p := g.Get(x, y)
			if !p.On {
				fmt.Print(".")
			} else {
				fmt.Print(regionMap[p.region] % 10)
			}
		}
		fmt.Println()
	}
}

func main() {
	if len(os.Args) < 2 {
		panic("you must pass a key as an argument")
	}

	key := os.Args[1]
	grid := NewGrid()

	count := 0
	for i := 0; i < 128; i++ {
		row := knothash.HashString(fmt.Sprintf("%s-%v", key, i))
		inB := binary.StringHexString(row) // with apologies to Terry Riley
		count += strings.Count(inB, "1")

		for j, c := range inB {
			grid.Append(&Point{
				x:    j,
				y:    i,
				On:   fmt.Sprintf("%c", c) == "1",
				grid: grid,
			})
		}
	}
	fmt.Printf("Part 1: %v\n", count)

	grid.CalculateRegions()
	fmt.Printf("Part 2: %v\n", len(grid.Regions())-1)
}
