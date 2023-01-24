// See https://aka.ms/new-console-template for more information

bool learning = false;

List<PairOfPacket> PacketPairs = new List<PairOfPacket>();

PacketPairs = ReadInput(learning);

if (learning)
{
    PrintLists(PacketPairs);
}

int SumOfIndicesInRightOrder = SumOfIndicesThatAreInRightOrder(PacketPairs);

Console.WriteLine("The sum of indices of pairs in the right order is: " + SumOfIndicesInRightOrder.ToString());

Console.WriteLine("Press <ENTER> to continue to part 2...");
Console.ReadLine();
Console.WriteLine();

int DecoderKey = CalculateDecoderKey(PacketPairs);

Console.WriteLine();
Console.WriteLine($"The decoder key is: {DecoderKey}");

int CalculateDecoderKey(List<PairOfPacket> packetPairs)
{
    int decoderKey = 0;

    List<List<PacketData>> PacketData = new List<List<PacketData>>();

    foreach (PairOfPacket packet in packetPairs)
    {
        PacketData.Add(packet.Left);
        PacketData.Add(packet.Right);
    }

    //PacketData.Add(new List<PacketData>());
    //PacketData.Last().Add(new PacketData("[[2]]"));

    //PacketData.Add(new List<PacketData>());
    //PacketData.Last().Add(new PacketData("[[6]]"));

    PacketData = SortPackets(PacketData);

    if (learning)
    {
        PrintSortedLists(PacketData);
    }

    PacketData divider = new PacketData("[[2]]");

    decoderKey = RightPlaceForNewPacket(PacketData, divider);

    divider = new PacketData("[[6]]");

    decoderKey *= (RightPlaceForNewPacket(PacketData, divider) + 1);

    return decoderKey;
}

void PrintSortedLists(List<List<PacketData>> packetData)
{
    Console.WriteLine();
    Console.WriteLine("Sorted lists:");

    int i = 0;

    foreach (var p in packetData) 
    {
        i++;
        Console.Write(i.ToString() + " - ");
        PrintPacket(p);
        Console.WriteLine();
    }

    Console.WriteLine();
}

int RightPlaceForNewPacket(List<List<PacketData>> packetData, PacketData divider)
{
    for (int i = 0; i < packetData.Count; i++) 
    {
        PairOfPacket checkPair = new PairOfPacket();
        checkPair.Left = packetData[i];
        checkPair.Right.Add(divider);

        if (AreListsInRightOrder(checkPair, out bool u))
        {
            continue;
        }
        else
        {
            return i + 1;
        }
    }

    return -1;
}

List<List<PacketData>> SortPackets(List<List<PacketData>> packetData)
{
    List<List<PacketData>> sortedPackets = new List<List<PacketData>>(packetData.Count);

    sortedPackets.Add(packetData[0]);

    for (int i = 1; i < packetData.Count; i++)
    {
        var sortingPacket = packetData[i];
        int currentIndex = i;

        PairOfPacket checkPair = new PairOfPacket();
        checkPair.Left = sortedPackets[currentIndex - 1];
        checkPair.Right = sortingPacket;

        while (currentIndex > 0 && !AreListsInRightOrder(checkPair, out bool u))
        {
            currentIndex--;

            if (currentIndex > 0)
            {
                checkPair.Left = sortedPackets[currentIndex - 1]; 
            }
        }
            
        sortedPackets.Insert(currentIndex, sortingPacket);
    }

    return sortedPackets;
}

int SumOfIndicesThatAreInRightOrder(List<PairOfPacket> packetPairs)
{
    int sum = 0;
    bool undecided;

    for (int i = 0; i < packetPairs.Count; i++)
    {
        if (learning)
        {
            Console.WriteLine("== Pair {0} ==", i + 1);
        }

        if (AreListsInRightOrder(packetPairs[i], out undecided))
        {
            sum += i + 1;    // 1-based index numbering
            //if (learning) 
            //{ 
                Console.WriteLine("Pair {0} is in the right order.", i+1); 
            //}
        }
        else
        {
            //if (learning) 
            //{ 
                Console.WriteLine("Pair {0} is NOT in right order.", i+1); 
            //}
        }

        //if (learning) 
        //{ 
            Console.WriteLine();
        //}
    }

    return sum;
}

