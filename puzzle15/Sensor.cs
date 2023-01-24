using System;

namespace puzzle15
{
    internal class Sensor
    {
        public (int, int) Coordinates { get; set; }
        public (int, int) ClosestBeacon { get; set; }
        public int BeaconFreeManhattanDistance { get { return Math.Abs(ClosestBeacon.Item1 - Coordinates.Item1) + Math.Abs(ClosestBeacon.Item2 - Coordinates.Item2); } }
    }
}