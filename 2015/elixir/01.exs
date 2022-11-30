part1 = fn x, acc ->
  case x do
    "(" -> acc + 1
    ")" -> acc - 1
  end
end

File.read!("../input/input1.txt")
|> String.graphemes
|> Enum.reduce(0, part1)
|> then(fn result -> "Part 1: #{result}" end)
|> IO.puts

part2 = fn x, acc ->
  {v, i} = acc
  cond do
    v < 0 -> {:halt, acc}
    x == "(" -> {:cont, {v + 1, i + 1}}
    x == ")" -> {:cont, {v - 1, i + 1}}
  end
end

File.read!("../input/input1.txt")
|> String.graphemes
|> Enum.reduce_while({0, 0}, part2)
|> elem(1)
|> then(fn result -> "Part 2: #{result}" end)
|> IO.puts
