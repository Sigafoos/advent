package main

import (
	"bufio"
	"fmt"
	"os"
	"regexp"
	"strconv"

	"github.com/Sigafoos/advent/grid"
	"github.com/Sigafoos/advent/position"
	"github.com/jroimartin/gocui"
)

var (
	sky   = grid.New()
	skyre = regexp.MustCompile(`^position=< *(-?\d+), +(-?\d+)> velocity=< *(-?\d+), +(-?\d+)>$`)
)

func main() {
	fp, err := os.Open("../../input/10.txt")
	if err != nil {
		panic(err)
	}
	scanner := bufio.NewScanner(fp)
	for scanner.Scan() {
		match := skyre.FindStringSubmatch(scanner.Text())
		if len(match) < 5 {
			panic(fmt.Sprintf("string does not match regex: %s", scanner.Text()))
		}
		x, err := strconv.Atoi(match[1])
		if err != nil {
			panic(err)
		}
		y, err := strconv.Atoi(match[2])
		if err != nil {
			panic(err)
		}
		vx, err := strconv.Atoi(match[3])
		if err != nil {
			panic(err)
		}
		vy, err := strconv.Atoi(match[4])
		if err != nil {
			panic(err)
		}

		p := position.New(x, y)
		p.Velocity = [2]int{vx, vy}
		sky.Add(p)
	}
	cui, err := gocui.NewGui(gocui.OutputNormal)
	if err != nil {
		panic(err)
	}
	defer cui.Close()

	cui.SetManagerFunc(layout)

	if err := cui.SetKeybinding("", gocui.KeyCtrlC, gocui.ModNone, quit); err != nil {
		panic(err)
	}

	if err := cui.SetKeybinding("", gocui.KeySpace, gocui.ModNone, progress); err != nil {
		panic(err)
	}
	if err := cui.MainLoop(); err != nil && err != gocui.ErrQuit {
		panic(err)
	}
}

func layout(g *gocui.Gui) error {
	maxX, maxY := g.Size()
	if v, err := g.SetView("mail", 0, 0, maxX-1, maxY-1); err != nil {
		if err != gocui.ErrUnknownView {
			return err
		}
		v.Title = fmt.Sprintf("Iteration %v", sky.Iteration)
		sky.Update(true)
		fmt.Fprintf(v, "%v", sky.Sprint(sky.Min(), sky.Max()))
		if _, err = g.SetCurrentView("mail"); err != nil {
			panic(err)
		}
	}
	return nil
}

func quit(g *gocui.Gui, v *gocui.View) error {
	return gocui.ErrQuit
}

func progress(g *gocui.Gui, v *gocui.View) error {
	v.Clear()
	v.Title = fmt.Sprintf("Iteration %v", sky.Iteration)
	sky.Update(false) // it could update to fail the printable test, causing an infinite loop
	fmt.Fprintf(v, "%s", sky.Sprint(sky.Min(), sky.Max()))
	return nil
}
