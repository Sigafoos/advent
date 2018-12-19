package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
)

var LineRegexp = regexp.MustCompile(`^Step (.) must be finished before step (.) can begin\.$`)

func NewStep(ID string) *Step {
	return &Step{
		ID:        ID,
		Satisfied: true,
	}
}

type Step struct {
	ID         string
	Dependents []*Step
	Parents    []*Step
	Satisfied  bool
	Done       bool
}

func (s *Step) DependsOn(p *Step) {
	s.Satisfied = false
	s.Parents = append(s.Parents, p)
	p.Dependents = append(p.Dependents, s)
}

func (s *Step) Do() {
	s.Done = true
	for _, c := range s.Dependents {
		c.CheckParents()
	}
}

func (s *Step) AsNumeric() int {
	return int([]rune(s.ID)[0]) - 64
}

func (s *Step) CheckParents() {
	if s.Satisfied {
		return
	}

	s.Satisfied = true
	for _, p := range s.Parents {
		if !p.Satisfied || !p.Done {
			s.Satisfied = false
			return
		}
	}
}

func NewInstructions() *Instructions {
	return &Instructions{
		steps: make(map[string]*Step),
	}
}

type Instructions struct {
	steps map[string]*Step
}

func (i *Instructions) AddStep(line string) {
	m := LineRegexp.FindStringSubmatch(line)
	parent, exists := i.Step(m[1])
	if !exists {
		parent = NewStep(m[1])
		i.steps[m[1]] = parent
	}
	dependent, exists := i.Step(m[2])
	if !exists {
		dependent = NewStep(m[2])
		i.steps[m[2]] = dependent
	}

	dependent.DependsOn(parent)
}

func (i *Instructions) Step(ID string) (*Step, bool) {
	s, ok := i.steps[ID]
	return s, ok
}

func (i *Instructions) ValidSteps() []*Step {
	var valid []*Step
	for _, s := range i.steps {
		if s.Satisfied && !s.Done {
			valid = append(valid, s)
		}
	}

	return valid
}

func (i *Instructions) TraversibleOrder() string {
	var order string

	for valid := i.ValidSteps(); len(valid) > 0; {
		var step *Step
		for _, v := range valid {
			if step == nil || v.ID < step.ID {
				step = v
			}
		}
		order = fmt.Sprintf("%s%s", order, step.ID)
		step.Do()
		valid = i.ValidSteps()
	}
	return order
}

func (i *Instructions) ThreadedTime(n int) int {
	var workers []*Worker

	for j := 0; j < n; j++ {
		workers = append(workers, &Worker{})
	}

	t := 0
	for ; ; t++ {
		break
	}

	return t
}

type Worker struct {
	step    *Step
	started int
}

func main() {
	p1 := NewInstructions()
	p2 := NewInstructions()

	fp, err := os.Open("../../input/07-example.txt")
	if err != nil {
		panic(err)
	}
	scanner := bufio.NewScanner(fp)
	for scanner.Scan() {
		p1.AddStep(scanner.Text())
		p2.AddStep(scanner.Text())
	}

	fmt.Printf("Part 1: %s\n", p1.TraversibleOrder())
	fmt.Printf("Part 2: %v\n", p2.ThreadedTime(2))
}
