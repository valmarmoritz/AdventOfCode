// See https://aka.ms/new-console-template for more information

bool learning = true;
bool basic_learning = false;
int part = 2;

List<List<(long, long)>> Rocks = new List<List<(long, long)>>();

AddRocks(Rocks, learning);

char[] JetPattern = ReadInput(learning);

Dictionary<(long, long), object> Tower = new Dictionary<(long, long), object>();

long rocks_to_fall_count = 0;

if (part == 1)
{
    if (basic_learning) { rocks_to_fall_count = 11; }
    else { rocks_to_fall_count = 2022; }
}

if (part == 2) rocks_to_fall_count = 1000000000000;

int current_rock = 0;
List<(long, long)> rock = new List<(long, long)>();
int current_jet = 0;

long tower_height = 0;

for (long i = 0; i < rocks_to_fall_count; i++)
{
    rock.Clear();

    foreach (var r in Rocks[current_rock])
    {
        rock.Add(r);
    }

    current_jet = LetRocksFall(Tower, tower_height, rock, JetPattern, current_jet, learning, out Dictionary<(long, long), object> new_tower, out long new_tower_height);

    Tower = new_tower;

    tower_height = new_tower_height;

    if (i % 100 == 0)
    { 
        Tower = ClearUnreachableTunnel(Tower, tower_height);
    }

    if (learning)
    {
        if (basic_learning)
        {
            Console.WriteLine($"Rock {(i + 1).ToString()} landed.");
            //DrawTower(Tower, tower_height, null);
            Console.WriteLine();
        }

        else
        {
            if (part == 1) { Console.Write($"R {(i + 1).ToString()} - "); }
            else if (part == 2)
            {
                if (i % (rocks_to_fall_count / Math.Pow(10, 5)) == 0)
                {
                    Console.Write($"{(i * 1.0 / rocks_to_fall_count).ToString("P3")} ");
                }
            }
        }
    }

    current_rock++;
    current_rock %= Rocks.Count();
}

Console.WriteLine();
Console.WriteLine($"The height of the tower of rocks after {rocks_to_fall_count} have stopped falling is: {tower_height}.");
Console.WriteLine();

//Console.WriteLine("Press <ENTER> to continue...");
//Console.ReadLine();

return;

int LetRocksFall(Dictionary<(long, long), object> tower, long tower_height, List<(long, long)> rock, char[] jetPattern, int current_jet, bool learning, out Dictionary<(long, long), object> new_tower, out long new_tower_height)
{
    //long tower_height = HowHighIsTheTower(tower);
    long rock_height = HowHighIsRock(rock);
    bool landed = false;
    new_tower_height = tower_height;

    for (int r = 0; r < rock.Count(); r++)
    {
        rock[r] = (tower_height + rock_height - rock[r].Item1 + 3, rock[r].Item2 + 2);
    }

    if (basic_learning)
    {
        Console.WriteLine("New rock appears:");
        DrawTower(tower, tower_height, rock);
    }

    for (int j = current_jet; j <= jetPattern.Count(); j++)
    {
        j %= jetPattern.Count();

        if (jetPattern[j] == '<')
        {
            rock = PushRock(rock, "left");
        }
        else if (jetPattern[j] == '>')
        {
            rock = PushRock(rock, "right");
        }

        if (basic_learning)
        {
            Console.WriteLine($"Rock is pushed to {jetPattern[j]} (j = {j})");
            DrawTower(tower, tower_height, rock);
        }

        rock = FallDown(rock, out landed);

        if (basic_learning)
        {
            Console.WriteLine("Rock falls down:");
            DrawTower(tower, tower_height, rock);
        }

        if (landed)
        {
            foreach (var r in rock)
            {
                if (r.Item1 > new_tower_height)
                {
                    new_tower_height = r.Item1;
                }
                tower[r] = new object();
            }

            if (basic_learning)
            {
                Console.WriteLine("Rock has landed:");
                DrawTower(tower, new_tower_height, null);
            }

            current_jet = j;
            break;
        }
    }

    new_tower = tower;
    return current_jet + 1;

    List<(long, long)> FallDown(List<(long, long)> rock, out bool landed)
    {
        List<(long, long)> new_position = new List<(long, long)>();
        bool can_move = true;
        landed = false;

        foreach (var part in rock)
        {
            new_position.Add((part.Item1 - 1, part.Item2));
        }

        can_move = CheckSpace(new_position);

        if (can_move)
        {
            rock = new_position;
        }
        else
        {
            landed = true;
        }

        return rock;
    }

    List<(long, long)> PushRock(List<(long, long)> rock, string direction)
    {
        List<(long, long)> new_position = new List<(long, long)>();
        bool can_move = true;

        foreach (var part in rock)
        {
            if (direction == "left")
            {
                new_position.Add((part.Item1, part.Item2 - 1));
            }
            else if (direction == "right")
            {
                new_position.Add((part.Item1, part.Item2 + 1));
            }
        }

        can_move = CheckSpace(new_position);

        if (can_move)
        {
            rock = new_position;
        }

        return rock;
    }

    bool CheckSpace(List<(long, long)> new_position)
    {
        bool is_space = true;

        foreach (var part in new_position)
        {
            if (part.Item1 < 1 || part.Item2 < 0 || part.Item2 > 6)
            {
                is_space = false;
                break;
            }
            if (tower.ContainsKey(part))
            {
                is_space = false;
                break;
            }
        }

        return is_space;
    }
}

