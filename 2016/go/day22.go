package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

type Grid map[int]map[int]Node

type Node struct {
	X         int
	Y         int
	Size      int
	Used      int
	Available int
}

type Part1 [][2]Node

func (p Part1) Contains(a, b Node) bool {
	for _, v := range p {
		if (v[0] == a && v[1] == b) || (v[0] == b && v[1] == a) {
			return true
		}
	}
	return false
}

func main() {
	file, err := os.Open("../input/input22.txt")
	if err != nil {
		panic(err)
	}
	regex := regexp.MustCompile("^/dev/grid/node-x([0-9]{1,2})-y([0-9]{1,2}) +([0-9]+)T +([0-9]+)T +([0-9]+)T +([0-9]+)%$")
	scanner := bufio.NewScanner(file)
	grid := make(map[int]map[int]Node)
	for scanner.Scan() {
		matches := regex.FindStringSubmatch(scanner.Text())
		if matches == nil {
			continue
		}
		var ints []int
		for _, v := range matches[1:] {
			i, _ := strconv.Atoi(v)
			ints = append(ints, i)
		}
		curr := Node{
			X:         ints[0],
			Y:         ints[1],
			Size:      ints[2],
			Used:      ints[3],
			Available: ints[4],
		}
		if len(grid) < ints[1]+1 {
			grid[ints[1]] = make(map[int]Node)
		}
		grid[ints[1]][ints[0]] = curr
	}

	viable := Part1{}
	for k, _ := range grid {
		for _, node := range grid[k] {
			for y := k; y < len(grid); y++ {
				for x := 0; x < len(grid[y]); x++ {
					if node.Used == 0 {
						continue
					}
					if node.Used <= grid[y][x].Available && !viable.Contains(node, grid[y][x]) {
						viable = append(viable, [2]Node{node, grid[y][x]})
					}
				}
			}
		}
	}
	fmt.Printf("Part 1: %v\n", len(viable))
}
