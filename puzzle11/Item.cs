using System;
using System.Collections.Generic;
using System.Numerics;

namespace puzzle11
{
    public class Item
    {
        public long WorryLevel { get; set; }
        public List<ItemHistory> itemHistories  { get; set; }
        public Item()
        {
            WorryLevel = 0;
            itemHistories = new List<ItemHistory>();
        }
        public void AddItemHistoryElement(ItemHistory _ih)
        {
            itemHistories.Add(_ih);
            WorryLevel = OneOperation(_ih);
        }
        public long WorryLevelCalc() 
        {
            WorryLevel = 0;

            foreach (ItemHistory ih in itemHistories)
            {
                WorryLevel = OneOperation(ih);
            }
            return WorryLevel; 
        }
        public long OneOperation(ItemHistory _ih)
        {
            long r = WorryLevel;

            switch (_ih.operation)
            {
                case "*":
                    r *= _ih.factor;
                    break;
                case "+":
                    r += _ih.factor;
                    break;
                case "**":
                    r *= r;
                    break;
                case "/":
                    r = r / 3;
                    break;
                case "%":
                    r = r % _ih.factor;
                    break;
                default:
                    break;
            }

            return r;
        }
    }
}