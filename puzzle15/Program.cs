using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle15
{
    static class Program
    {

        static void Main(string[] args)
        {

            bool learning = false;

            List<Sensor> TunnelMap = new List<Sensor>();
            Dictionary<(int, int), char> Map = new Dictionary<(int, int), char>();

            Console.Write("Reading input: ");
            ReadInput(TunnelMap, learning);
            Console.WriteLine("done.");

            Console.Write("Adding sensors and beacons to map: ");
            ReadSensorsAndBeaconsFromTunnelMap(TunnelMap, Map);
            Console.WriteLine("done.");

            if (learning)
            {
                DrawMap(Map);
                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to continue...");
                Console.ReadLine();
            }

            int row;
            // learing: 10
            // puzzle: 2000000
            if (learning) row = 10;
            else row = 2000000;

            int x_min = 0;
            int x_max;
            int y_min = 0;
            int y_max;

            if (learning)
            {
                x_max = 20;
                y_max = 20;
            }
            else
            {
                x_max = 4000000;
                y_max = 4000000;

                // testing knowing that the correct answer is: (2949122, 3041245)
                //y_min = 3041000;
            }

            int[] area = { x_min, x_max, y_min, y_max };

            Console.WriteLine("Determining no beacon positions on row y=" + row + ": ");
            FindSureNoBeaconsOnRow(TunnelMap, Map, row,learning);
            Console.WriteLine("done.");

            if (learning)
            {
                DrawMap(Map);
                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to continue...");
                Console.ReadLine();
            }

            Console.Write("Counting no beacon positions on row y=" + row + ": ");
            int noBeaconsOnRowCount = CountNoBeaconsOnRow(Map, row);
            Console.WriteLine("done.");

            Console.WriteLine();
            Console.WriteLine("There are " + noBeaconsOnRowCount + " NO Beacon positions on row y=" + row);

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to continue...");
            Console.ReadLine();


            Console.WriteLine();
            Console.WriteLine("Determining no beacon positions in coordinates x= " + x_min.ToString("N0") + "..." + x_max.ToString("N0") + " y= " + y_min.ToString("N0") + " ... " + y_max.ToString("N0") + ": ");
            // cleanup after phase 1 to reduce memory impact
            RemoveFoundNoBeaconsFromRow(Map, row);
            //(int, int) BeaconPosition = FindFirstUndetectedBeaconInArea(TunnelMap, Map, area, learning);
            (int, int) BeaconPosition = FindFirstUndetectedBeaconInArea2(TunnelMap, Map, area, learning);
            Console.WriteLine(1.ToString("P") + " done.");
            Console.WriteLine("The address of the first not detected beacon in this area is " + BeaconPosition);
            Console.WriteLine("The tuning frequency of this beacon is " + BeaconTuningFrequencyCalc(BeaconPosition));

            Console.WriteLine();

            if (learning)
            {
                DrawMap(Map);
                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to continue...");
                Console.ReadLine();
            }

        }

        private static (int, int) FindFirstUndetectedBeaconInArea2(List<Sensor> tunnelMap, Dictionary<(int, int), char> map, int[] area, bool learning)
        {
            int sensors_count = tunnelMap.Count;
            int sensors_checked;
            (int, int) result = (0, 0);
            int row_length = area[1] - area[0] + 1;

            List<((int, int), int)> sensors = new List<((int, int), int)>();
            foreach (Sensor sensor in tunnelMap.OrderByDescending(x => x.BeaconFreeManhattanDistance))
            {
                sensors.Add(((sensor.Coordinates.Item1, sensor.Coordinates.Item2), sensor.BeaconFreeManhattanDistance));
            }

            var sensorArray = sensors.ToArray();

            // don't update map, just determine for each point in area if any sensor can see it
            for (int y = area[2]; y <= area[3]; y++)
            {
                Console.WriteLine("Checking row: " + y.ToString("N0") + " of { " + area[2].ToString("N0") + " - " + area[3].ToString("N0") + " }, " + (y * 1.0 / row_length).ToString("P") + " done");

                for (int x = area[0]; x <= area[1]; x++)
                {
                    // don't check places where there is either a beacon or a sensor
                    //if (!map.ContainsKey((x, y)))
                    // this seems to be a resource hog, let's try not checking this
                    if (true)
                        {
                        sensors_checked = 0;

                        // look through all sensors by the BeaconFreeManhattanDistance in descending order to increase probability of finding a match as early as possible
                        foreach (var sensor in sensorArray)
                        {
                            if (Math.Abs(x - sensor.Item1.Item1) + Math.Abs(y - sensor.Item1.Item2) <= sensor.Item2)
                            {
                                // we have found that this point is in reach of at least one sensor, no need to check other sensors
                                break;
                            }

                            sensors_checked++;
                        }
                        
                        // if we have reached here, all sensors have been checked and no one has been found to be within reach
                        if (sensors_checked == sensors_count)
                        { 
                            result = (x, y);

                            // knowing that there is only one possible answer, we can stop looking
                            y = area[3] + 1;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private static (int, int) FindFirstUndetectedBeaconInArea(List<Sensor> tunnelMap, Dictionary<(int, int), char> map, int[] area, bool learning)
        {
            int row = 0;
            int col = 0;

            int row_length = area[1] - area[0] + 1;

            for (int y = area[2]; y <= area[3]; y++)
            {
                Console.WriteLine("Checking row: " + y + " of { " + area[2].ToString("N0") + " - " + area[3].ToString("N0") + " }, " + (y * 1.0 / row_length).ToString("P") + " done");

                // remove data for previous row to avoid System.OutOfMemoryException
                RemoveFoundNoBeaconsFromRow(map, y - 1);

                FindSureNoBeaconsOnRow(tunnelMap, map, y, learning);

                if (map.Where(x => x.Key.Item1 >= area[0] && x.Key.Item1 <= area[1] && x.Key.Item2 == y).Count() < row_length)
                {
                    row = y;
                    Console.WriteLine();
                    Console.WriteLine("Row with empty cells: " + row);

                    for (int x = area[0]; x <= area[1]; x++)
                    { 
                        if (!map.ContainsKey((x, row)))
                        {
                           col = x;
                        }
                    }
                    Console.WriteLine("Column with empty cells: " + col);

                    break;
                }; 
            }

            return (col, row);
        }

        private static void RemoveFoundNoBeaconsFromRow(Dictionary<(int, int), char> map, int row)
        {
            foreach (var c in map.Where(x => x.Key.Item2 == row && x.Value == '#'))
            {
                map.Remove(c.Key);
            }

            return;
        }

        private static void FindSureNoBeaconsOnRow(List<Sensor> tunnelMap, Dictionary<(int, int), char> map, int row, bool learning)
        {
            int beaconFreeManhattanDistance = 0;
            int sensor_x = 0;
            int sensor_y = 0;
            int x_min = 0;
            int x_max = 0;
            int sensorCounter = 0;
            int loopFraction = 0;


            // loop through all sensors 
            foreach (Sensor s in tunnelMap)
            {
                sensorCounter += 1;
                sensor_x = s.Coordinates.Item1;
                sensor_y = s.Coordinates.Item2;

                beaconFreeManhattanDistance = Math.Abs(s.ClosestBeacon.Item1 - sensor_x) + Math.Abs(s.ClosestBeacon.Item2 - sensor_y);

                loopFraction = 0;

                if (learning)
                    Console.Write("S" + sensorCounter + ": ");

                x_min = sensor_x - beaconFreeManhattanDistance;
                x_max = sensor_x + beaconFreeManhattanDistance;

                for (int x = x_min; x <= x_max; x++)
                {
                    if (learning)
                        if ((x - x_min) * 1.0 / (x_max - x_min) * 100 > loopFraction)
                        {
                            loopFraction += 1;
                            if (loopFraction % 10 == 0)
                            {
                                Console.Write(loopFraction / 10);
                            }
                            else
                            {
                                Console.Write('.');
                            }
                        }

                    if (Math.Abs(x - sensor_x) + Math.Abs(row - sensor_y) <= beaconFreeManhattanDistance)
                    {
                        if (!map.ContainsKey((x, row))) { map[(x, row)] = '#'; }
                    }
                }

                if (learning)
                    Console.WriteLine();
            }

            return;
        }

        private static void DrawMap(Dictionary<(int, int), char> map)
        {
            int x_min = 0;
            int x_max = 0;
            int y_min = 0;
            int y_max = 0;

            foreach (var loc in map.Keys)
            {
                if (loc.Item1 < x_min) x_min = loc.Item1;
                if (loc.Item1 > x_max) x_max = loc.Item1;
                if (loc.Item2 < y_min) y_min = loc.Item2;
                if (loc.Item2 > y_max) y_max = loc.Item2;
            }

            Console.Write("   ");
            for (int x = x_min; x <= x_max; x++)
            {
                if (x % 5 == 0)
                {
                    Console.Write(x / 10);
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();

            Console.Write("   ");
            for (int x = x_min; x <= x_max; x++)
            {
                if (x % 5 == 0)
                {
                    Console.Write(x % 10);
                }
                else
                {
                    Console.Write(' ');
                }
            }
            Console.WriteLine();


            for (int y = y_min; y <= y_max; y++)
            {
                if (y < 0)
                {
                    Console.Write(y.ToString("D1") + ' ');
                }
                else
                {
                    Console.Write(y.ToString("D2") + ' ');
                }

                for (int x = x_min; x <= x_max; x++)
                {
                    if (map.ContainsKey((x, y)))
                    {
                        Console.Write(map[(x, y)]);
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        private static void ReadSensorsAndBeaconsFromTunnelMap(List<Sensor> tunnelMap, Dictionary<(int, int), char> map)
        {
            foreach(Sensor sensor in tunnelMap) 
            {
                map[sensor.Coordinates] = 'S';

                if (!map.ContainsKey(sensor.ClosestBeacon))
                {
                    map[sensor.ClosestBeacon] = 'B';
                }
            }
        }

        private static long BeaconTuningFrequencyCalc((int, int) beaconTuningFrequency)
        {
            return ((long)beaconTuningFrequency.Item1 * 4000000 + (long)beaconTuningFrequency.Item2);
        }

        private static int CountNoBeaconsOnRow(Dictionary<(int, int), char> map, int row)
        {
            return map.Where(x => x.Key.Item2 == row && x.Value == '#').Count();
        }

        private static void ReadInput(List<Sensor> sensorMap, bool learning)
        {
            string[] lines;
            // Read a text file line by line.
            if (learning) lines = System.IO.File.ReadAllLines(@"puzzle15_learning.txt");
            else lines = System.IO.File.ReadAllLines(@"puzzle15.txt");

            List<string> words;

            foreach (string line in lines)
            {
                words = line
                    .Split(' ')
                    .ToList();

                sensorMap.Add(new Sensor()
                {
                    Coordinates = (int.Parse(words[2].Substring(2, words[2].Length - 3)), int.Parse(words[3].Substring(2, words[3].Length - 3))),
                    ClosestBeacon = (int.Parse(words[8].Substring(2, words[8].Length - 3)), int.Parse(words[9].Substring(2, words[9].Length - 2))),
                }
                );
            }

        }
    }
}
