// See https://aka.ms/new-console-template for more information

bool learning = true;
bool basic_learning = false;
bool reachability_learning = false;
bool reachability_deep_learning = false;
bool use_reachable_tower_cleaning = false;
int part = 2;

List<List<(long x, int y)>> Rocks = new List<List<(long x, int y)>>();

AddRocks(Rocks, learning);

char[] JetPattern = ReadInput(learning);

Dictionary<(long x, int y), object> Tower = new Dictionary<(long x, int y), object>();

long rocks_to_fall_count = 0;

if (part == 1)
{
    if (basic_learning) { rocks_to_fall_count = 11; }
    else { rocks_to_fall_count = 2022; }
}

if (part == 2) rocks_to_fall_count = 1000000000000;

int current_rock = 0;
List<(long x, int y)> rock = new List<(long x, int y)>();
int current_jet = 0;

long tower_height = 0;

for (long i = 0; i < rocks_to_fall_count; i++)
{
    rock.Clear();

    foreach (var r in Rocks[current_rock])
    {
        rock.Add(r);
    }

    current_jet = LetRocksFall(Tower, tower_height, rock, JetPattern, current_jet, learning, out Dictionary<(long x, int y), object> new_tower, out long new_tower_height);

    Tower = new_tower;

    tower_height = new_tower_height;

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

    if (reachability_learning)
    {
        Console.WriteLine($"Will now clear the unreachable part of the tower...");
        Tower = ClearUnreachableTunnel(Tower, tower_height);
        Console.WriteLine($"Cleared the unreachable part of the tower.");
        DrawTower(Tower, tower_height, null);
    }
    else
    {
        if (i % 10000 == 0)
        {
            Tower = ClearUnreachableTunnel(Tower, tower_height);
        }
    }
}

Console.WriteLine();
Console.WriteLine($"The height of the tower of rocks after {rocks_to_fall_count} have stopped falling is: {tower_height}.");
Console.WriteLine();

//Console.WriteLine("Press <ENTER> to continue...");
//Console.ReadLine();

return;

