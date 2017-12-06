package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type Bank [16]int

func (b *Bank) Max() (key, value int) {
	value = 0
	for k, v := range b {
		if v > value {
			key = k
			value = v
		}
	}

	return key, value
}

func (b *Bank) Iterate() {
	k, blocks := b.Max()
	b[k] = 0

	for ; blocks > 0; blocks-- {
		k = (k + 1) % len(b)
		b[k]++
	}
}

type History struct {
	banks []Bank
}

func (h *History) Append(b Bank) {
	h.banks = append(h.banks, b)
}

func (h *History) Contains(b Bank) bool {
	for _, stored := range h.banks {
		if b == stored {
			return true
		}
	}
	return false
}

func main() {
	bank := Bank{}
	b, err := ioutil.ReadFile("../inputs/06.txt")
	if err != nil {
		panic(err)
	}

	parsed := strings.Split(strings.TrimSpace(string(b)), "\t")
	for k, v := range parsed {
		i, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		bank[k] = i
	}

	h := History{}
	i := 0
	for ; !h.Contains(bank); i++ {
		h.Append(bank)
		bank.Iterate()
	}
	fmt.Printf("Part 1: %v\n", i)

	h = History{}
	i = 0
	for ; !h.Contains(bank); i++ {
		h.Append(bank)
		bank.Iterate()
	}
	fmt.Printf("Part 2: %v\n", i)
}
