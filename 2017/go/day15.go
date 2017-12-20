package main

import (
	"fmt"
	"os"
	"strconv"
)

func Generate1(val uint64, multiple uint64, c chan uint64) {
	for {
		val = (val * multiple) % 2147483647
		//fmt.Println(val)
		c <- val
	}
}

func Generate2(val uint64, multiple uint64, factor uint64, c chan uint64) {
	for {
		val = (val * multiple) % 2147483647
		if val%factor == 0 {
			c <- val
		}
	}
}

func main() {
	if len(os.Args) != 3 {
		fmt.Println("fatal error: you must pass in two command-line arguments for generators A and B")
		os.Exit(1)
	}

	astart, aerr := strconv.Atoi(os.Args[1])
	bstart, berr := strconv.Atoi(os.Args[2])
	if aerr != nil || berr != nil {
		fmt.Println("I don't think you gave me ints")
		os.Exit(1)
	}

	mask := uint64(65535) // 2^16-1

	a := make(chan uint64)
	b := make(chan uint64)
	go Generate1(uint64(astart), 16807, a)
	go Generate1(uint64(bstart), 48271, b)

	matches := 0
	for i := 0; i < 40000000; i++ {
		va := <-a
		vb := <-b

		if va&mask == vb&mask {
			matches++
		}
	}
	fmt.Printf("Part 1: %v\n", matches)

	a2 := make(chan uint64)
	b2 := make(chan uint64)
	go Generate2(uint64(astart), 16807, 4, a2)
	go Generate2(uint64(bstart), 48271, 8, b2)

	matches = 0
	for i := 0; i < 5000000; i++ {
		va := <-a2
		vb := <-b2

		if va&mask == vb&mask {
			matches++
		}
	}
	fmt.Printf("Part 2: %v\n", matches)
}
