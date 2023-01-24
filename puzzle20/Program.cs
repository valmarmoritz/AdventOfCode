// See https://aka.ms/new-console-template for more information

bool learning = false;
int part = 2;
long DecryptionKey = 811589153;
// part 1: mix once
// part 2: use decryption key, mix 10 times
bool decrypt = false;
int mix_cycles = 0;
if (part == 1) { decrypt = false; mix_cycles = 1; }
if (part == 2) { decrypt = true;  mix_cycles = 10; }

List<long> EncryptedFile = new List<long>();

ReadInput(EncryptedFile, learning);

if (decrypt) { ApplyDecryptionKey(EncryptedFile, DecryptionKey); }

Dictionary<int, (int, long)> DecryptedFile = new Dictionary<int, (int, long)>();
Dictionary<int, int> ReverseLookup = new Dictionary<int, int>();

InitializeDecryptionFile(EncryptedFile, DecryptedFile, ReverseLookup);

Console.WriteLine("Mix cycle: 1");
// Key: sequence number in the decrypted file; Value.Item1 = original sequence number; Value.Item2: numeric value of item;
DecryptedFile = DecryptFile(DecryptedFile, ReverseLookup);
Console.WriteLine();

if (mix_cycles > 1)
{
    for (int c = 1; c < mix_cycles; c++)
    {
        Console.WriteLine("Mix cycle: " + (c + 1));
        DecryptedFile = DecryptFile(DecryptedFile, ReverseLookup);
        Console.WriteLine();
    }
}

long groove_coordinates = CalcGrooveCoordinates(DecryptedFile);

Console.WriteLine("Groove coordinates: " + groove_coordinates);

long CalcGrooveCoordinates(Dictionary<int, (int, long)> decryptedFile)
{
    long r = 0;
    int first_position_of_0 = decryptedFile.FirstOrDefault(x => x.Value.Item2 == 0).Key;
    int length_of_file = decryptedFile.Count();

    // add 1000th, 2000th and 3000th element values after "0"
    for (int i = 1; i <= 3; i++)
    {
        int add_from_position = i * 1000 + first_position_of_0;
        r += decryptedFile[add_from_position % length_of_file].Item2;
    }

    return r;
}

Dictionary<int, (int, long)> DecryptFile(Dictionary<int, (int, long)> decryptedFile, Dictionary<int, int> reverseLookup)
{
    // move each element in the original order
    for (int i = 0; i < reverseLookup.Count(); i++)
    {
        //MoveItem(decryptedFile, i, reverseLookup);
        MoveItemCleverly(decryptedFile, i, reverseLookup);
        if (learning) Console.WriteLine(string.Join(",", decryptedFile));
    }

    return decryptedFile;
}

void MoveItemCleverly(Dictionary<int, (int, long)> decryptedFile, int i, Dictionary<int, int> reverseLookup)
{
    int max_index = decryptedFile.Count() - 1;
    int current_position = reverseLookup[i];
    (int, long) current_element = decryptedFile[current_position];
    long move = current_element.Item2;
    int next_position;

    if (move > 0)
    {
        move = move % max_index;

        if (move == 0) { Shift(decryptedFile, reverseLookup, "right"); }

        for (long n = 0; n < move; n++)
        {
            current_position = reverseLookup[i];
            if (current_position == max_index) { next_position = 0; }
            else { next_position = current_position + 1; }

            decryptedFile[current_position] = decryptedFile[next_position];
            reverseLookup[decryptedFile[next_position].Item1] = current_position;

            decryptedFile[next_position] = current_element;
            reverseLookup[i] = next_position;

            if (/*current_position == max_index ||*/ next_position == max_index) { Shift(decryptedFile, reverseLookup, "right"); }
        }
    }
    else if (move < 0)
    {
        move = move % max_index;
        
        if (move == 0) { Shift(decryptedFile, reverseLookup, "left"); }


        for (long n = 0; n > move; n--)
        {
            current_position = reverseLookup[i];
            if (current_position == 0) { next_position = max_index; }
            else { next_position = current_position - 1; }

            decryptedFile[current_position] = decryptedFile[next_position];
            reverseLookup[decryptedFile[next_position].Item1] = current_position;

            decryptedFile[next_position] = current_element;
            reverseLookup[i] = next_position;

            if (/*current_position == 0 ||*/ next_position == 0) { Shift(decryptedFile, reverseLookup, "left"); }
        }
    }
}

