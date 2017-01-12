using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    class Program
    {
        private static readonly int MAX_DEPTH = 5;
        private static readonly string EASY = "134862705";
        private static readonly string MEDIUM = "281043765";

        static void Main(string[] args)
        {
            string defaultArgs = EASY;
            Board initialBoard = Board.CreateNew(defaultArgs);
            Node rootNode = new Node(initialBoard, null);

            Console.WriteLine("Initial State: " + initialBoard.ToString());

            if (initialBoard.isInEndState() == false) {
              
                doBreadthFirstSearch(rootNode);
                doDepthFirstSearch(rootNode);

                //harder stuff
                //TODO: fix nodes visited for this thing
                
                doIterativeDeepening(rootNode);
            }
        }

        static void doBreadthFirstSearch(Node rootNode)
        {
            Console.WriteLine("Beginning breadth first search");

            Queue<Node> q = new Queue<Node>();
            q.Enqueue(rootNode);

            HashSet<int> alreadySeen = new HashSet<int> { rootNode.board.Id };

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

        static void doDepthFirstSearch(Node rootNode)
        {
            Console.WriteLine("Beginning depth first search");

            Stack<Node> s = new Stack<Node>();
            HashSet<int> alreadySeen = new HashSet<int> { rootNode.board.Id };
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

        static void doIterativeDeepening(Node rootNode)
        {
            Console.WriteLine("Beginning iterative deepening");

            for (int depth = 0; depth < Int32.MaxValue; depth++)
            {
                Node endNode = doDepthLimitedDFS(rootNode, depth);
                if (endNode != null)
                {
                    //we're done
                    return;
                }                
            }
        }

        static Node doDepthLimitedDFS(Node currentNode, 
                                         int depth)
        {

            Board currentBoard = currentNode.board;
            if (currentBoard.isInEndState())
            {
                //found endState
                //use this opportunity to write path to console,
                //as it will be rewinded by recursive call.
                handleEndState(currentNode, currentNode.depth);
                return currentNode;
            }

            if (depth <= 0)
            {
                return null;
            }

            List<Board> children = currentBoard.GenerateNextStates();

            foreach (Board child in children)
            {
                Node childNode = new Node(child,
                                        currentNode,
                                        currentNode.cost + child.TileMoved,
                                        currentNode.depth + 1);

                if (doDepthLimitedDFS(childNode, depth - 1) != null)
                {
                    return childNode;
                }
            }

            return null;
        }


        //TODO: make sure the repeated state checking has both
        //isExpanded, and cost is less, two things
        static void doUniformCost(Node rootNode)
        {
            C5.IntervalHeap<Node> q = new C5.IntervalHeap<Node>();
            q.Add(rootNode);

            HashSet<int> alreadySeen = new HashSet<int> { rootNode.board.Id };

            int iterations = 1;
            while (q.Count > 0)
            {
                Node currentNode = q.DeleteMin();
                Board currentBoard = currentNode.board;

                Console.WriteLine("Current board being considered: " + currentBoard.ToString());

                Console.WriteLine("Child no. " + iterations);

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

                        writeBoardInfo(childNode);

                        q.Add(childNode);
                        alreadySeen.Add(child.Id);
                    }
                }

                iterations += 1;

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