bool AreListsInRightOrder(PairOfPacket pairOfPackets, out bool undecided)
{
    undecided = true;
    bool result = false;
    
    int i = 0;

    if (learning)
    {
        Console.Write("- Compare ");
        PrintPacket(pairOfPackets.Left);
        Console.Write(" vs ");
        PrintPacket(pairOfPackets.Right);
        Console.WriteLine();
    }

    while (i < pairOfPackets.Left.Count && i < pairOfPackets.Right.Count)
    {

        if (pairOfPackets.Left[i].IsInteger && pairOfPackets.Right[i].IsInteger)
        {
            if (learning)
            {
                Console.WriteLine("  - Compare integers {0} vs {1}", pairOfPackets.Left[i].IntValue, pairOfPackets.Right[i].IntValue);
            }

            if (pairOfPackets.Left[i].IntValue < pairOfPackets.Right[i].IntValue)
            {
                if (learning) { Console.WriteLine("Left side is smaller."); }
                undecided = false;
                return true;
            }
            else if (pairOfPackets.Left[i].IntValue > pairOfPackets.Right[i].IntValue)
            {
                if (learning) { Console.WriteLine("Right side is smaller."); }
                undecided = false;
                return false;
            }
            //else
            //{
            //    i++;
            //    continue;
            //}
        }

        else if (!pairOfPackets.Left[i].IsInteger && !pairOfPackets.Right[i].IsInteger)
        {
            PairOfPacket newComparison = new PairOfPacket();
            newComparison.Left = pairOfPackets.Left[i].packetList;
            newComparison.Right = pairOfPackets.Right[i].packetList;

            result = AreListsInRightOrder(newComparison, out undecided);
        }

        else if (pairOfPackets.Left[i].IsInteger && !pairOfPackets.Right[i].IsInteger)
        {
            PacketData leftList = new PacketData('[' + pairOfPackets.Left[i].IntValue.ToString() + ']');

            PairOfPacket newComparison = new PairOfPacket();
            newComparison.Left.Add(leftList);
            newComparison.Right.Add(pairOfPackets.Right[i]);

            result = AreListsInRightOrder(newComparison, out undecided);
        }

        else if (!pairOfPackets.Left[i].IsInteger && pairOfPackets.Right[i].IsInteger)
        {
            PacketData rightList = new PacketData('[' + pairOfPackets.Right[i].IntValue.ToString() + ']');

            PairOfPacket newComparison = new PairOfPacket();
            newComparison.Left.Add(pairOfPackets.Left[i]);
            newComparison.Right.Add(rightList);

            result = AreListsInRightOrder(newComparison, out undecided);
        }

        if (undecided)
        {
            i++;
        }
        else
        {
            return result;
        }
    }

    if (i >= pairOfPackets.Left.Count && i < pairOfPackets.Right.Count)
    {
        if (learning)
        {
            Console.WriteLine("Left side ran out of items.");
        }
        undecided = false;
        return true;
    }
    else if (i >= pairOfPackets.Right.Count && i < pairOfPackets.Left.Count)
    {
        if (learning)
        {
            Console.WriteLine("Right side ran out of items.");
        }
        undecided = false;
        return false;
    }
    else if (i >= pairOfPackets.Left.Count && i >= pairOfPackets.Right.Count) // the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input
    {
        if (learning)
        {
            Console.WriteLine("No decision, need to check next part of input.");
        }
        undecided = true;
        return true;
    }
    else 
    { 
        undecided = true;
        return true; 
    }
}

bool AreListsInRightOrder_((string, string) pair)
{
    var left = pair.Item1.Substring(1, pair.Item1.Length - 2).Split(',').ToArray();
    var right = pair.Item2.Substring(1, pair.Item2.Length - 2).Split(',').ToArray();

    if (right.Length < left.Length)
        return false;

    if (left.Length == 0)
        return true;

    for (int i = 0; i < left.Length; i++)
    {
        if (ThisPairInRightOrder((left[i], right[i]), out bool cont))
        {
            if (cont) continue;
            else return true;
        }
        else
        {
            return false;
        }
    }

    return false;
}

bool ThisPairInRightOrder((string, string) pair, out bool cont)
{
    // if both are integers, left should not be bigger
    if (int.TryParse(pair.Item1, out int v1) && int.TryParse(pair.Item2, out int v2))
    {
        if (v1 < v2)
        {
            cont = false;
            return true;
        }
        else if (v1 == v2)
        {
            cont = true;
            return true;
        }
        else
        {
            cont = false;
            return false;
        }
    }

    // if both are lists, check list values one by one
    //if (IsList(pair.Item1) && IsList(pair.Item2))
    {
        if (AreListsInRightOrder_(pair))
        {
            cont = true;
            return true;
        }
    }

    // if exactly one value is integer, convert the integer to list and compare the lists
    //if (IsList(pair.Item1) && !IsList(pair.Item2))
    {
        if (AreListsInRightOrder_((pair.Item1, "[" + pair.Item2 + "]")))
        {
            cont = true;
            return true;
        }
    }

    //if (!IsList(pair.Item1) && IsList(pair.Item2))
    {
        if (AreListsInRightOrder_(("[" + pair.Item1 + "]", pair.Item2)))
        {
            cont = true;
            return true;
        }
    }

    cont = true;
    return false;
}


void PrintLists(List<PairOfPacket> packetPairs)
{
    foreach (PairOfPacket pair in packetPairs)
    {
        PrintPacket(pair.Left);
        Console.WriteLine();
        PrintPacket(pair.Right);
        Console.WriteLine();
        Console.WriteLine();
    }
}

void PrintPacket(List<PacketData> packet)
{
    foreach (PacketData p in packet)
    {
        if (p.IsInteger) 
        {
            Console.Write(p.IntValue.ToString());
        }
        else 
        { 
            Console.Write('[');
            PrintPacket(p.packetList); 
            Console.Write(']');
        }
        
        if (packet.IndexOf(p) != packet.Count - 1) { Console.Write(','); }
    }
}

List<PairOfPacket> ReadInput(bool learning)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle13_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle13.txt");

    List<(string, string)> packetPairStrings = new List<(string, string)>();

    for (int i = 0; i < (lines.Length + 2) / 3; i++)
    {
        packetPairStrings.Add((lines[i * 3], lines[i * 3 + 1]));
    }

    List<PairOfPacket> packetPairs = new();
    
    foreach (var pp in packetPairStrings)
    {
        packetPairs.Add(new PairOfPacket());
        packetPairs.Last().Left.Add(new PacketData(pp.Item1));
        packetPairs.Last().Right.Add(new PacketData(pp.Item2));
    }

    return packetPairs;
}