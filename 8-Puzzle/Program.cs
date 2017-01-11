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
            string defaultArgs = "281043765";//"134862705";
            Board initialBoard = Board.CreateNew(defaultArgs);
            
    
            Console.WriteLine("Initial State: " + initialBoard.ToString());

            if (initialBoard.isInEndState() == false) {
                
                

                doBreadthFirstSearch(initialBoard);

                doDepthFirstSearch(initialBoard);
            }
        }

        static void doBreadthFirstSearch(Board board)
        {
            C5.IntervalHeap<Node> heap = new C5.IntervalHeap<Node>();
            Node newNode = new Node(board);
            heap.Add(newNode);

            HashSet<int> alreadySeen = new HashSet<int> { board.Id };
            Queue<Board> q = new Queue<Board>();
            q.Enqueue(board);

            int iterations = 1;
            while (q.Count > 0)
            {
                Board currentBoard = q.Dequeue();

                Console.WriteLine("Current board being considered: " + currentBoard.ToString());

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

                    //Console.WriteLine("There are now: " + q.Count + " nodes in the list.");
                    iterations += 1;    
                }
            }
        }

        static void doDepthFirstSearch(Board board)
        {
            HashSet<int> alreadySeen = new HashSet<int> { board.Id };
            Stack<Board> s = new Stack<Board>();
            s.Push(board);

            int iterations = 1;
            while (s.Count > 0)
            {
                Board currentBoard = s.Pop();

                Console.WriteLine("Current board being considered: " + currentBoard.ToString());

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
                            s.Push(child);
                            alreadySeen.Add(child.Id);
                        }
                    }

                    //Console.WriteLine("There are now: " + q.Count + " nodes in the list.");
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
