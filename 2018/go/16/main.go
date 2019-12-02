package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

var re = regexp.MustCompile(`(\d),? (\d),? (\d),? (\d)`)

type Device struct {
	Register [4]int
}

func (d *Device) Addr(a, b, c int) {
	d.Register[c] = d.Register[a] + d.Register[b]
}

func (d *Device) Addi(a, b, c int) {
	d.Register[c] = d.Register[a] + b
}

func (d *Device) Mulr(a, b, c int) {
	d.Register[c] = d.Register[a] * d.Register[b]
}

func (d *Device) Muli(a, b, c int) {
	d.Register[c] = d.Register[a] * b
}

func (d *Device) Banr(a, b, c int) {
	d.Register[c] = d.Register[a] & d.Register[b]
}

func (d *Device) Bani(a, b, c int) {
	d.Register[c] = d.Register[a] & b
}

func (d *Device) Borr(a, b, c int) {
	d.Register[c] = d.Register[a] | d.Register[b]
}

func (d *Device) Bori(a, b, c int) {
	d.Register[c] = d.Register[a] | b
}

func (d *Device) Setr(a, b, c int) {
	d.Register[c] = d.Register[a]
}

func (d *Device) Seti(a, b, c int) {
	d.Register[c] = a
}

func (d *Device) Gtir(a, b, c int) {
	if a > d.Register[b] {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

func (d *Device) Gtri(a, b, c int) {
	if d.Register[a] > b {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

func (d *Device) Gtrr(a, b, c int) {
	if d.Register[a] > d.Register[b] {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

func (d *Device) Eqir(a, b, c int) {
	if a == d.Register[b] {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

func (d *Device) Eqri(a, b, c int) {
	if d.Register[a] == b {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

func (d *Device) Eqrr(a, b, c int) {
	if d.Register[a] == d.Register[b] {
		d.Register[c] = 1
	} else {
		d.Register[c] = 0
	}
}

type TestCase struct {
	Before       [4]int
	After        [4]int
	Instructions [4]int
}

func main() {
	fp, err := os.Open("../../input/16.txt")
	if err != nil {
		fmt.Println(err)
		os.Exit(1)
	}
	defer fp.Close()
	scanner := bufio.NewScanner(fp)

	var tests []TestCase
	test := TestCase{}

	for i := 0; scanner.Scan(); i++ {
		if scanner.Text() == "" {
			tests = append(tests, test)
			test = TestCase{}
			continue
		}

		var vals [4]int
		matches := re.FindStringSubmatch(scanner.Text())
		for j := 0; j < 4; j++ {
			val, err := strconv.Atoi(matches[j+1])
			if err != nil {
				fmt.Println(err)
				os.Exit(1)
			}
			vals[j] = val
		}

		switch i % 4 {
		case 0:
			test.Before = vals
		case 1:
			test.Instructions = vals
		case 2:
			test.After = vals
		}
	}

	codes := [10]map[string]bool
	for i := 0; i < 10; i++ {
		code := make(map[string]bool)

		codes[i] = code
	}

	multis := 0
	for _, test := range tests {
		passes := 0

		d := Device{Register: test.Before}
		d.Addr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Addi(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Mulr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Muli(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Banr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Bani(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Borr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Bori(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Setr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Seti(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Gtir(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Gtri(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Gtrr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Eqir(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Eqri(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		d = Device{Register: test.Before}
		d.Eqrr(test.Instructions[1], test.Instructions[2], test.Instructions[3])
		if d.Register == test.After {
			passes++
		}

		if passes >= 3 {
			multis++
		}
	}

	// 319 (and 320) is too low
	fmt.Printf("Part 1: %v\n", multis)
}
