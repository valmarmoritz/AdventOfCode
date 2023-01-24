using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace puzzle07
{
    class Directory
    {
        public string Name { get; set; }
        public Directory ParentDirectory { get; set; }
        public List<File> Files = new List<File>();
        public List<Directory> Directories = new List<Directory>();
        public int SizeOfFiles = 0;

        public void AddFile(File _file)
        {
            Files.Add(_file);
            SizeOfFiles += _file.Size;
        }

        public void AddDirectory(Directory _directory)
        {
            Directories.Add(_directory);
        }

        public int SizeWithSubdirectories()
        {
            int c = 0;
            //Console.WriteLine("    start summing for directory " + this.Name);
            int s = this.SizeOfFiles;

            foreach (Directory d in this.Directories)
            {
                //Console.WriteLine("        " + d.Name + ": " + d.SizeWithSubdirectories());
                s += d.SizeWithSubdirectories();
                c += 1;
            }
            //Console.WriteLine("    " + this.Name + " size with " + c + " subdirectories: " + s);
            return s;
        }
        //public int SizeWithSubdirectories(int _max_size)
        //{
        //    Console.WriteLine("    start summing for directory " + this.Name);
        //    int s = this.SizeOfFiles;

        //    foreach (Directory d in this.Directories)
        //    {
        //        Console.WriteLine("            " + d.Name + ": " + d.SizeWithSubdirectories());
        //        s += d.SizeWithSubdirectories();
        //    }
        //    Console.WriteLine("    " + this.Name + " size with subdirectories: " + s);

        //    if (s < _max_size) { return s; }
        //    else { return 0; }
        //}
    }
}
