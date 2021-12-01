using System;
using Advent.Common;

Grid<Seat> grid = await Grid<Seat>.FromFile("../../../../../input/11-example.txt", c => new Seat(c));

public class Seat
{
    // null is floor
    public bool? Occupied { get; set; }

    public Seat(char c)
    {
        Occupied = c switch
        {
            '#' => true,
            'L' => false,
            _ => Occupied
        };
    }
}
