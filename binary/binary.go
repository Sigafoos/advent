package binary

import (
	"encoding/hex"
	"fmt"
	"math"
)

// StringHexString accepts a hexadecimal string and returns its representation as a string of 1s and 0s.
func StringHexString(s string) string {
	var binary string
	b, _ := hex.DecodeString(s)
	for _, v := range b {
		binary = fmt.Sprintf("%s%s", binary, String8(v))
	}
	return binary
}

// String8 accepts a uint8 number and returns its representation as a string of 1s and 0s.
func String8(n uint8) string {
	var binary string
	var s string
	for i := 0; i < 8; i++ {
		p := uint8(math.Pow(2, float64(i)))
		if n&p == p {
			s = "1"
		} else {
			s = "0"
		}
		binary = fmt.Sprintf("%s%s", s, binary)
	}
	return binary
}
