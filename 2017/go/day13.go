package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type Scanner struct {
	Position int
	Depth    int
	Down     bool
}

func (s *Scanner) Move() {
	if s.Down {
		if s.Position+1 == s.Depth {
			s.Down = false
			s.Position--
		} else {
			s.Position++
		}
	} else {
		if s.Position == 0 {
			s.Down = true
			s.Position++
		} else {
			s.Position--
		}
	}
}

type Firewall struct {
	max      int
	scanners map[int]*Scanner
	bail     bool
}

func NewFirewall(list []string) Firewall {
	firewall := Firewall{
		scanners: make(map[int]*Scanner),
	}

	for _, v := range list {
		split := strings.Split(strings.TrimSpace(v), ": ")
		key, kerr := strconv.Atoi(split[0])
		size, serr := strconv.Atoi(split[1])

		if kerr != nil || serr != nil {
			panic("error converting values")
		}

		firewall.Add(key, Scanner{
			Position: 0,
			Depth:    size,
			Down:     true,
		})
		firewall.max = key // assumes they're in ascending order
	}

	return firewall
}

func (f *Firewall) Add(i int, s Scanner) {
	f.scanners[i] = &s
}

func (f *Firewall) Scanner(i int) *Scanner {
	if scanner, ok := f.scanners[i]; ok {
		return scanner
	}
	return nil
}

func (f *Firewall) Advance() {
	for k, _ := range f.scanners {
		f.scanners[k].Move()
	}
}

func (f *Firewall) Run(delay int) int {
	for i := 0; i < delay; i++ {
		f.Advance()
	}

	severity := 0
	for i := 0; i <= f.max; i++ {
		scanner := f.Scanner(i)
		if scanner != nil && scanner.Position == 0 {
			if f.bail {
				fmt.Printf("caught at column %v\n\n", i)
				return -1
			}

			severity += i * scanner.Depth
		}
		f.Advance()
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

	//fmt.Printf("Part 1: %v\n", firewall.Run(0))
	c := 0
	for k, v := range firewall.scanners {
		if k%((v.Depth-2)*2+2) == 0 {
			c += k * v.Depth
		}
	}
	fmt.Printf("Part 1: %v\n", c)

	/*
		for delay := 0; delay < 50; delay++ {
			fmt.Printf("trying delay %v...\n", delay)
			firewall = NewFirewall(list)
			firewall.bail = true

			if firewall.Run(delay) == 0 {
				fmt.Printf("Part 2: %v\n", delay)
				break
			}
		}
	*/
}
