using System;
using System.Collections.Generic;

static class Program
{
    static void Main(string[] args)
    {
        List<((int, int), int)> Points = new List<((int, int), int)>();

        int T = int.Parse(Console.ReadLine());

        for (int t = 0; t < T; t++)
        {
            var point = Console.ReadLine().Split(' ');
            Points.Add(((int.Parse(point[0]), int.Parse(point[1])), int.Parse(point[2])));
        }

        foreach (var p in Points)
        {
            int k = p.Item2;
            int result = ManhattanDistanceArea(k);

            Console.WriteLine(result);
        }
    }

    static int ManhattanDistanceArea(int i)
    {
        int result = 1;

        for (int x = 0; x < i; x++)
        {
            result += (x * 4);
        }
        return result;
    }
}

