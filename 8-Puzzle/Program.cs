using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    class Program
    {
        private static readonly string EASY = "134862705";
        private static readonly string MEDIUM = "281043765";
        private static readonly string HARD = "567408321";

        static void Main(string[] args)
        {
            string defaultArgs = MEDIUM;
            Board initialBoard = Board.CreateNew(defaultArgs);
            Node rootNode = new Node(initialBoard, null);

            Console.WriteLine("Initial State: " + initialBoard.ToString());

            if (initialBoard.isInEndState() == false) {

                //gets different answer than uniform... 132 vs 128
                //doBreadthFirstSearch(rootNode);
                //doDepthFirstSearch(rootNode);

                //harder stuff
                //TODO: fix nodes visited for this thing

                //doIterativeDeepening(rootNode);

                //doUniformCost(rootNode);
                doBestFirstSearch(rootNode);


                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        static void doBreadthFirstSearch(Node rootNode)
        {
            Console.WriteLine("Beginning breadth first search");

            Queue<Node> q = new Queue<Node>();
            q.Enqueue(rootNode);

            HashSet<int> alreadySeen = new HashSet<int> { };

            int iterations = 0;
            while (q.Count > 0)
            {
                Node currentNode = q.Dequeue();
                Board currentBoard = currentNode.board;
                alreadySeen.Add(currentBoard.Id);

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
            HashSet<int> alreadySeen = new HashSet<int> { };
            s.Push(rootNode);

            int iterations = 0;
            while (s.Count > 0)
            {
                Node currentNode = s.Pop();
                Board currentBoard = currentNode.board;
                alreadySeen.Add(currentBoard.Id);

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
                    }
                }

                iterations += 1;
            }         
        }

        static void doIterativeDeepening(Node rootNode)
        {
            Console.WriteLine("Beginning iterative deepening");
            HashSet<int> nodesVisited = new HashSet<int>();

            for (int depth = 0; depth < Int32.MaxValue; depth++)
            {
                Node endNode = doDepthLimitedDFS(rootNode, depth, ref nodesVisited);
                if (endNode != null)
                {
                    //we're done
                    return;
                }                
            }
        }

        static Node doDepthLimitedDFS(Node currentNode, 
                                      int depth,
                                      ref HashSet<int> nodesVisited)
        {

            Board currentBoard = currentNode.board;

            if (currentBoard.isInEndState())
            {
                //found endState
                //use this opportunity to write path to console,
                //as it will be rewinded by recursive call.
                handleEndState(currentNode, nodesVisited.Count);
                return currentNode;
            }

            //stop recursing once you're back at the root
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

                nodesVisited.Add(childNode.board.Id);
                if (doDepthLimitedDFS(childNode, depth - 1, ref nodesVisited) != null)
                {
                    return childNode;
                }
                

            }

            return null;
        }

        static void doUniformCost(Node rootNode)
        {
            Console.WriteLine("Beginning uniform cost search...");

            C5.IntervalHeap<Node> q = new C5.IntervalHeap<Node>();
            q.Add(rootNode);

            //repeated state checking tools
            HashSet<int> previouslyExpanded = new HashSet<int> { };
            Dictionary<int, Node> idsToNode = new Dictionary<int, Node> { };

            int iterations = 1;
            while (q.Count > 0)
            {
                Node currentNode = q.DeleteMin();
                if (currentNode.expanded == true)
                {
                    throw new Exception("Ruh roh");
                }
                currentNode.expanded = true;
                Board currentBoard = currentNode.board;

                //keeps track of expanded states
                //we are currently expanding this state.
                previouslyExpanded.Add(currentBoard.Id);
                
                if (currentBoard.isInEndState())
                {
                    handleEndState(currentNode, iterations);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {
                   
                    if (!previouslyExpanded.Contains(child.Id))
                    {
                        int childPotentialCost = currentNode.cost + child.TileMoved;
                        Node childNode = new Node(child,
                                                  currentNode,
                                                  childPotentialCost,
                                                  currentNode.depth + 1);

                        C5.IPriorityQueueHandle<Node> h = null;

                        if (idsToNode.ContainsKey(child.Id))
                        {
                            //already seen this node somewhere. check queue for cheaper, otherwise don't add
                            Node currentNodeInQueue = idsToNode[child.Id];
                            int currentCostInQueue = currentNodeInQueue.cost;
                            if (childPotentialCost < currentCostInQueue)
                            {
                                //replace the node with the same configuration with the cheaper one
                                //update the handle in the queue appropriately for future reference
                                h = currentNodeInQueue.handle;
                                q.Replace(currentNodeInQueue.handle, childNode);
                                childNode.handle = h;
                            } else
                            {
                                //do not add this child, the one in the queue already is cheaper
                            }
                        } else
                        {              
                            //never seen this child before, add it         
                            q.Add(ref h, childNode);
                            childNode.handle = h;
                            idsToNode.Add(child.Id, childNode);
                        }
                        
                        
                    } 
                }

                iterations += 1;

            }
        }

        static void doBestFirstSearch(Node rootNode)
        {
            IComparer<Node> sillyHeuristic = new Heuristic();

            C5.IntervalHeap<Node> q = new C5.IntervalHeap<Node>(sillyHeuristic);
            q.Add(rootNode);

            //repeated state checking tools
            HashSet<int> previouslyExpanded = new HashSet<int> { };
            Dictionary<int, Node> idsToNode = new Dictionary<int, Node> { };

            int iterations = 1;
            while (q.Count > 0)
            {
                Node currentNode = q.DeleteMin();
                if (currentNode.expanded == true)
                {
                    throw new Exception("Ruh roh");
                }
                currentNode.expanded = true;
                Board currentBoard = currentNode.board;

                //keeps track of expanded states
                //we are currently expanding this state.
                previouslyExpanded.Add(currentBoard.Id);

                if (currentBoard.isInEndState())
                {
                    handleEndState(currentNode, iterations);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {

                    if (!previouslyExpanded.Contains(child.Id))
                    {
                        int childPotentialCost = currentNode.cost + child.TileMoved;
                        Node childNode = new Node(child,
                                                  currentNode,
                                                  childPotentialCost,
                                                  currentNode.depth + 1);

                        C5.IPriorityQueueHandle<Node> h = null;

                        if (idsToNode.ContainsKey(child.Id))
                        {
                            //already seen this node somewhere. check queue for cheaper, otherwise don't add
                            Node currentNodeInQueue = idsToNode[child.Id];
                            int currentCostInQueue = currentNodeInQueue.cost;
                            if (childPotentialCost < currentCostInQueue)
                            {
                                //replace the node with the same configuration with the cheaper one
                                //update the handle in the queue appropriately for future reference
                                h = currentNodeInQueue.handle;
                                q.Replace(currentNodeInQueue.handle, childNode);
                                childNode.handle = h;
                            }
                            else
                            {
                                //do not add this child, the one in the queue already is cheaper
                            }
                        }
                        else
                        {
                            //never seen this child before, add it         
                            q.Add(ref h, childNode);
                            childNode.handle = h;
                            idsToNode.Add(child.Id, childNode);
                        }


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
