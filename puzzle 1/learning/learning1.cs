using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace learning
{
    class Stack
    {
        public int Id;
        public string crate;
        public LinkedList<string> crates;

        static void Learning_Main()
        {
            List<Stack> stacks = new List<Stack>();

            stacks.Add(new Stack { Id = 1, crates = new LinkedList<string>() });
            stacks.Find(x => x.Id == 1).crates.AddLast("Z");
            stacks.Find(x => x.Id == 1).crates.AddLast("N");

            stacks.Add(new Stack { Id = 2, crates = new LinkedList<string>() });
            stacks.Find(x => x.Id == 2).crates.AddLast("M");
            stacks.Find(x => x.Id == 2).crates.AddLast("C");
            stacks.Find(x => x.Id == 2).crates.AddLast("D");

            stacks.Add(new Stack { Id = 3, crates = new LinkedList<string>() });
            stacks.Find(x => x.Id == 3).crates.AddLast("P");

            foreach (var s in stacks)
            {
                Console.Write("Stack: " + s.Id + ", Crates: ");
                foreach (var c in s.crates)
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }

            Console.Write("Top crates: ");
            foreach (var s in stacks)
            {
                Console.Write(s.crates.Last());
            }
            Console.WriteLine();

            Move(1, 2, 1);
            Move(3, 1, 3);
            Move(2, 2, 1);
            Move(1, 1, 2);

            foreach (var s in stacks)
            {
                Console.Write("Stack: " + s.Id + ", Crates: ");
                foreach (var c in s.crates)
                {
                    Console.Write(c + " ");
                }
                Console.WriteLine();
            }

            Console.Write("Top crates: ");
            foreach (var s in stacks)
            {
                Console.Write(s.crates.Last());
            }
            Console.WriteLine();

            void Move(int _cnt, int _from, int _to)
            {
                string c;
                
                // first puzzle: move one by one
                //for (int i = 0; i < _cnt; i++)
                //{
                //    c = stacks.Find(x => x.Id == _from).crates.Last();
                //    stacks.Find(x => x.Id == _to).crates.AddLast(c);
                //    stacks.Find(x => x.Id == _from).crates.RemoveLast();
                //}

                // second puzzle: move all at once
                var cs = new LinkedList<string>();
                for (int i = 0; i < _cnt; i++)
                {
                    c = stacks.Find(x => x.Id == _from).crates.Last();
                    cs.AddLast(c);
                    stacks.Find(x => x.Id == _from).crates.RemoveLast();
                }
                for (int i = 0; i < _cnt; i++)
                {
                    c = cs.Last();
                    stacks.Find(x => x.Id == _to).crates.AddLast(c);
                    cs.RemoveLast();
                }
            }
        }

    }
}
