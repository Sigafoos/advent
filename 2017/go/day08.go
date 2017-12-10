package main

import (
	"fmt"
	"io/ioutil"
	"regexp"
	"strconv"
	"strings"
)

type Register struct {
	register map[string]int
}

func NewRegister() *Register {
	return &Register{
		register: make(map[string]int),
	}
}

func (r *Register) Get(key string) int {
	val, exists := r.register[key]
	if !exists {
		r.Set(key, 0)
		return 0
	}
	return val
}

func (r *Register) Set(key string, val int) {
	r.register[key] = val
}

func (r *Register) Dump() {
	for k, v := range r.register {
		fmt.Printf("%s: %v\n", k, v)
	}
}

func (r *Register) Max() (string, int) {
	max := -2147483648
	maxKey := ""
	for k, v := range r.register {
		if v > max {
			max = v
			maxKey = k
		}
	}
	return maxKey, max
}

func Eval(a int, operator string, b int) bool {
	switch operator {
	case "<":
		return a < b
	case "<=":
		return a <= b
	case "==":
		return a == b
	case ">=":
		return a >= b
	case ">":
		return a > b
	case "!=":
		return a != b
	}
	panic(fmt.Sprintf("%s is not a valid operator", operator))
}

func main() {
	b, err := ioutil.ReadFile("../inputs/08.txt")
	if err != nil {
		panic(err)
	}

	pattern := regexp.MustCompile("^([a-z]+) ([a-z]+) (-?[0-9]+) if ([a-z]+) ([><!=]=?) (-?[0-9]+)$")
	instructions := strings.Split(strings.TrimSpace(string(b)), "\n")
	registers := NewRegister()
	var max int

	for _, instruction := range instructions {
		raw := strings.TrimSpace(instruction)
		parsed := pattern.FindStringSubmatch(raw)
		if parsed == nil {
			panic(fmt.Sprintf("didn't match pattern: %s", strings.TrimSpace(instruction)))
		}

		b, _ := strconv.Atoi(parsed[6])
		if Eval(registers.Get(parsed[4]), parsed[5], b) {
			reg := registers.Get(parsed[1])
			amount, _ := strconv.Atoi(parsed[3])
			if parsed[2] == "inc" {
				reg += amount
			} else {
				reg -= amount
			}
			registers.Set(parsed[1], reg)
		}
		_, v := registers.Max()
		if v > max {
			max = v
		}
	}

	_, v := registers.Max()
	fmt.Printf("Part 1: %v\n", v)
	fmt.Printf("Part 2: %v\n", max)
}
