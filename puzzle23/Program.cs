// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;

bool learning = false;
int learning_step = 2;
int part = 2;
// part 1: max_rounds = 10
// part 2: count rounds
int max_rounds = 10;

List<(int, int)> Elves = new List<(int, int)>();

ReadInput(Elves, learning, learning_step);

Dictionary<(int, int), char> Map = new Dictionary<(int, int), char>();

Map = CurrentMap(Elves);

if (learning) DrawMap(Map);

LinkedList<char> DirectionOrder = new LinkedList<char>();
DirectionOrder.AddLast('N');
DirectionOrder.AddLast('S');
DirectionOrder.AddLast('W');
DirectionOrder.AddLast('E');

bool NeedToMove = true;
int Rounds = 0;

if (part == 1)
{
    for (int round = 0; round < max_rounds; round++)
    {
        if (NeedToMove)
        {
            ProcessRound(Elves, Map, DirectionOrder, ref NeedToMove);
            Map = CurrentMap(Elves);

            if (learning) DrawMap(Map);
        }
    }

    int EmptyGroundTiles = CountEmptyGroundTiles(Map);

    Console.WriteLine();
    Console.WriteLine("Empty ground tiles in the rectangle: " + EmptyGroundTiles);
    Console.WriteLine();
}
else if (part == 2)
{
    while (NeedToMove)
    {
        ProcessRound(Elves, Map, DirectionOrder, ref NeedToMove);
        Rounds++;
        Map = CurrentMap(Elves);

        if (learning) DrawMap(Map);
    }

    Console.WriteLine();
    Console.WriteLine("Total rounds needed to figure out where the Elves need to go: " + Rounds);
    Console.WriteLine();
}


int CountEmptyGroundTiles(Dictionary<(int, int), char> map)
{
    int result = 0;

    int min_x;
    int max_x;
    int min_y;
    int max_y;

    var init_m = map.FirstOrDefault(x => x.Value == '#');

    min_x = init_m.Key.Item1;
    max_x = init_m.Key.Item1;
    min_y = init_m.Key.Item2;
    max_y = init_m.Key.Item2;

    foreach (var m in map.Where(x => x.Value == '#'))
    {
        if (m.Key.Item1 < min_x) min_x = m.Key.Item1;
        if (m.Key.Item1 > max_x) max_x = m.Key.Item1;
        if (m.Key.Item2 < min_y) min_y = m.Key.Item2;
        if (m.Key.Item2 > max_y) max_y = m.Key.Item2;
    }

    for (int x = min_x; x <= max_x; x++)
    {
        for (int y = min_y; y <= max_y; y++)
        {
            if (map[(x, y)] == '.') { result++; }
        }
    }

    return result;
}



