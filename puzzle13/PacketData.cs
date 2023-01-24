// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

internal class PacketData
{
    public bool IsInteger { get; set; }    
    public int IntValue { get; set; }
    public List<PacketData> packetList { get; set; }

    public PacketData(string item)
    {
        IsInteger = false;
        IntValue = 0;
        packetList = new List<PacketData>();

        if (int.TryParse(item, out int intValue))
        {
            IsInteger = true;
            IntValue = intValue;
        }
        else
        {
            packetList = ConvertStringToList(item);
        }
    }


    private List<PacketData> ConvertStringToList(string item)
    {
        List<PacketData> packets = new List<PacketData>();

        if (item.First() != '[' || item.Last() != ']')
        {
            return packets; // not a proper list
        }

        if (item.Length == 2)
        {
            return packets; // empty list
        }

        string _list = item.Substring(1, item.Length - 2);

        if (_list.Contains('[') || _list.Contains(']'))
        {
            //throw new NotImplementedException();

            // temporarily replace all inner commas with semicolons
            string _list_temp = "";
            int inner = 0;
            foreach (char c in _list)
            {
                switch (c)
                {
                    case '[':
                        inner += 1;
                        _list_temp += c;
                        break;
                    case ']': inner -= 1;
                        _list_temp += c;
                        break;
                    case ',':
                        if (inner > 0)
                        {
                            _list_temp += ';';
                            break;
                        }
                        else
                        {
                            _list_temp += c;
                            break;
                        }
                    default:
                        _list_temp += c;
                        break;
                }
            }
            foreach (string i in _list_temp.Split(','))
            {
                packets.Add(new PacketData(i.Replace(';', ',')));
            }
        }
        else
        {
            foreach (string i in _list.Split(','))
            {
                packets.Add(new PacketData(i));
            }
        }


        return packets;
    }
}
