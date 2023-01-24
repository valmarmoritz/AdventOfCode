// See https://aka.ms/new-console-template for more information

internal class PairOfPacket
{
    public List<PacketData> Left { get; set; } 
    public List<PacketData> Right { get; set; }

    public PairOfPacket()
    {
        Left = new List<PacketData>();
        Right = new List<PacketData>();
    }
}