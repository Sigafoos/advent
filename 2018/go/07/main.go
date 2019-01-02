package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
	"sort"
)

var LineRegexp = regexp.MustCompile(`^Step (.) must be finished before step (.) can begin\.$`)

const (
	Unsatisfied = iota
	Satisfied   = iota
	InProgress  = iota
	Done        = iota
)

func NewStep(ID string) *Step {
	return &Step{
		ID:     ID,
		Status: Satisfied,
	}
}

type Step struct {
	ID         string
	Dependents []*Step
	Parents    []*Step
	Status     int
}

func (s *Step) DependsOn(p *Step) {
	s.Status = Unsatisfied
	s.Parents = append(s.Parents, p)
	p.Dependents = append(p.Dependents, s)
}

func (s *Step) Do() {
	s.Status = Done
	for _, c := range s.Dependents {
		c.CheckParents()
	}
}

func (s *Step) AsNumeric() int {
	return int([]rune(s.ID)[0]) - 64
}

func (s *Step) CheckParents() {
	if s.Status > Satisfied {
		return
	}

	s.Status = Satisfied
	for _, p := range s.Parents {
		if p.Status < Done {
			s.Status = Unsatisfied
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
		if s.Status == Satisfied {
			valid = append(valid, s)
		}
	}

	sort.Slice(valid, func(i, j int) bool {
		return valid[i].ID < valid[j].ID
	})
	return valid
}

func (i *Instructions) TraversibleOrder() string {
	var order string

	for valid := i.ValidSteps(); len(valid) > 0; {
		step := valid[0]
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
	for valid := i.ValidSteps(); len(valid) > 0; t++ {
		for _, w := range workers {
			if w.step == nil || w.step.Status == Done {
				// get a new one
				/*
					w.step = valid[0]
					w.step.Status = InProgress
					valid = i.ValidSteps()
				*/
				fmt.Printf("%+v\n", valid)
				// HANDLE NO VALID BUT SOME IN PROGRESS
			}
		}
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
