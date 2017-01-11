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
            Console.WriteLine("Beginning breadth first search");

            Queue<Node> q = new Queue<Node>();
            Node newNode = new Node(board, null);
            q.Enqueue(newNode);

            HashSet<int> alreadySeen = new HashSet<int> { board.Id };

            int iterations = 0;
            while (q.Count > 0)
            {
                Node currentNode = q.Dequeue();
                Board currentBoard = currentNode.board;

                if (currentBoard.isInEndState())
                {
                    handleEndState(currentNode,
                                    iterations);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {
                    if (!alreadySeen.Contains(child.Id))
                    {
                        Node childNode = new Node(child, 
                                                  currentNode,
                                                  currentNode.cost + child.TileMoved,
                                                  currentNode.depth + 1);

                        q.Enqueue(childNode);
                        alreadySeen.Add(child.Id);
                    }
                }
                    
                iterations += 1;    
                
            }
        }

        static void doDepthFirstSearch(Board board)
        {
            Console.WriteLine("Beginning depth first search");

            Stack<Node> s = new Stack<Node>();
            HashSet<int> alreadySeen = new HashSet<int> { board.Id };
            Node rootNode = new Node(board, null);
            s.Push(rootNode);

            int iterations = 0;
            while (s.Count > 0)
            {
                Node currentNode = s.Pop();
                Board currentBoard = currentNode.board;

                if (currentBoard.isInEndState())
                {
                    handleEndState(currentNode, iterations);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {
                    if (!alreadySeen.Contains(child.Id))
                    {
                        Node childNode = new Node(child,
                                                  currentNode,
                                                  currentNode.cost + child.TileMoved,
                                                  currentNode.depth + 1);
                        s.Push(childNode);
                        alreadySeen.Add(child.Id);
                    }
                }

                iterations += 1;
            }         
        }

        static void doBreadthFirstSearchWithHeuristic(Board board)
        {
            C5.IntervalHeap<Node> q = new C5.IntervalHeap<Node>();
            Node newNode = new Node(board, null);
            q.Add(newNode);

            HashSet<int> alreadySeen = new HashSet<int> { board.Id };

            int iterations = 1;
            while (q.Count > 0)
            {
                Node currentNode = q.DeleteMin();
                Board currentBoard = currentNode.board;

                Console.WriteLine("Current board being considered: " + currentBoard.ToString());


                if (currentBoard != null)
                {
                    Console.WriteLine("Child no. " + iterations);

                    if (currentBoard.isInEndState())
                    {
                        //handleEndState(currentBoard, iterations);
                        return;
                    }

                    List<Board> children = currentBoard.GenerateNextStates();

                    foreach (Board child in children)
                    {
                        if (!alreadySeen.Contains(child.Id))
                        {
                            //q.Enqueue(child);
                            Node childNode = new Node(child,
                                                      currentNode,
                                                      currentNode.cost + child.TileMoved,
                                                      currentNode.depth + 1);

                            writeBoardInfo(childNode);

                            q.Add(childNode);
                            alreadySeen.Add(child.Id);
                        }
                    }

                    //Console.WriteLine("There are now: " + q.Count + " nodes in the list.");
                    iterations += 1;
                }
            }
        }

        static void handleEndState(Node endBoardNode,
                                   int iterations)
        {
            Stack<Node> parentStack = new Stack<Node>();
            parentStack.Push(endBoardNode);
            
            while (endBoardNode.Parent != null)
            {
                parentStack.Push(endBoardNode.Parent);
                endBoardNode = endBoardNode.Parent;
            }

            while (parentStack.Count > 0)
            {
                Node currentNodeInPath = parentStack.Pop();
                writeBoardInfo(currentNodeInPath);                
            }
            Console.WriteLine("Total nodes considered: " + iterations);
        }

        static void writeBoardInfo(Node currentNode)
        {
            StringBuilder sb = new StringBuilder();
            Board currentBoard = currentNode.board;

            //it only makes sense to write out the move info for nodes that are not the root
            if (currentNode.depth > 0)
            {
                switch (currentBoard.Direction)
                {
                    case 0:
                        sb.Append("UP, ");
                        break;
                    case 1:
                        sb.Append("DOWN, ");
                        break;
                    case 2:
                        sb.Append("RIGHT, ");
                        break;
                    case 3:
                        sb.Append("LEFT, ");
                        break;
                    default:
                        throw new Exception("Invalid direction");
                }

                sb.Append("cost = " + currentBoard.TileMoved);
                sb.Append(" total cost = " + currentNode.cost);

                Console.WriteLine(sb.ToString());
            }
            Console.WriteLine(currentBoard.ToString());
        }
    }
}
