using System;
using System.Collections.Generic;

namespace puzzle10
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Signal> signals = new List<Signal>();
            List<Register> register = new List<Register>();

            ReadSignals(signals);

            int _cycle = 0;
            int _X = 1;

            foreach (Signal s in signals)
            {
                ProcessSignal(s);
            }

            void ProcessSignal(Signal _s)
            {
                _cycle += 1;
                register.Add(new Register { cycle = _cycle, X = _X }) ;
                switch (_s.op)
                {
                    case "noop":
                        break;
                    case "addx":
                        _cycle += 1;
                        register.Add(new Register { cycle = _cycle, X = _X }) ;
                        _X += _s.mv;
                        break;
                }
            }

            int sum_x = SumCertainXValues(register);

            Console.WriteLine("Sum of certain signal strengths: " + sum_x);

            List<Sprite> sprites = new List<Sprite>();
            foreach (Register r in register)
            {
                sprites.Add(new Sprite { pixel = r.cycle, draw = "." });
            }

            CalculateSprites(signals, sprites);

            DrawScreen(sprites);

        }

        private static void DrawScreen(List<Sprite> _sprites)
        {
            Console.WriteLine();
            foreach (Sprite s in _sprites)
            {
                Console.Write(s.draw);
                if (s.pixel  % 40 == 0) Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void CalculateSprites(List<Signal> _signals, List<Sprite> _sprites)
        {
            int _sprite_pos = 1; 
            int _pixel_pos = 0;

            foreach (Signal sig in _signals)
            {
                
                if (_sprite_pos - 1 <= _pixel_pos % 40 && _pixel_pos % 40 <= _sprite_pos + 1)
                {
                    _sprites[_pixel_pos].draw = "X";
                }

                switch (sig.op)
                {
                    case "noop":
                        break;
                    case "addx":
                        _pixel_pos += 1;
                        if (_sprite_pos - 1 <= _pixel_pos % 40 && _pixel_pos% 40 <= _sprite_pos + 1)
                        {
                            _sprites[_pixel_pos].draw = "X";
                        }
                        break;
                }

                _sprite_pos += sig.mv;
                _pixel_pos += 1;
            }
        }

        private static int SumCertainXValues(List<Register> _register)
        {
            int s = 0;

            foreach(var r in _register)
            {
                if ((r.cycle + 20) % 40 == 0)
                {
                    Console.WriteLine("Cycle: " + r.cycle + " Signal: " + r.X + " Signal strength: " + r.cycle * r.X);
                    s += r.cycle * r.X;
                }
            }

            return s;
        }

        private static void ReadSignals(List<Signal> _signals)
        {
            /*
            // learning
            _signals.Add(new Signal { op = "addx", mv = 15 });
            _signals.Add(new Signal { op = "addx", mv = -11 });
            _signals.Add(new Signal { op = "addx", mv = 6 });
            _signals.Add(new Signal { op = "addx", mv = -3 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = -8 });
            _signals.Add(new Signal { op = "addx", mv = 13 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = -35 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 24 });
            _signals.Add(new Signal { op = "addx", mv = -19 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 16 });
            _signals.Add(new Signal { op = "addx", mv = -11 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 21 });
            _signals.Add(new Signal { op = "addx", mv = -15 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -3 });
            _signals.Add(new Signal { op = "addx", mv = 9 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = -3 });
            _signals.Add(new Signal { op = "addx", mv = 8 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -36 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 7 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 6 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 7 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -13 });
            _signals.Add(new Signal { op = "addx", mv = 13 });
            _signals.Add(new Signal { op = "addx", mv = 7 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = -33 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 8 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 17 });
            _signals.Add(new Signal { op = "addx", mv = -9 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = -3 });
            _signals.Add(new Signal { op = "addx", mv = 11 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -13 });
            _signals.Add(new Signal { op = "addx", mv = -19 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = 26 });
            _signals.Add(new Signal { op = "addx", mv = -30 });
            _signals.Add(new Signal { op = "addx", mv = 12 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -9 });
            _signals.Add(new Signal { op = "addx", mv = 18 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 9 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -37 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 15 });
            _signals.Add(new Signal { op = "addx", mv = -21 });
            _signals.Add(new Signal { op = "addx", mv = 22 });
            _signals.Add(new Signal { op = "addx", mv = -6 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -10 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 20 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -6 });
            _signals.Add(new Signal { op = "addx", mv = -11 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            */
            
            // puzzle input
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 30 });
            _signals.Add(new Signal { op = "addx", mv = -24 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -4 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -38 });
            _signals.Add(new Signal { op = "addx", mv = 9 });
            _signals.Add(new Signal { op = "addx", mv = -4 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -17 });
            _signals.Add(new Signal { op = "addx", mv = 22 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = -36 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -5 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 10 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 7 });
            _signals.Add(new Signal { op = "addx", mv = 1 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -38 });
            _signals.Add(new Signal { op = "addx", mv = 39 });
            _signals.Add(new Signal { op = "addx", mv = -32 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -1 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = -26 });
            _signals.Add(new Signal { op = "addx", mv = 31 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = -18 });
            _signals.Add(new Signal { op = "addx", mv = 19 });
            _signals.Add(new Signal { op = "addx", mv = -38 });
            _signals.Add(new Signal { op = "addx", mv = 7 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 34 });
            _signals.Add(new Signal { op = "addx", mv = -39 });
            _signals.Add(new Signal { op = "addx", mv = 8 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 10 });
            _signals.Add(new Signal { op = "addx", mv = -5 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 11 });
            _signals.Add(new Signal { op = "addx", mv = -6 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = -2 });
            _signals.Add(new Signal { op = "addx", mv = -38 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 5 });
            _signals.Add(new Signal { op = "addx", mv = 11 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -11 });
            _signals.Add(new Signal { op = "addx", mv = 16 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "addx", mv = 2 });
            _signals.Add(new Signal { op = "addx", mv = 8 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = 4 });
            _signals.Add(new Signal { op = "addx", mv = 3 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "addx", mv = -20 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            _signals.Add(new Signal { op = "noop", mv = 0 });
            
        }
    }
}
