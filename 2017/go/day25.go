package main

import (
	"fmt"
	"os"
	"strconv"
)

// the specific implementation from my input
type Turing struct {
	tape     []bool
	state    rune
	position int
}

func NewTuring() *Turing {
	return &Turing{
		tape:     []bool{false},
		state:    'A',
		position: 0,
	}
}

func (t *Turing) Step() {
	switch t.state {
	case 'A':
		t.A()
	case 'B':
		t.B()
	case 'C':
		t.C()
	case 'D':
		t.D()
	case 'E':
		t.E()
	case 'F':
		t.F()
	default:
		panic(fmt.Sprintf("unknown state %#U", t.state))
	}
}

func (t *Turing) Get() bool {
	if t.position < 0 {
		for ; t.position < 0; t.position++ {
			t.tape = append([]bool{false}, t.tape...)
		}
	} else {
		for t.position >= len(t.tape) {
			t.tape = append(t.tape, false)
		}
	}
	return t.tape[t.position]
}

func (t *Turing) Set(v bool) {
	t.tape[t.position] = v
}

func (t *Turing) A() {
	if !t.Get() {
		t.Set(true)
		t.position++
		t.state = 'B'
	} else {
		t.Set(false)
		t.position--
		t.state = 'E'
	}
}

func (t *Turing) B() {
	if !t.Get() {
		t.Set(true)
		t.position--
		t.state = 'C'
	} else {
		t.Set(false)
		t.position++
		t.state = 'A'
	}
}

func (t *Turing) C() {
	if !t.Get() {
		t.Set(true)
		t.position--
		t.state = 'D'
	} else {
		t.Set(false)
		t.position++
		// same state
	}
}

func (t *Turing) D() {
	if !t.Get() {
		t.Set(true)
		t.position--
		t.state = 'E'
	} else {
		t.Set(false)
		t.position--
		t.state = 'F'
	}
}

func (t *Turing) E() {
	if !t.Get() {
		t.Set(true)
		t.position--
		t.state = 'A'
	} else {
		// same value
		t.position--
		t.state = 'C'
	}
}

func (t *Turing) F() {
	if !t.Get() {
		t.Set(true)
		t.position--
		t.state = 'E'
	} else {
		// same value
		t.position++
		t.state = 'A'
	}
}

func (t *Turing) Checksum() int {
	count := 0
	for _, v := range t.tape {
		if v {
			count++
		}
	}
	return count
}

func main() {
	if len(os.Args) < 2 {
		panic("pass a number of steps, dummy")
	}
	steps, err := strconv.Atoi(os.Args[1])
	if err != nil {
		panic(err)
	}

	t := NewTuring()
	for i := 0; i < steps; i++ {
		t.Step()
	}
	fmt.Printf("Part 1: %v\n", t.Checksum())
}
