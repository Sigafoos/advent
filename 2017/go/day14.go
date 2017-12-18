package main

import (
	"encoding/hex"
	"fmt"
	"math/bits"
	"os"

	"./knothash"
)

func main() {
	key := os.Args[1]

	count := 0
	for i := 0; i < 128; i++ {
		row := knothash.HashString(fmt.Sprintf("%s-%v", key, i))
		b, _ := hex.DecodeString(row)
		for _, v := range b {
			count += bits.OnesCount8(uint8(v))
		}
	}
	fmt.Printf("Part 1: %v\n", count)
}
