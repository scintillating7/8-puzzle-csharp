using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            string defaultArgs = "1 3 4 8 6 2 7 0 5";
            Board initialBoard = Board.CreateNew(defaultArgs);
    
            Console.WriteLine("Initial State: " + initialBoard.ToString());
        }
    }
}
