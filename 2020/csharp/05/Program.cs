using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Advent.Common;

List<Ticket> tickets = await FileLoader<Ticket>.Parse("../../../../../input/05.txt", line => new Ticket(line));

tickets = tickets.OrderByDescending(t => t.Id).ToList();
int partOne = tickets.First().Id;
Console.WriteLine($"Part 1: {partOne}");

int current = partOne;
int partTwo = -1;
foreach (Ticket ticket in tickets.Skip(1))
{
    if (ticket.Id != current - 1)
    {
        partTwo = current - 1;
        break;
    }
    current = ticket.Id;
}

Console.WriteLine($"Part 2: {partTwo}");

internal class Ticket
{
    public string Raw { get; init; }
    public int Row { get; private set; }
    public int Seat { get; private set; }
    public int Id => Row * 8 + Seat;
    
    public Ticket(string ticket)
    {
        Raw = ticket;
        Row = 0;
        Seat = 0;

        int rowLow = 0;
        int rowHigh = 127;
        int seatLow = 0;
        int seatHigh = 7;

        foreach (char direction in Raw)
        {
            int rowMove = (rowHigh-rowLow+1)/2;
            int seatMove = (seatHigh-seatLow+1)/2;
            switch (direction)
            {
                case 'F':
                    rowHigh -= rowMove;
                    break;
                case 'B':
                    rowLow += rowMove;
                    break;
                case 'L':
                    seatHigh -= seatMove;
                    break;
                case 'R':
                    seatLow += seatMove;
                    break;
                default:
                    throw new ArgumentException($"unknown direction {direction}");
            }
            
        }

        if (rowLow != rowHigh)
            throw new Exception($"low and high rows do not match: {rowLow}, {rowHigh}");
        if (seatLow != seatHigh)
            throw new Exception($"low and high seats do not match: {seatLow}, {seatHigh}");
        Row = rowHigh;
        Seat = seatHigh;
    }
}