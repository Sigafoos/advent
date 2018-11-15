package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type Duet struct {
	registers map[string]int
	sound     int
}

func NewDuet() *Duet {
	return &Duet{
		registers: make(map[string]int),
	}
}

func (d *Duet) Val(x string) int {
	if value, err := strconv.Atoi(x); err == nil {
		return value
	} else {
		return d.registers[x]
	}
}

func (d *Duet) Snd(x string) {
	d.sound = d.Val(x)
}

func (d *Duet) Set(x, y string) {
	d.registers[x] = d.Val(y)
}

func (d *Duet) Add(x, y string) {
	d.registers[x] += d.Val(y)
}

func (d *Duet) Mul(x, y string) {
	d.registers[x] *= d.Val(y)
}

func (d *Duet) Mod(x, y string) {
	d.registers[x] = d.registers[x] % d.Val(y)
}

func (d *Duet) Rcv(x string) (int, error) {
	val := d.Val(x)
	if val == 0 {
		return 0, fmt.Errorf("x is 0")
	}
	return d.sound, nil
}

func (d *Duet) Operate(instructions []string) int {
	for i := 0; ; {
		if i >= len(instructions) {
			return -1
		}

		line := strings.Split(instructions[i], " ")

		switch line[0] {
		case "snd":
			d.Snd(line[1])
		case "set":
			d.Set(line[1], line[2])
		case "add":
			d.Add(line[1], line[2])
		case "mul":
			d.Mul(line[1], line[2])
		case "mod":
			d.Mod(line[1], line[2])
		case "rcv":
			if i, err := d.Rcv(line[1]); err == nil {
				return i
			}
		case "jgz":
			if d.Val(line[1]) > 0 {
				i += d.Val(line[2])
			} else {
				i++
			}
		}

		if line[0] != "jgz" {
			i++
		}
	}
}

func main() {
	file, err := os.Open("../../inputs/18.txt")
	if err != nil {
		panic(err)
	}
	var instructions []string
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		instructions = append(instructions, scanner.Text())
	}
	d := NewDuet()

	part1 := d.Operate(instructions)
	fmt.Printf("Part 1: %v\n", part1)
}
