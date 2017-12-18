package main

import (
	"container/ring"
	"encoding/hex"
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

type KnotHash struct {
	list     *ring.Ring
	skip     int
	lengths  []int
	position int
}

func (k *KnotHash) twist(length int) {
	k.list = k.list.Move(length)
	reversed := []int{}

	for i := length; i > 0; i-- {
		k.list = k.list.Prev()
		reversed = append(reversed, k.list.Value.(int))
	}

	for i := 0; i < length; i++ {
		k.list.Value = reversed[i]
		k.list = k.list.Next()
	}

	k.list = k.list.Move(k.skip)
	k.position += length + k.skip
	k.skip++
}

func (k *KnotHash) Home() {
	k.list = k.list.Move(-k.position)
}

func (k *KnotHash) TwistAll() {
	for _, v := range k.lengths {
		k.twist(v)
	}
}

func (k *KnotHash) Hash() string {
	for i := 0; i < 64; i++ {
		k.TwistAll()
	}
	k.Home()

	dense := k.denseHash()

	return hex.EncodeToString(dense)
}

func (k *KnotHash) denseHash() []byte {
	hash := make([]byte, 16)

	for i := 0; i < 16; i++ {
		block := k.list.Value.(int)
		for j := 1; j < 16; j++ {
			k.list = k.list.Next()
			block = block ^ k.list.Value.(int)
		}
		k.list = k.list.Next()

		hash[i] = byte(block)
	}

	return hash
}

func (k *KnotHash) Values() []string {
	vals := make([]string, k.list.Len())
	for i := 0; i < k.list.Len(); i++ {
		v := strconv.Itoa(k.list.Value.(int))
		vals[i] = v
		k.list = k.list.Next()
	}

	return vals
}

func NewKnotHash(size int, lengths []int) KnotHash {
	list := ring.New(size)
	for i := 0; i < list.Len(); i++ {
		list.Value = i
		list = list.Next()
	}

	return KnotHash{
		list:     list,
		skip:     0,
		position: 0,
		lengths:  lengths,
	}
}

func HashString(input string) string {
	input = strings.TrimSpace(input)

	b := []byte(input)
	b = append(b, 17, 31, 73, 47, 23)
	lengths := make([]int, len(b))
	for k, v := range b {
		lengths[k] = int(v)
	}
	knot := NewKnotHash(256, lengths)
	return knot.Hash()
}

func main() {
	b, err := ioutil.ReadFile("../inputs/10.txt")
	if err != nil {
		panic(err)
	}
	trimmed := strings.TrimSpace(string(b))

	// *** Part 1 ***
	raw := strings.Split(trimmed, ",")
	lengths := make([]int, len(raw))
	for k, v := range raw {
		i, _ := strconv.Atoi(v)
		lengths[k] = i
	}

	knot := NewKnotHash(256, lengths)
	knot.TwistAll()
	knot.Home()

	vals := knot.Values()
	first, _ := strconv.Atoi(vals[0])
	second, _ := strconv.Atoi(vals[1])
	fmt.Printf("Part 1: %v\n", first*second)

	// *** Part 2 ***
	fmt.Printf("Part 2: %s\n", HashString(trimmed))
}
