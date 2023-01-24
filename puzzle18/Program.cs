// See https://aka.ms/new-console-template for more information

bool learning = false;

List<(int, int, int)> Cubes = ReadInput(learning);

((int, int, int), (int, int, int)) GridSize;

Dictionary<(int, int, int), bool> Grid = PopulateGridMap(Cubes, out GridSize);

(int, int, int)[] Sides = new (int, int, int)[6];

Sides[0] = (-1, 0, 0);
Sides[1] = (0, -1, 0);
Sides[2] = (0, 0, -1);
Sides[3] = (1, 0, 0);
Sides[4] = (0, 1, 0);
Sides[5] = (0, 0, 1);

Dictionary<(int, int, int), object> ReachableCoordinates = FindReachableCoordinates(Grid);

Dictionary<(int, int, int), object> UnreachableCoordinates = FindUnreachableCoordinates(Grid, ReachableCoordinates);

bool OuterOnly = false;

int SurfaceArea = CountExposedSides(Cubes, Grid, GridSize, OuterOnly);

Console.WriteLine("Total surface area of the lava droplets: " + SurfaceArea);

OuterOnly = true;

int ExteriorSurfaceArea = CountExposedSides(Cubes, Grid, GridSize, OuterOnly);

Console.WriteLine("External surface area of the lava droplets: " + ExteriorSurfaceArea);


Dictionary<(int, int, int), object> FindUnreachableCoordinates(Dictionary<(int, int, int), bool> grid, Dictionary<(int, int, int), object> reachableCoordinates)
{
    Dictionary<(int, int, int), object> unreachableCoordinates = new Dictionary<(int, int, int), object>();

    foreach (var kvp in grid)
    {
        if (!kvp.Value && !reachableCoordinates.ContainsKey(kvp.Key))
        {
            unreachableCoordinates.Add(kvp.Key, null);
        }
    }

    return unreachableCoordinates;
}

int CountExposedSides(List<(int, int, int)> cubes, Dictionary<(int, int, int), bool> grid, ((int, int, int), (int, int, int)) cubeSize, bool outerOnly)
{
    int count = 0;

    (int, int, int) checkingCoordinates;

    foreach (var cube in cubes)
    {
        for (int side = 0; side < Sides.Length; side++)
        {
            checkingCoordinates = (cube.Item1 + Sides[side].Item1, cube.Item2 + Sides[side].Item2, cube.Item3 + Sides[side].Item3);

            if (!grid[checkingCoordinates])
            {
                if (!outerOnly)
                {
                    count++;
                }
                else if (ReachableCoordinates.ContainsKey(checkingCoordinates))
                {
                    count++;
                }
            }
        }
    }

    return count;
}

Dictionary<(int, int, int), object> FindReachableCoordinates(Dictionary<(int, int, int), bool> grid)
{
    Dictionary<(int, int, int), object> reachable = new Dictionary<(int, int, int), object>();

    Queue<(int, int, int)> queue = new Queue<(int, int, int)>();

    var startPoint = grid.FirstOrDefault().Key;
    reachable.Add(startPoint, null);
    queue.Enqueue(startPoint);

    // seed the discovery
    while (queue.Count > 0)
    {
        // take first in queue
        (int, int, int) fromCoordinates = queue.Dequeue();

        // check each possible moving direction
        for (int side = 0; side < Sides.Length; side++)
        {
            //(int, int, int) possibleMove = (checkingCoordinates.Item1 + Sides[side].Item1, checkingCoordinates.Item2 + Sides[side].Item2, checkingCoordinates.Item3 + Sides[side].Item3);
            (int, int, int) possibleMove = (fromCoordinates.Item1 + Sides[side].Item1, fromCoordinates.Item2 + Sides[side].Item2, fromCoordinates.Item3 + Sides[side].Item3);

            if (
                possibleMove != fromCoordinates              // don't look back where we just came from
                && grid.ContainsKey(possibleMove)            // be sure this is an actual place on the grid
                && !grid[possibleMove]                       // exclude spaces taken up by lava droplets
                && !reachable.ContainsKey(possibleMove)      // don't look at places we've been before
                )
            {
                reachable.Add(possibleMove, null);
                queue.Enqueue(possibleMove);
            }
        }
    }

return reachable;
}



Dictionary<(int, int, int), bool> PopulateGridMap(List<(int, int, int)> cubes, out ((int, int, int), (int, int, int)) cubeSize)
{
    Dictionary<(int, int, int), bool> gridMap = new Dictionary<(int, int, int), bool>();

    cubeSize = (
        (cubes[0].Item1, cubes[0].Item2, cubes[0].Item3),
        (cubes[0].Item1, cubes[0].Item2, cubes[0].Item3)
    );

    foreach (var c in cubes)
    {
        if (c.Item1 < cubeSize.Item1.Item1) cubeSize.Item1.Item1= c.Item1;
        if (c.Item2 < cubeSize.Item1.Item2) cubeSize.Item1.Item2= c.Item2;
        if (c.Item3 < cubeSize.Item1.Item3) cubeSize.Item1.Item3= c.Item3;
        if (c.Item1 > cubeSize.Item2.Item1) cubeSize.Item2.Item1= c.Item1;
        if (c.Item2 > cubeSize.Item2.Item2) cubeSize.Item2.Item2= c.Item2;
        if (c.Item3 > cubeSize.Item2.Item3) cubeSize.Item2.Item3= c.Item3;
    }

    cubeSize.Item1.Item1--; cubeSize.Item2.Item1++;
    cubeSize.Item1.Item2--; cubeSize.Item2.Item2++;
    cubeSize.Item1.Item3--; cubeSize.Item2.Item3++;

    for (int x = cubeSize.Item1.Item1; x <= cubeSize.Item2.Item1; x++)
        for (int y = cubeSize.Item1.Item2; y <= cubeSize.Item2.Item2; y++)
            for (int z = cubeSize.Item1.Item3; z <= cubeSize.Item2.Item3; z++)
                gridMap[(x, y, z)] = false;

    foreach (var c in cubes)
        gridMap[(c.Item1, c.Item2, c.Item3)] = true;

    return gridMap;
}

List<(int, int, int)> ReadInput(bool learning)
{
    List<(int, int, int)> cubes = new List<(int, int, int)>();

    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle18_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle18.txt");

    int[] c;

    foreach (string line in lines)
    {
        c = line
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        cubes.Add( (c[0], c[1], c[2]) );
    }

    return cubes;
}