package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type Header struct {
	Children int
	Metadata int
}

type Node struct {
	Header   *Header
	Children []*Node
	Metadata []int
	Length   int
}

func (n *Node) SumMetadata() int {
	s := 0
	for _, v := range n.Metadata {
		s += v
	}

	for _, c := range n.Children {
		s += c.SumMetadata()
	}
	return s
}

func (n *Node) Value() int {
	if len(n.Children) == 0 {
		return n.SumMetadata()
	}
	s := 0
	for _, v := range n.Metadata {
		if v == 0 || v-1 >= len(n.Children) {
			continue
		}
		s += n.Children[v-1].Value()
	}

	return s
}

func NewNode(list []int) *Node {
	header := &Header{
		Children: list[0],
		Metadata: list[1],
	}

	n := &Node{
		Header: header,
		Length: 2,
	}

	for i := 0; i < header.Children; i++ {
		child := NewNode(list[n.Length:])
		n.Length += child.Length
		n.Children = append(n.Children, child)
	}

	n.Metadata = list[n.Length:(n.Length + n.Header.Metadata)]
	n.Length += n.Header.Metadata

	return n
}

func main() {
	raw, err := ioutil.ReadFile("../../input/08.txt")
	if err != nil {
		panic(err)
	}
	var list []int
	for _, v := range strings.Split(strings.TrimSpace(string(raw)), " ") {
		i, err := strconv.Atoi(v)
		if err != nil {
			panic(err)
		}
		list = append(list, i)
	}

	tree := NewNode(list)

	fmt.Printf("Part 1: %v\n", tree.SumMetadata())
	fmt.Printf("Part 2: %v\n", tree.Value())
}
