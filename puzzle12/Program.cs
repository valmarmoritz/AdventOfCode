using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle12
{
    class Program
    {
        static void Main(string[] args)
        {
            bool learning = false;

            Dictionary<(int, int), char> Map = new Dictionary<(int, int), char>();

            (int, int) Size = (0, 0);
            
            Size = ReadMapInput(Map, learning);

            Dictionary<(int, int), char> ElevationMap = new Dictionary<(int, int), char>();

            (int, int) start = (0, 0);
            (int, int) end = (0, 0);

            foreach (var m in Map)
            {
                if (m.Value == 'S')
                {
                    start = (m.Key);
                    ElevationMap[m.Key] = 'a';
                }
                else if (m.Value == 'E')
                {
                    end = (m.Key);
                    ElevationMap[m.Key] = 'z';
                }
                else
                {
                    ElevationMap[m.Key] = m.Value;
                }
            }

            VisualizeMap(Map, Size);

            (int, int)[] Moves = new (int, int)[4];

            Moves[0] = (-1, 0);
            Moves[1] = (0, -1);
            Moves[2] = (1, 0);
            Moves[3] = (0, 1);

            // part 1 : shortest path from start
            int steps = ShortestPath(Map, Size, ElevationMap, Moves, start, end);

            Console.WriteLine();
            Console.WriteLine("Minimum steps: " + steps);
            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue to part 2...");
            Console.ReadLine();

            // part 2 : shortest path from any elevation 'a'
            (int, int) best_start = start;
            int shortest_path = int.MaxValue;

            int possible_starts = ElevationMap.Count(x => x.Value == 'a');
            Console.WriteLine("There are " + possible_starts + " possible starting points on the map.");

            int current_counter = 0;

            //foreach (var e in ElevationMap.Where(x => x.Value == 'a'))
            Parallel.ForEach(ElevationMap.Where(x => x.Value == 'a'), e =>
            {
                current_counter++;
                Console.WriteLine();
                Console.Write("[ " + current_counter + " / " + possible_starts + " ] ");

                int this_path = ShortestPath(Map, Size, ElevationMap, Moves, e.Key, end);
                if (this_path < shortest_path)
                {
                    best_start = e.Key;
                    shortest_path = this_path;
                }
            }
            // needed closing bracket for Parallel.ForEach
            );
                
            Console.WriteLine();
            Console.WriteLine("Shortest path from any elevation 'a' starts from " + best_start + " and has " + shortest_path + " steps.");
            Console.WriteLine();

        }

        // Dijkstra: https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm
        private static int ShortestPath(Dictionary<(int, int), char> map, (int, int) size, Dictionary<(int, int), char> elevationMap, (int, int)[] moves, (int, int) start, (int, int) end)
        {
            Dictionary<(int, int), bool> visited = new Dictionary<(int, int), bool>();
            Dictionary<(int, int), (int, int)> previous = new Dictionary<(int, int), (int, int)>();
            List<(int, int)> unvisited = new List<(int, int)>();
            Dictionary<(int, int), int> distance = new Dictionary<(int, int), int>();

            Console.WriteLine("Starting finding shortest path from " + start);

            // Mark all nodes unvisited. Create a set of all the unvisited nodes called the unvisited set.
            // Assign to every node a tentative distance value: set it to zero for our initial node and to infinity for all other nodes.
            for (int x = 0; x < size.Item1; x++)
            {
                for (int y = 0; y < size.Item2; y++)
                {
                    visited[(x, y)] = false;

                    unvisited.Add((x, y));

                    if ((x, y) == start)   // this is the starting postion
                    {
                        distance[(x, y)] = 0;
                    }
                    else
                    {
                        distance[(x, y)] = int.MaxValue;
                    }
                }
            }

            while (unvisited.Count > 0)
            {
                // take the one with minimal distance
                (int, int) from = distance.OrderBy(x => x.Value).FirstOrDefault(x => unvisited.Contains(x.Key)).Key;

                // if the target is achieved, we have found the shortest path
                if (from == end)
                {
                    break;
                }

                // For the current node, consider all of its unvisited neighbors ...
                foreach ((int, int) move in moves)
                {
                    (int, int) possibleMoveTo = (from.Item1 + move.Item1, from.Item2 + move.Item2);

                    if (
                        map.ContainsKey(possibleMoveTo)                                          // check that this move would not take us out of the map
                        && !visited[possibleMoveTo]                                              // check that we have not already visited this
                        && (elevationMap[possibleMoveTo].CompareTo(elevationMap[from]) < 2)      // check that the elevation of destination is at most 1 higher than current (only possible moves)
                        )
                    {
                        // ... and calculate their tentative distances through the current node.
                        int dist = distance[from] + 1;

                        // Compare the newly calculated tentative distance to the one currently assigned to the neighbor and assign it the smaller one.
                        if (dist < distance[possibleMoveTo]) 
                        {
                            distance[possibleMoveTo] = dist; 
                            
                            //
                            previous[possibleMoveTo] = from;
                        }
                    }
                }

                // When we are done considering all of the unvisited neighbors of the current node, mark the current node as visited and remove it from the unvisited set.
                visited[from] = true;
                unvisited.Remove(from);

                // If the destination node has been marked visited, then stop as we have found the shortest path.
                // ??
            }


            // Now we can read the shortest path from source to target by reverse iteration
            LinkedList<(int, int)> path = new LinkedList<(int, int)>();

            (int, int) u = end;

            if (previous.ContainsKey(u) || u == start)    // Do something only if the vertex is reachable
            {
                while (u != start)
                {
                    path.AddFirst(u);
                    if (previous.ContainsKey(u))
                    {
                        u = previous[u];
                    }
                    else    // no path found
                    {
                        Console.WriteLine("No path found from " + start);
                        return int.MaxValue;
                    }
                }
            }

            VisualizePath(path, size, start, end);

            Console.WriteLine("Shortest path length from " + start + " : " + path.Count);
            return path.Count;
        }

        private static void VisualizePath(LinkedList<(int, int)> path, (int, int) size, (int, int) start, (int, int) end)
        {
            Dictionary<(int, int), char> direction = new Dictionary<(int, int), char>();

            var current = start;
            LinkedListNode<(int, int)> next = path.First;

            while (current != end)
            {
                if (next.Value.Item1 < current.Item1) { direction[current] = '<'; }
                if (next.Value.Item1 > current.Item1) { direction[current] = '>'; }
                if (next.Value.Item2 < current.Item2) { direction[current] = '^'; }
                if (next.Value.Item2 > current.Item2) { direction[current] = 'V'; }

                current = next.Value;
                next = next.Next;
            }

            for (int y = 0; y < size.Item2; y++)
            {
                for (int x = 0; x < size.Item1; x++)
                {
                    if (direction.ContainsKey((x, y)))
                    {
                        Console.Write(direction[(x, y)]);
                    }
                    else if ((x, y) == end)
                    {
                        Console.Write('E');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void VisualizeMap(Dictionary<(int, int), char> map, (int, int) size)
        {
            for (int y = 0; y < size.Item2; y++)
            {
                for (int x = 0; x < size.Item1; x++)
                {
                    Console.Write(map[(x,y)]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static (int, int) ReadMapInput(Dictionary<(int, int), char> _map, bool learning)
        {
            string[] lines;
            // Read a text file line by line.
            if (learning) { lines = System.IO.File.ReadAllLines(@"puzzle12_learning.txt"); }
            else { lines = System.IO.File.ReadAllLines(@"puzzle12.txt"); }

            int x = 0;
            int y = 0;
            (int, int) _size = (0, 0);

            foreach (string line in lines)
            {
                x = 0;

                foreach (char c in line.ToCharArray())
                {
                    _map[(x, y)] = c;

                    x++;
                }
                
                y++;
                _size = (x, y);
            }

            return _size;
        }
    }
}
