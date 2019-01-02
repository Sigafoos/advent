package main

import (
	"fmt"
	"os"
	"strconv"
)

type PowerGrid struct {
	cells  [][]int
	serial int
}

func NewPowerGrid(serial int) *PowerGrid {
	return &PowerGrid{
		serial: serial,
	}
}

func (pg *PowerGrid) Cell(x, y int) int {
	rack := x + 10
	power := rack * y
	power += pg.serial
	power *= rack

	// get just the hundredths digit
	power /= 100
	power = power - ((power / 10) * 10) - 5

	return power
}

func (pg *PowerGrid) SquarePower(x, y, size int) int {
	if x < 1 || x > 298 || y < 1 || y > 298 {
		panic(fmt.Sprintf("(%v, %v) is not a valid grid top left", x, y))
	}

	power := 0
	for ix := 0; ix < size; ix++ {
		for iy := 0; iy < size; iy++ {
			power += pg.Cell(x+ix, y+iy)
		}
	}

	return power
}

func (pg *PowerGrid) LargestGrid(size int) (largest [2]int, value int) {
	for x := 1; x < 299; x++ {
		if x+size > 300 {
			continue
		}
		for y := 1; y < 299; y++ {
			if y+size > 300 {
				continue
			}
			power := pg.SquarePower(x, y, size)
			if power > value {
				value = power
				largest = [2]int{x, y}
			}
		}
	}

	return largest, value
}

func main() {
	if len(os.Args) != 2 {
		panic("missing serial number")
	}

	serial, err := strconv.Atoi(os.Args[1])
	if err != nil {
		panic(err)
	}

	grid := NewPowerGrid(serial)
	largest, _ := grid.LargestGrid(3)
	fmt.Printf("Part 1: %v,%v\n", largest[0], largest[1])

	// taking a guess that 30 is the max size. it works out and saves... lots of time
	largest = [2]int{0, 0}
	value := 0
	size := 0
	for i := 1; i < 30; i++ {
		l, v := grid.LargestGrid(i)
		if v > value {
			largest = l
			value = v
			size = i
		}
	}
	fmt.Printf("Part 2: %v,%v,%v\n", largest[0], largest[1], size)
}
