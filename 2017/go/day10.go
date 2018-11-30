package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"

	"github.com/Sigafoos/advent/2017/go/knothash"
)

func main() {
	b, err := ioutil.ReadFile("../inputs/10.txt")
	if err != nil {
		panic(err)
	}
	trimmed := strings.TrimSpace(string(b))

	// *** Part 1 ***
	raw := strings.Split(trimmed, ",")
	lengths := make([]int, len(raw))
	for k, v := range raw {
		i, _ := strconv.Atoi(v)
		lengths[k] = i
	}

	knot := knothash.NewKnotHash(256, lengths)
	knot.TwistAll()
	knot.Home()

	vals := knot.Values()
	first, _ := strconv.Atoi(vals[0])
	second, _ := strconv.Atoi(vals[1])
	fmt.Printf("Part 1: %v\n", first*second)

	// *** Part 2 ***
	fmt.Printf("Part 2: %s\n", knothash.HashString(trimmed))
}