Dictionary<(long, long), object> ClearUnreachableTunnel(Dictionary<(long, long), object> tower, long tower_height)
{
    Dictionary<(long, long), object> new_tower = new Dictionary<(long, long), object>();

    long new_floor = tower_height;

    for (int y = 0; y < 7; y++)
    {
        for (long x = tower_height; x >= 0; x--)
        {
            if (tower.ContainsKey((x, y)))
            {
                break;
            }
            else
            {
                if (x <= new_floor)
                {
                    new_floor = x - 1;
                }
            }
        }
    }

    // as we really did not check reachability, let's leave some rows more to be sure
    new_floor -= 20;

    for (long x = tower_height; x >= new_floor; x--)
    {
        for (int y = 0; y < 7; y++)
        {
            if (tower.ContainsKey((x, y)))
            {
                new_tower[(x, y)] = new object();
            }
        }
    }

    return new_tower;
}



long HowHighIsRock(List<(long, long)> rock)
{
    long max_x = 0;

    foreach (var r in rock)
    {
        if (r.Item1 > max_x) max_x = r.Item1;
    }

    return max_x + 1;
}

long HowHighIsTheTower(Dictionary<(long, long), object> tower)
{
    long max_x = 0;
    long max_y = 0;

    //foreach (var t in tower.Keys)
    //{
    //    if (t.Item1 > max_x) max_x = t.Item1;
    //}

    if (tower.Keys.Count > 0)
    {
        (max_x, max_y) = tower.Keys.Max();
    }

    return max_x;
}

void DrawTower(Dictionary<(long, long), object> tower, long tower_height, List<(long, long)>? rock)
{
    //long max_x = HowHighIsTheTower(tower);
    if (rock is not null)
    {
        foreach (var r in rock)
        {
            if (r.Item1 > tower_height) tower_height = r.Item1;
        }
    }

    for (long x = tower_height; x >=0; x--)
    {
        if (x == 0)
        {
            Console.WriteLine("+" + new string('-',7) + "+");
        }
        else
        {
            Console.Write('|');
            for (long y = 0; y < 7; y++)
            {
                if (tower.ContainsKey((x, y))) Console.Write('#');
                else if (rock is not null && rock.Contains((x, y))) Console.Write('@');
                else Console.Write('.');
            }
            Console.WriteLine('|');
        }
    }
}

char[] ReadInput(bool learning)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle17_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle17.txt");

    return lines[0].ToCharArray();
}

void AddRocks(List<List<(long, long)>> rocks, bool learning)
{
    AddNewRock(rocks, "####");

    AddNewRock(rocks, ".#.\r\n###\r\n.#.");

    AddNewRock(rocks, "..#\r\n..#\r\n###");

    AddNewRock(rocks, "#\r\n#\r\n#\r\n#");

    AddNewRock(rocks, "##\r\n##");

    if (learning)
    {
        foreach (var rock in rocks)
        {
            long max_x = 0;
            long max_y = 0;

            foreach (var coords in rock)
            {
                if (coords.Item1 > max_x) max_x = coords.Item1;
                if (coords.Item2 > max_y) max_y = coords.Item2;
            }

            for (int x = 0; x <= max_x; x++)
            {
                for (int y = 0; y <= max_y; y++)
                {
                    if (rock.Contains((x, y))) Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    return;

    void AddNewRock(List<List<(long, long)>> rocks, string rockString)
    {
        string[] rockLines = rockString.Split("\r\n");

        List<(long, long)> rock = new List<(long, long)>();

        for (int line = 0; line < rockLines.Count(); line++)
        {
            for (int c = 0; c < rockLines[line].Length; c++)
            {
                if (rockLines[line][c] == '#')
                {
                    rock.Add((line, c));
                }
            }
        }

        rocks.Add(rock);
        return;
    }
}