void MoveItem(Dictionary<int, (int, long)> decryptedFile, int i, Dictionary<int, int> reverseLookup)
{
    int max_index = decryptedFile.Count() - 1;
    int current_position = reverseLookup[i];
    (int, long) current_element = decryptedFile[current_position];
    long move = current_element.Item2;
    int next_position;

    if (move > 0)
    {
        for (long n = 0; n < move; n++)
        {
            current_position = reverseLookup[i];
            if (current_position == max_index) { next_position = 0; }
            else { next_position = current_position + 1; }

            decryptedFile[current_position] = decryptedFile[next_position];
            reverseLookup[decryptedFile[next_position].Item1] = current_position;

            decryptedFile[next_position] = current_element;
            reverseLookup[i] = next_position;

            if (current_position == max_index || next_position == max_index) { Shift(decryptedFile, reverseLookup, "right"); }
        }
    }
    else if (move < 0)
    {
        for (long n = 0; n > move; n--)
        {
            current_position = reverseLookup[i];
            if (current_position == 0) { next_position = max_index; }
            else { next_position = current_position - 1; }

            decryptedFile[current_position] = decryptedFile[next_position];
            reverseLookup[decryptedFile[next_position].Item1] = current_position;

            decryptedFile[next_position] = current_element;
            reverseLookup[i] = next_position;

            if (current_position == 0 || next_position == 0) { Shift(decryptedFile, reverseLookup, "left"); }
        }
    }
}

void Shift(Dictionary<int, (int, long)> decryptedFile, Dictionary<int, int> reverseLookup, string v)
{
    if (v == "left")
    {
        int max_index = decryptedFile.Count() - 1;
        var first_element = decryptedFile[0];

        for (int i = 0; i < max_index; i++)
        {
            decryptedFile[i] = decryptedFile[i + 1];

        }

        decryptedFile[max_index] = first_element;

        for (int i = 0; i <= max_index; i++)
        {
            if (reverseLookup[i] == 0) { reverseLookup[i] = max_index; }
            else { reverseLookup[i]--; }
        }
    }
    else if (v == "right")
    {
        int max_index = decryptedFile.Count() - 1;
        var last_element = decryptedFile[max_index];

        for (int i = max_index; i > 0; i--)
        {
            decryptedFile[i] = decryptedFile[i - 1];

        }

        decryptedFile[0] = last_element;

        for (int i = 0; i <= max_index; i++)
        {
            if (reverseLookup[i] == max_index) { reverseLookup[i] = 0; }
            else { reverseLookup[i]++; }
        }
    }
}

/*
void MoveItem(ObservableCollection<int> decryptedFile, int i)
{
   if (i != 0)
   {
       int max_index = decryptedFile.Count - 1;
       int current_location = decryptedFile.IndexOf(i);

       int new_location = (current_location + i);

       if (i > 0)
       {
           new_location = new_location % max_index;
       }
       else if (i < 0)
       {
           while (new_location <= 0) new_location += max_index;
       }

       if (current_location != new_location)
       {
           decryptedFile.Move(current_location, new_location);
       }
   }
}
*/

void InitializeDecryptionFile(List<long> encryptedFile, Dictionary<int, (int, long)> decryptedFile, Dictionary<int, int> reverseLookup)
{
    // copy of the original to start sorting; reverse lookup to know where each element of the original array currently are
    for (int i = 0; i < encryptedFile.Count(); i++)
    {
        decryptedFile[i] = new(i, encryptedFile[i]);
        reverseLookup[i] = i;
    }

    Console.WriteLine("Initial arrangement:");
    if (learning) Console.WriteLine(string.Join(",", decryptedFile));
    Console.WriteLine();
}

void ApplyDecryptionKey(List<long> encryptedFile, long decryptionKey)
{
    for (int i = 0; i < encryptedFile.Count(); i++)
    {
        encryptedFile[i] *= decryptionKey;
    }
}


void ReadInput(List<long> encryptedFile, bool learning)
{
    string[] lines;
    // Read a text file line by line.
    if (learning) lines = System.IO.File.ReadAllLines(@"puzzle20_learning.txt");
    else lines = System.IO.File.ReadAllLines(@"puzzle20.txt");

    foreach (string line in lines)
    {
        encryptedFile.Add(int.Parse(line));
    }

    return;
}
