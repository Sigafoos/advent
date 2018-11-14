package main

import (
	"bufio"
	"fmt"
	"os"
	"os/signal"
	"strconv"
	"strings"
)

type Duet struct {
	registers map[string]int
	from      chan int
	to        chan int
	sent      int
	id        int
}

func NewDuet(id int, from, to chan int) *Duet {
	d := Duet{
		registers: make(map[string]int),
		from:      from,
		to:        to,
	}
	d.registers["p"] = id
	d.id = id
	return &d
}

func (d *Duet) Val(x string) int {
	if value, err := strconv.Atoi(x); err == nil {
		return value
	} else {
		return d.registers[x]
	}
}

func (d *Duet) Snd(x string) {
	d.from <- d.Val(x)
	d.sent++
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

func (d *Duet) Rcv(x string) error {
	fmt.Printf("program %v is waiting...\n", d.id)
	if v, ok := <-d.to; !ok {
		return fmt.Errorf("channel is closed")
	} else {
		d.registers[x] = v
	}
	fmt.Printf("program %v received a value\n", d.id)
	return nil
}

func (d *Duet) Operate(instructions []string) {
	// recover from panicking after sending on a closed channel
	defer func() {
		if r := recover(); r != nil {
			return
		}
	}()

	for i := 0; ; {
		if i >= len(instructions) {
			fmt.Printf("program %v is exiting!\n", d.id)
			return
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
			if err := d.Rcv(line[1]); err != nil {
				return
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

func (d *Duet) Sent() int {
	return d.sent
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

	from0 := make(chan int, 1000)
	from1 := make(chan int, 1000)
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, os.Interrupt)

	d0 := NewDuet(0, from0, from1)
	d1 := NewDuet(1, from1, from0)

	go d0.Operate(instructions)
	go d1.Operate(instructions)

	<-quit
	close(from0)
	close(from1)
	fmt.Printf("\n\nPart 2: %v\n", d1.Sent())
}