void ProcessRound(List<(int, int)> elves, Dictionary<(int, int), char> map, LinkedList<char> directionOrder, ref bool needToMove)
{
    Dictionary<(int, int), int> proposedCounts = new Dictionary<(int, int), int>();
    Dictionary<int, (int, int)> proposedDestinations = new Dictionary<int, (int, int)>();

    MakeProposals(map, elves, directionOrder, proposedCounts, proposedDestinations);

    needToMove = (proposedCounts.Where(x => x.Value == 1).Count() > 0);

    if (needToMove)
    {
        MoveToDestinations(map, elves, proposedCounts, proposedDestinations);

        RotateDirectionOrder(DirectionOrder);
    }

    return;


    void RotateDirectionOrder(LinkedList<char> directionOrder)
    {
        directionOrder.AddLast(directionOrder.First());
        directionOrder.RemoveFirst();
    }

    void MoveToDestinations(Dictionary<(int, int), char> map, List<(int, int)> elves, Dictionary<(int, int), int> proposedCounts, Dictionary<int, (int, int)> proposedDestinations)
    {
        int elf_sq = 0;

        foreach (var d in proposedDestinations)
        {
            elf_sq = d.Key;
            int dest_x = d.Value.Item1;
            int dest_y = d.Value.Item2;

            if (proposedCounts[(dest_x, dest_y)] == 1)
            {
                elves[elf_sq] = (dest_x, dest_y);
            }
        }
    }

    void MakeProposals(Dictionary<(int, int), char> map, List<(int, int)> elves, LinkedList<char> directionOrder, Dictionary<(int, int), int> proposedCounts, Dictionary<int, (int, int)> proposedDestinations)
    {
        int elf_sq = 0;
        int count_empty_spaces;

        foreach (var elf in elves)
        {
            count_empty_spaces = 0;

            for (int x = elf.Item1 - 1; x <= elf.Item1 + 1; x++)
                for (int y = elf.Item2 - 1; y <= elf.Item2 + 1; y++)
                    if ((x, y) != (elf.Item1, elf.Item2))
                        if (map[(x, y)] == '.') count_empty_spaces++;

            if (count_empty_spaces < 8)
            {
                ProposeDestinations(map, elf, elf_sq, directionOrder, proposedCounts, proposedDestinations);
            }

            elf_sq++;
        }

        elf_sq = 0;
    }

    void ProposeDestinations(Dictionary<(int, int), char> map, (int, int) elf, int elf_sq, LinkedList<char> directionOrder, Dictionary<(int, int), int> proposedCounts, Dictionary<int, (int, int)> proposedDestinations)
    {
        bool space_found = false;

        foreach (var d in directionOrder)
        {
            if (!space_found)
            {
                switch (d)
                {
                    case 'N':
                        if (map[(elf.Item1 - 1, elf.Item2 - 1)] == '.' && map[(elf.Item1 - 1, elf.Item2)] == '.' && map[(elf.Item1 - 1, elf.Item2 + 1)] == '.')
                        {
                            space_found = true;
                            proposedDestinations[elf_sq] = (elf.Item1 - 1, elf.Item2);
                            if (proposedCounts.ContainsKey((elf.Item1 - 1, elf.Item2))) proposedCounts[(elf.Item1 - 1, elf.Item2)] += 1;
                            else proposedCounts[(elf.Item1 - 1, elf.Item2)] = 1;
                        }
                        break;
                    case 'S':
                        if (map[(elf.Item1 + 1, elf.Item2 - 1)] == '.' && map[(elf.Item1 + 1, elf.Item2)] == '.' && map[(elf.Item1 + 1, elf.Item2 + 1)] == '.')
                        {
                            space_found = true;
                            proposedDestinations[elf_sq] = (elf.Item1 + 1, elf.Item2);
                            if (proposedCounts.ContainsKey((elf.Item1 + 1, elf.Item2))) proposedCounts[(elf.Item1 + 1, elf.Item2)] += 1;
                            else proposedCounts[(elf.Item1 + 1, elf.Item2)] = 1;
                        }
                        break;
                    case 'W':
                        if (map[(elf.Item1 - 1, elf.Item2 - 1)] == '.' && map[(elf.Item1, elf.Item2 - 1)] == '.' && map[(elf.Item1 + 1, elf.Item2 - 1)] == '.')
                        {
                            space_found = true;
                            proposedDestinations[elf_sq] = (elf.Item1, elf.Item2 - 1);
                            if (proposedCounts.ContainsKey((elf.Item1, elf.Item2 - 1))) proposedCounts[(elf.Item1, elf.Item2 - 1)] += 1;
                            else proposedCounts[(elf.Item1, elf.Item2 - 1)] = 1;
                        }
                        break;
                    case 'E':
                        if (map[(elf.Item1 - 1, elf.Item2 + 1)] == '.' && map[(elf.Item1, elf.Item2 + 1)] == '.' && map[(elf.Item1 + 1, elf.Item2 + 1)] == '.')
                        {
                            space_found = true;
                            proposedDestinations[elf_sq] = (elf.Item1, elf.Item2 + 1);
                            if (proposedCounts.ContainsKey((elf.Item1, elf.Item2 + 1))) proposedCounts[(elf.Item1, elf.Item2 + 1)] += 1;
                            else proposedCounts[(elf.Item1, elf.Item2 + 1)] = 1;
                        }
                        break;
                    default:
                        space_found = false;
                        break;
                }
            }
        }
    }
}



void DrawMap(Dictionary<(int, int), char> map)
{
    int min_x;
    int max_x;
    int min_y;
    int max_y;

    var init_m = map.FirstOrDefault();

    min_x = init_m.Key.Item1;
    max_x = init_m.Key.Item1;
    min_y = init_m.Key.Item2;
    max_y = init_m.Key.Item2;

    foreach (var m in map)
    {
        if (m.Key.Item1 < min_x) min_x = m.Key.Item1;
        if (m.Key.Item1 > max_x) max_x = m.Key.Item1;
        if (m.Key.Item2 < min_y) min_y = m.Key.Item2;
        if (m.Key.Item2 > max_y) max_y = m.Key.Item2;
    }

    for (int x = min_x; x <= max_x; x++)
    {
        Console.WriteLine();
        for (int y = min_y; y <= max_y; y++)
        {
            Console.Write(map[(x, y)]);
        }
    }
 
    Console.WriteLine();
}

Dictionary<(int, int), char> CurrentMap(List<(int, int)> elves)
{
    Dictionary<(int, int), char> currentMap = new Dictionary<(int, int), char>();

    int min_x;
    int max_x;
    int min_y;
    int max_y;

    var init_e = elves[0];

    min_x = init_e.Item1 - 1;
    max_x = init_e.Item1 + 1;
    min_y = init_e.Item2 - 1;
    max_y = init_e.Item2 + 1;

    foreach (var e in elves)
    {
        if (e.Item1 <= min_x) min_x = e.Item1 - 1;
        if (e.Item1 >= max_x) max_x = e.Item1 + 1;
        if (e.Item2 <= min_y) min_y = e.Item2 - 1;
        if (e.Item2 >= max_y) max_y = e.Item2 + 1;
    }

    for (int x = min_x; x <= max_x; x++)
    {
        for (int y = min_y; y <= max_y; y++)
        {
            if (elves.Contains((x, y))) currentMap[(x, y)] = '#';
            else currentMap[(x, y)] = '.';
        }
    }

    return currentMap;
}

void ReadInput(List<(int, int)> elves, bool learning, int learning_step)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle23_learning" + learning_step + ".txt"); 
    else lines = System.IO.File.ReadAllLines(@"puzzle23.txt");

    int x = 0;
    int y = 0;

    foreach (string line in lines)
    {
        x += 1;
        y = 0;

        foreach (char c in line.ToCharArray())
        {
            y += 1;

            if (c == '#')
            {
                elves.Add((x, y));
            }
        }
    }
    
    return;
}
