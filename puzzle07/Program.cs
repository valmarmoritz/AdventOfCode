using System;
using System.Collections.Generic;

namespace puzzle07
{
    class Program
    {
        static void Main(string[] args)
        {
            //Directory directory = new Directory{ Name = "\\" };

            //directory.AddDirectory(new Directory { Name = "a" });

            //directory.AddFile(new File { Size = 14848514, Name = "b.txt"});
            //directory.AddFile(new File { Size = 8504156, Name = "c.dat"});
            
            //directory.AddDirectory(new Directory { Name = "d" });

            //int size = directory.SizeOfFiles;

            // initialize the tree
            Directory directory = new Directory { Name = "/" };

            ReadInput(directory);

            int max_size = 100000;
            Console.WriteLine("Sum of all directories smaller than " + max_size + " : " + SumOfSizes(directory, max_size)); // + directory.SizeWithSubdirectories(max_size));
            Console.WriteLine();

            int total_disk_space = 70000000;
            int min_space_free = 30000000;
            int currently_used = directory.SizeWithSubdirectories();
            int currently_free = total_disk_space - currently_used;
            int need_to_free = min_space_free - currently_free;

            Console.WriteLine("total_disk_space: " + total_disk_space);
            Console.WriteLine("min_space_free: " + min_space_free);
            Console.WriteLine("currently_used: " + currently_used);
            Console.WriteLine("currently_free: " + currently_free);
            Console.WriteLine("need_to_free: " + need_to_free);

            Directory smallest_needed_directory = directory;
            string name_of_smallest_directory_found = directory.Name;
            int size_of_smallest_directory_found = currently_used;

            //smallest_needed_directory = FindSmallestDirectoryNeeded(directory, need_to_free, size_of_smallest_directory_found);

            //Directory FindSmallestDirectoryNeeded(Directory _directory, int _need_to_free, int _smallest_found)
            //{
            //    Directory smallest = _directory;

            //    foreach (Directory d in _directory.Directories)
            //    {
            //        if (d.SizeWithSubdirectories() > _need_to_free && d.SizeWithSubdirectories() < _smallest_found)
            //        {
            //            _smallest_found = d.SizeWithSubdirectories();
            //            smallest = FindSmallestDirectoryNeeded(d, _need_to_free, _smallest_found);
            //        }
            //    }

            //    return smallest;
            //}


            List<Directory> directories = new List<Directory>();
            WalkDirectories(directory);

            void WalkDirectories(Directory _directory)
            {

                if (_directory.Directories != null)
                {
                    foreach (Directory d in _directory.Directories)
                    {
                        directories.Add(d);
                        WalkDirectories(d);
                    }
                }

            }

            foreach (Directory d in directories)
            {
                if (d.SizeWithSubdirectories() > need_to_free && d.SizeWithSubdirectories() < size_of_smallest_directory_found)
                {
                    size_of_smallest_directory_found = d.SizeWithSubdirectories();
                    smallest_needed_directory = d;
                }
            }

            name_of_smallest_directory_found = smallest_needed_directory.Name;
            size_of_smallest_directory_found = smallest_needed_directory.SizeWithSubdirectories();

            Console.WriteLine("Smallest directory that would free enough space is " + name_of_smallest_directory_found + " with size " + size_of_smallest_directory_found);

        }


        static void ReadInput(Directory _directory)
        {
            int _filesize;
            string _filename;
            string _change_to_directory;

            Directory _currentDirectory = _directory;

            // Read a text file line by line.
            string[] lines = System.IO.File.ReadAllLines(@"puzzle07_q1.txt");
            foreach (string line in lines)
            {
                //Console.WriteLine(line);
                
                if (line.StartsWith("$"))
                {
                    // this is a command
                    if (line.StartsWith("$ cd"))
                    {
                        // this changes directories
                        _change_to_directory = line.Substring(5, line.Length - 5);
                        if (_change_to_directory == "/")
                        {
                            // change to root directory
                            _currentDirectory = _directory;
                        }
                        else if (_change_to_directory == "..")
                        {
                            // change to parent directory
                            _currentDirectory = _currentDirectory.ParentDirectory;
                        }
                        else
                        {
                            // change to subdirectory
                            _currentDirectory = _currentDirectory.Directories.Find(x => x.Name == _change_to_directory);
                        }
                    }
                    else if (line.StartsWith("$ ls"))
                    {
                        // this lists files and subdirectories
                    }
                }
                else if (line.StartsWith("dir"))
                {
                    // this is a new subdirectory
                    _currentDirectory.AddDirectory(new Directory 
                        { 
                            Name = line.Substring(4, line.Length - 4),
                            ParentDirectory = _currentDirectory
                        });
                }
                else
                {
                    // this is a new file
                    int.TryParse(line.Split(" ")[0], out _filesize);
                    _filename = line.Split(" ")[1];
                    _currentDirectory.AddFile(new File { Size = _filesize, Name = _filename });
                }
            }
        }

        static int SumOfSizes(Directory _directory, int _max_size)
        {
            int s = 0;
            if (_directory.SizeWithSubdirectories() < _max_size) 
            { 
                s += _directory.SizeWithSubdirectories();
                //Console.WriteLine(_directory.Name + ": " + _directory.SizeWithSubdirectories() + " = " + s);
            }

            foreach (Directory d in _directory.Directories)
            {
                s += SumOfSizes(d, _max_size);  
                //Console.WriteLine(d.Name + ": " + SumOfSizes(d, _max_size) + " = " + s);
            }

            return s;
        }

    }
}
