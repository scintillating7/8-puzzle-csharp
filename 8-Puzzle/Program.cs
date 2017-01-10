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
            string defaultArgs = "134862705";
            Board initialBoard = Board.CreateNew(defaultArgs);
            
    
            Console.WriteLine("Initial State: " + initialBoard.ToString());

            if (initialBoard.isInEndState() == false) {
                
                doBreadthFirstSearch(initialBoard);

                
            }
        }

        static void doBreadthFirstSearch(Board board)
        {
            HashSet<int> alreadySeen = new HashSet<int> { board.Id };
            Queue<Board> q = new Queue<Board>();
            q.Enqueue(board);

            int iterations = 1;
            while (q.Count > 0)
            {
                Board currentBoard = q.Dequeue();
                if (currentBoard != null)
                {
                    Console.WriteLine("Iteration no. " + iterations);

                    if (currentBoard.isInEndState())
                    {
                        handleEndState(currentBoard);
                        return;
                    }

                    List<Board> children = currentBoard.generateNextStates();

                    foreach (Board child in children)
                    {
                        if (!alreadySeen.Contains(child.Id))
                        {
                            q.Enqueue(child);
                            alreadySeen.Add(child.Id);
                        }
                    }

                    Console.WriteLine("There are now: " + q.Count + " nodes in the list.");
                    iterations += 1;    
                }
            }
        }
        
        static void handleEndState(Board endBoard)
        {
            Console.WriteLine("End state found! " + endBoard.ToString());
        }
    }
}
