package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type Firewall struct {
	scanners map[int]int
	bail     bool
}

func NewFirewall(list []string) Firewall {
	firewall := Firewall{
		scanners: make(map[int]int),
	}

	for _, v := range list {
		split := strings.Split(strings.TrimSpace(v), ": ")
		key, kerr := strconv.Atoi(split[0])
		size, serr := strconv.Atoi(split[1])

		if kerr != nil || serr != nil {
			panic("error converting values")
		}

		firewall.scanners[key] = size
	}

	return firewall
}

func (f *Firewall) Run(delay int) int {
	severity := 0
	for k, v := range f.scanners {
		if (k+delay)%((v-2)*2+2) == 0 {
			if f.bail {
				return -1
			}
			severity += k * v
		}
	}

	return severity
}

func main() {
	b, err := ioutil.ReadFile("../inputs/13.txt")
	if err != nil {
		panic(err)
	}

	list := strings.Split(strings.TrimSpace(string(b)), "\n")

	firewall := NewFirewall(list)

	fmt.Printf("Part 1: %v\n", firewall.Run(0))

	firewall.bail = true
	for delay := 1; ; delay++ {
		if delay < 0 {
			panic("int overflowwwwww")
		}
		if firewall.Run(delay) == 0 {
			fmt.Printf("Part 2: %v\n", delay)
			break
		}
	}
}
