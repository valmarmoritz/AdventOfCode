using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace puzzle11
{
    class Monkey
    {
        public List<Item> Items { get; set; }
        public string OperationFunction { get; set; }
        public int OperationFactor { get; set; }
        public string TestFunction { get; set; }
        public int TestFactor { get; set; }
        public int TrueThrowTo { get; set; }
        public int FalseThrowTo { get; set; }
        public int InspectedItemsCount { get; set; }

        public Monkey()
        {
            Items = new List<Item>();
        }
        public void Operation(Item _item)
        {
            _item.AddItemHistoryElement(new ItemHistory
            {
                operation = OperationFunction,
                factor = OperationFactor,
            });
        }

        public bool Test(Item _item)
        {
            //return BigInteger.Remainder(_item.WorryLevelCalc(), TestFactor) == 0;
            //return BigInteger.Remainder(_item.WorryLevel, TestFactor) == 0;
            return (_item.WorryLevel % TestFactor) == 0;
        }
    }
}