int LetRocksFall(Dictionary<(long x, int y), object> tower, long tower_height, List<(long x, int y)> rock, char[] jetPattern, int current_jet, bool learning, out Dictionary<(long x, int y), object> new_tower, out long new_tower_height)
{
    //long tower_height = HowHighIsTheTower(tower);
    long rock_height = HowHighIsRock(rock);
    bool landed = false;
    new_tower_height = tower_height;

    for (int r = 0; r < rock.Count(); r++)
    {
        rock[r] = (tower_height + rock[r].x + 4, rock[r].y + 2);
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
                if (r.x > new_tower_height)
                {
                    new_tower_height = r.x;
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

    List<(long x, int y)> FallDown(List<(long x, int y)> rock, out bool landed)
    {
        List<(long x, int y)> new_position = new List<(long x, int y)>();
        (long x, int y) new_coordinates = (0, 0);
        bool can_move = true;
        landed = false;

        foreach (var part in rock)
        {
            new_coordinates = (part.x - 1, part.y);

            can_move = CheckSpace(new_coordinates);

            if (can_move)
            {
                new_position.Add(new_coordinates);
            }
            else
            {
                break;
            }
        }

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

    List<(long x, int y)> PushRock(List<(long x, int y)> rock, string direction)
    {
        List<(long x, int y)> new_position = new List<(long x, int y)>();
        bool can_move = true;

        foreach (var part in rock)
        {
            (long x, int y) new_coordinates = (0, 0);

            if (direction == "left")
            {
                new_coordinates = (part.x, part.y - 1);

            }
            else if (direction == "right")
            {   
                new_coordinates = (part.x, part.y + 1);
            }
            
            can_move = CheckSpace(new_coordinates);

            if (can_move)
            {
                new_position.Add(new_coordinates);
            }
            else
            {
                break;
            }
        }

        if (can_move)
        {
            rock = new_position;
        }

        return rock;
    }

    bool CheckSpace((long x, int y) coordinates)
    {
        bool is_space = true;

        if (coordinates.x < 1 || coordinates.y < 0 || coordinates.y > 6)
        {
            is_space = false;
        }
        else if (tower.ContainsKey(coordinates))
        {
            is_space = false;
        }

        return is_space;
    }
}

Dictionary<(long x, int y), object> ClearUnreachableTunnel(Dictionary<(long x, int y), object> tower, long tower_height)
{
    Dictionary<(long x, int y), object> new_tower = new Dictionary<(long x, int y), object>();

    long new_floor = tower_height;

    if (use_reachable_tower_cleaning)
    {
        for (int y = 0; y < 7; y++)
        {
            for (long x = tower_height; x > 0; x--)
            {
                //if (reachability_learning && x == 6 && y == 1) 
                //{ 
                //    Console.WriteLine("This has to be reachable after Rock 5!");
                //    Console.Write("Press <ENTER> to continue...");
                //    Console.ReadLine(); 
                //}

                if (reachability_learning)
                {
                    Console.WriteLine($"Starting reachability analysis from {(tower_height + 3, 3).ToString()} to {(x, y).ToString()}.");
                }

                if (!tower.ContainsKey((x, y)) && IsReachable((tower_height + 3, 3), (x, y)))
                {
                    if (x <= new_floor)
                    {
                        new_floor = x - 1;
                    }
                }
            }
        }
    }
    else
    {
        new_floor -= 100;
    }

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

    bool IsReachable((long x, int y) source, (long x, int y) destination)
    {
        if (reachability_deep_learning)
        {
            Console.Write($"  Checking reachability from {source.ToString()} to {destination.ToString()} ... ");
        }

        // idea copied from:
        // https://www.geeksforgeeks.org/check-destination-reachable-source-two-movements-allowed/

        // base case: out of reach
        //if (source.x < destination.x || Math.Abs(source.y - destination.y) > 1)
        //{
        //    if (reachability_learning) { Console.WriteLine("Out of reach. FALSE."); }
        //    return false;
        //}

        // current point is equal to destination
        if (source.x == destination.x && source.y == destination.y)
        {
            if (reachability_deep_learning) { Console.WriteLine("  Reached destination. TRUE."); }
            return true;
        }

        // current point is out of cave
        if (source.x < 1 || source.y < 0 || source.y > 6)
        {
            if (reachability_deep_learning) { Console.WriteLine("  Out of cave. FALSE."); }
            return false;
        }

        // current point is occupied by fallen rocks
        if (tower.ContainsKey((source.x, source.y)))
        {
            if (reachability_deep_learning) { Console.WriteLine("  Occupied. FALSE."); }
            return false;
        }

        // check for other possibilities
        if (reachability_deep_learning) { Console.WriteLine("  Undetermined. Will look around."); }
        
        return (
            (!tower.ContainsKey((source.x - 1, source.y)) && IsReachable((source.x - 1, source.y), destination))                 // move one down
            || (!tower.ContainsKey((source.x - 1, source.y - 1)) && IsReachable((source.x - 1, source.y - 1), destination))      // move one left and one down
            || (!tower.ContainsKey((source.x - 1, source.y + 1)) && IsReachable((source.x - 1, source.y + 1), destination))      // move one right and one down
            );
    }
}



long HowHighIsRock(List<(long x, int y)> rock)
{
    long max_x = 0;

    foreach (var r in rock)
    {
        if (r.x > max_x) max_x = r.x;
    }

    return max_x + 1;
}

void DrawTower(Dictionary<(long x, int y), object> tower, long tower_height, List<(long x, int y)>? rock)
{
    //long max_x = HowHighIsTheTower(tower);
    if (rock is not null)
    {
        foreach (var r in rock)
        {
            if (r.x > tower_height) tower_height = r.x;
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
            for (int y = 0; y < 7; y++)
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

void AddRocks(List<List<(long x, int y)>> rocks, bool learning)
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
            int max_y = 0;

            foreach (var coords in rock)
            {
                if (coords.x > max_x) max_x = coords.x;
                if (coords.y > max_y) max_y = coords.y;
            }

            for (long x = max_x; x >= 0; x--)
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

    void AddNewRock(List<List<(long x, int y)>> rocks, string rockString)
    {
        string[] rockLines = rockString.Split("\r\n");

        List<(long x, int y)> rock = new List<(long x, int y)>();

        for (int line = 0; line < rockLines.Count(); line++)
        {
            for (int c = 0; c < rockLines[line].Length; c++)
            {
                if (rockLines[line][c] == '#')
                {
                    rock.Add((rockLines.Count() - 1 - line, c));
                }
            }
        }

        rocks.Add(rock);
        return;
    }
}
