// See https://aka.ms/new-console-template for more information

bool learning = false;

Dictionary<(int, int), bool> Cave = ReadInput(learning);
Dictionary<(int, int), bool> Sand = new Dictionary<(int, int), bool>();

(int, int) source = (500, 0);

if (learning)
{
    DrawCave(Cave, Sand, source, null);
}

bool last_sand_came_to_rest = true;

while (last_sand_came_to_rest)
{
    // produce a new sand and let it come to rest or fall off to the void
    last_sand_came_to_rest = LetNewSandFall(Cave, Sand, source);
}

Console.WriteLine();
Console.WriteLine($"Units of sand that came to rest: {Sand.Count()}");

Console.WriteLine("Press <ENTER> to continue...");
Console.ReadLine();

AddInfiniteFloor(Cave, source);

if (learning)
{
    DrawCave(Cave, Sand, source, null);
}

last_sand_came_to_rest = true;

while (last_sand_came_to_rest)
{
    // produce a new sand and let it come to rest or fall off to the void
    last_sand_came_to_rest = LetNewSandFall(Cave, Sand, source);
}

Console.WriteLine();
Console.WriteLine($"Units of sand that came to rest: {Sand.Count()}");
Console.WriteLine();




void AddInfiniteFloor(Dictionary<(int, int), bool> cave, (int, int) source)
{
    int max_y = source.Item2;

    foreach (var c in cave)
    {
        if (c.Key.Item2 > max_y) max_y = c.Key.Item2;
    }

    int y_from = max_y + 2;
    int y_to = max_y + 2;
    int x_from = source.Item1 - max_y - 2;
    int x_to = source.Item1 + max_y + 2;

    MarkTaken(cave, (x_from, y_from), (x_to, y_to));
}

bool LetNewSandFall(Dictionary<(int, int), bool> cave, Dictionary<(int, int), bool> sand, (int, int) source)
{
    (int, int) new_sand = source;
    bool came_to_rest = false;
    bool fell_off = false;

    int min_x = source.Item1;
    int min_y = source.Item2;
    int max_x = source.Item1;
    int max_y = source.Item2;

    foreach (var c in cave)
    {
        if (c.Key.Item1 < min_x) min_x = c.Key.Item1;
        if (c.Key.Item2 < min_y) min_y = c.Key.Item2;
        if (c.Key.Item1 > max_x) max_x = c.Key.Item1;
        if (c.Key.Item2 > max_y) max_y = c.Key.Item2;
    }

    foreach (var s in sand)
    {
        if (s.Key.Item1 < min_x) min_x = s.Key.Item1;
        if (s.Key.Item2 < min_y) min_y = s.Key.Item2;
        if (s.Key.Item1 > max_x) max_x = s.Key.Item1;
        if (s.Key.Item2 > max_y) max_y = s.Key.Item2;
    }

    while (!came_to_rest && !fell_off)
    {
        int x = new_sand.Item1;
        int y = new_sand.Item2;

        bool can_move_down = !(cave.ContainsKey((x, y + 1)) && cave[(x, y + 1)]) && !(sand.ContainsKey((x, y + 1)) && sand[(x, y + 1)]);
        bool can_move_down_left = !(cave.ContainsKey((x - 1, y + 1)) && cave[(x - 1, y + 1)]) && !(sand.ContainsKey((x - 1, y + 1)) && sand[(x - 1, y + 1)]);
        bool can_move_down_right = !(cave.ContainsKey((x + 1, y + 1)) && cave[(x + 1, y + 1)]) && !(sand.ContainsKey((x + 1, y + 1)) && sand[(x + 1, y + 1)]);

        if (!can_move_down && !can_move_down_left && !can_move_down_right)
        {
            came_to_rest = true;
            sand[new_sand] = true;
        }

        if (can_move_down) { new_sand = (x, y + 1); }
        else if (can_move_down_left) { new_sand = (x - 1, y + 1); } 
        else if (can_move_down_right) { new_sand = (x + 1, y + 1);}

        if  // is destination within the map
            (new_sand.Item1 <= max_x && new_sand.Item1 >= min_x
            && new_sand.Item2 <= max_y && new_sand.Item2 >= min_y)
        {
            fell_off = false;
        }
        else // fell off the map
        {
            fell_off = true;
        }

        if (learning)
        {
            DrawCave(cave, sand, source, new_sand);
            //Console.WriteLine($"sand came to rest: {came_to_rest.ToString()}");
        }
    }

    if // new_sand remained at source means that no more sand can flow
        (new_sand.Item1 == source.Item1 && new_sand.Item2 == source.Item2)
    {
        return false;
    }
    else
    {
        return came_to_rest;
    }
}

void DrawCave(Dictionary<(int, int), bool> cave, Dictionary<(int, int), bool> sand, (int, int) source, (int, int)? new_sand)
{
    int min_x = source.Item1;
    int min_y = source.Item2;
    int max_x = source.Item1;
    int max_y = source.Item2;

    foreach (var c in cave)
    {
        if (c.Key.Item1 < min_x) min_x = c.Key.Item1;
        if (c.Key.Item2 < min_y) min_y = c.Key.Item2;
        if (c.Key.Item1 > max_x) max_x = c.Key.Item1;
        if (c.Key.Item2 > max_y) max_y = c.Key.Item2;
    }

    foreach (var s in sand)
    {
        if (s.Key.Item1 < min_x) min_x = s.Key.Item1;
        if (s.Key.Item2 < min_y) min_y = s.Key.Item2;
        if (s.Key.Item1 > max_x) max_x = s.Key.Item1;
        if (s.Key.Item2 > max_y) max_y = s.Key.Item2;
    }

    Console.WriteLine();

    for (int y = min_y; y <= max_y; y++) 
    {
        for (int x = min_x; x <= max_x; x++)
        {
            if (cave.ContainsKey((x, y)))
            {
                Console.Write(cave[(x, y)] ? '#' : '.');
            }
            else if (sand.ContainsKey((x, y)))
            {
                Console.Write(sand[(x, y)] ? 'o' : '.');
            }
            else if (new_sand is not null && x == new_sand?.Item1 && y == new_sand?.Item2)
            {
                Console.Write('o');
            }
            else if (x == source.Item1 && y == source.Item2)
            {
                Console.Write('+');
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

Dictionary<(int, int), bool> ReadInput(bool learning)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle14_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle14.txt");

    Dictionary<(int, int), bool> taken = new Dictionary<(int, int), bool>();

    foreach (var line in lines)
    {
        var corners = line.Split(" -> ");

        var coordinates = corners[0].Split(",");
        int x = int.Parse(coordinates[0]);
        int y = int.Parse(coordinates[1]);
        taken[(x, y)] = true;

        for (int i = 1; i < corners.Count(); i++) 
        {
            coordinates = corners[i].Split(",");
            int to_x = int.Parse(coordinates[0]);
            int to_y = int.Parse(coordinates[1]);

            MarkTaken(taken, (x, y), (to_x, to_y));

            x = to_x;
            y = to_y;
        }
    }

    return taken;

}
void MarkTaken(Dictionary<(int, int), bool> taken, (int x, int y) from, (int x, int y) to)
{
    for (int xx = Math.Min(0, to.x - from.x); xx < Math.Max(0, to.x - from.x) + 1; xx++)
        for (int yy = Math.Min(0, to.y - from.y); yy < Math.Max(0, to.y - from.y) + 1; yy++)
        {
            taken[(from.x + xx, from.y + yy)] = true;
        }
}

