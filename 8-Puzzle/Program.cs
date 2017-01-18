using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace _8_Puzzle
{
    class Program
    {
        #region Global Variables

        private static readonly string EASY = "134862705";
        private static readonly string MEDIUM = "281043765";
        private static readonly string HARD = "567408321";

        #endregion

        #region Main

        static void Main(string[] args)
        {
            //user-input args for creating initial board
            string defaultArgs = getArgs();           
           
            //create initial board
            Board initialBoard = Board.CreateNew(defaultArgs);
            Node rootNode = new Node(initialBoard, null);
            Console.WriteLine("Initial State: " + initialBoard.ToString());

            //if initial board not in end state, do user-input search
            if (initialBoard.isInEndState() == false) {

                String entry = getSearchRequested();

                switch (entry)
                {
                    case "DFS":
                        doDepthFirstSearch(rootNode);
                        break;
                    case "BFS":
                        doBreadthFirstSearch(rootNode);
                        break;
                    case "IDS":
                        doIterativeDeepening(rootNode);
                        break;
                    case "UCS":
                        doUniformCostShort(rootNode);
                        break;
                    case "GBF":
                        doBestFirstSearchShort(rootNode);
                        break;
                    case "A*1":
                        doAStarOneShort(rootNode);
                        break;
                    case "A*2":
                        doAStarTwoShort(rootNode);
                        break;
                    case "A*3":
                        doAStar3Short(rootNode);
                        break;
                    default:
                        Console.WriteLine("Enter one of the listed options");
                        break;
                }

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        #endregion

        #region Search Algorithms

        static void doBreadthFirstSearch(Node rootNode)
        {
            //standard beginning code
            Console.WriteLine("Beginning breadth first search");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //BFS uses first-in, first-out queue
            Queue<Node> q = new Queue<Node>();
            q.Enqueue(rootNode);

            HashSet<int> previouslyExpanded = new HashSet<int> { };

            int iterations = 0;
            while (q.Count > 0)
            {
                Node currentNode = q.Dequeue();
                Board currentBoard = currentNode.board;
                previouslyExpanded.Add(currentBoard.Id);

                if (currentBoard.isInEndState())
                {
                    //capture time elapsed by algorithm, we're done
                    timer.Stop();
                    long milliseconds = timer.ElapsedMilliseconds;
                    handleEndState(currentNode,
                                    iterations,
                                    milliseconds);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {
                    if (!previouslyExpanded.Contains(child.Id))
                    {
                        Node childNode = new Node(child, 
                                                  currentNode,
                                                  currentNode.cost + child.TileMoved,
                                                  currentNode.depth + 1);

                        q.Enqueue(childNode);
                       
                    } 
                }
                    
                iterations += 1;    
                
            }
        }

        static void doDepthFirstSearch(Node rootNode)
        {
            Console.WriteLine("Beginning depth first search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //DFS uses first-in, last-out stack
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
                    timer.Stop();
                    long milliseconds = timer.ElapsedMilliseconds;
                    handleEndState(currentNode, iterations, milliseconds);
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

        /// <summary>
        /// recursively do DFS to a depth of 1, 2, 3, etc.
        /// </summary>
        /// <param name="rootNode"></param>
        static void doIterativeDeepening(Node rootNode)
        {
            Console.WriteLine("Beginning iterative deepening...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            int nodesVisited = 0;

            for (int depth = 0; depth < Int32.MaxValue; depth++)
            {
                Node endNode = doDepthLimitedDFS(rootNode, 
                                                 depth, 
                                                 timer,
                                                 ref nodesVisited);
                if (endNode != null)
                {
                    //we're done
                    return;
                }                
            }
        }

        static Node doDepthLimitedDFS(Node currentNode, 
                                      int depth,
                                      Stopwatch timer,
                                      ref int nodesVisited)
        {
            //we visit the same node multiple times, 
            //so count each board each time we visit it
            Board currentBoard = currentNode.board;
            nodesVisited += 1;

            if (currentBoard.isInEndState())
            {
                timer.Stop();
                long milliseconds = timer.ElapsedMilliseconds;
                //found endState
                //use this opportunity to write path to console,
                //as it will be rewinded by recursive call.
                handleEndState(currentNode, nodesVisited, milliseconds);
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
           
                if (doDepthLimitedDFS(childNode, 
                                      depth - 1, 
                                      timer,
                                      ref nodesVisited) != null)
                {
                    return childNode;
                }
                

            }

            return null;
        }

        static void doPriorityQueueSkeletonCode(Node rootNode,
                                                Heuristic selectedHeuristic,
                                                IComparer<Node> selectedComparer,
                                                Stopwatch timer)
        {

            C5.IntervalHeap<Node> q = new C5.IntervalHeap<Node>(selectedComparer);
            q.Add(rootNode);

            //repeated state checking tools
            HashSet<int> previouslyExpanded = new HashSet<int> { };
            Dictionary<int, Node> idsToNode = new Dictionary<int, Node> { };

            int iterations = 1;
            while (q.Count > 0)
            {
                //heap extracts minimum based on selected comparer
                Node currentNode = q.DeleteMin();
                Board currentBoard = currentNode.board;

                //keeps track of expanded states
                //we are currently expanding this state.
                previouslyExpanded.Add(currentBoard.Id);

                if (currentBoard.isInEndState())
                {
                    //stop timer now, algorithm is done
                    //(otherwise we time writing to the console, which can be long)
                    timer.Stop();
                    long milliseconds = timer.ElapsedMilliseconds;

                    handleEndState(currentNode, iterations, milliseconds);
                    return;
                }

                List<Board> children = currentBoard.GenerateNextStates();

                foreach (Board child in children)
                {

                    if (!previouslyExpanded.Contains(child.Id))
                    {

                        int childPotentialCost = selectedHeuristic.getHeuristicScore(currentNode,
                                                                                     child);
                        Node childNode = new Node(child,
                                                  currentNode,
                                                  currentNode.cost + child.TileMoved,
                                                  currentNode.depth + 1,
                                                  childPotentialCost);

                        C5.IPriorityQueueHandle<Node> h = null;

                        if (idsToNode.ContainsKey(child.Id))
                        {
                            //already seen this node somewhere. check queue for cheaper, otherwise don't add
                            Node currentNodeInQueue = idsToNode[child.Id];
                            int currentCostInQueue = currentNodeInQueue.heuristicCost;
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

        static void doUniformCostShort(Node rootNode)
        {
            Console.WriteLine("Beginning uniform cost search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Heuristic costHeuristic = new CostHeuristic();
            IComparer<Node> costComparer = new CostHeuristic.CostComparer();

            doPriorityQueueSkeletonCode(rootNode, costHeuristic, costComparer, timer);
        }

        static void doBestFirstSearchShort(Node rootNode)
        {
            Console.WriteLine("Beginning best-first search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Heuristic outOfPlaceHeuristic = new OutOfPlaceHeuristic();
            IComparer<Node> outOfPlaceComparer = new OutOfPlaceHeuristic.OutOfPlaceComparer();

            doPriorityQueueSkeletonCode(rootNode, outOfPlaceHeuristic, outOfPlaceComparer, timer);
        }

        static void doAStarOneShort(Node rootNode)
        {
            Console.WriteLine("Beginning A*-One search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Heuristic aStarOneHeuristic = new AStarOneHeuristic();
            IComparer<Node> aStarOneComparer = new AStarOneHeuristic.AStarOneComparer();

            doPriorityQueueSkeletonCode(rootNode, aStarOneHeuristic, aStarOneComparer, timer);
        }

        static void doAStarTwoShort(Node rootNode)
        {
            Console.WriteLine("Beginning A*-Two search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Heuristic aStarTwoHeuristic = new AStarTwoHeuristic();
            IComparer<Node> aStarTwoComparer = new AStarTwoHeuristic.AStarTwoComparer();

            doPriorityQueueSkeletonCode(rootNode, aStarTwoHeuristic, aStarTwoComparer, timer);
        }

        static void doAStar3Short(Node rootNode)
        {
            Console.WriteLine("Beginning A*-3 search...");
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Heuristic aStarThreeHeuristic = new AStarThreeHeuristic();
            IComparer<Node> aStarThreeComparer = new AStarThreeHeuristic.AStarThreeComparer();

            doPriorityQueueSkeletonCode(rootNode, aStarThreeHeuristic, aStarThreeComparer, timer);
        }

        #endregion

        #region Writing to Console Functions

        static void handleEndState(Node endBoardNode,
                                   int iterations,
                                   long milliseconds)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Development\BFS.txt");

            int depthOfEndNode = endBoardNode.depth;
            int totalCost = endBoardNode.cost;

            //push parent onto stack and then pop to get correct order
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
                writeBoardInfo(currentNodeInPath, sw);                
            }

            sw.WriteLine("Length: " + depthOfEndNode);
            sw.WriteLine("Total cost: " + totalCost);
            sw.WriteLine("Elapsed milliseconds: " + milliseconds);
            sw.WriteLine("Total nodes considered: " + iterations);
            sw.Close();
            
        }

        static void writeBoardInfo(Node currentNode,
                                   System.IO.StreamWriter sw = null)
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

                sw.WriteLine(sb.ToString());
            }
            sw.WriteLine(currentBoard.ToString());
        }

        static String getArgs()
        {
            String defaultArgs = "";
            Console.WriteLine("Enter EASY, MEDIUM, or HARD for built-in args");
            Console.WriteLine("Or enter initial board configuration manually like so: '134862705'");
            string enteredArgs = Console.ReadLine();
            defaultArgs = enteredArgs;

            switch (defaultArgs)
            {
                case "EASY":
                    defaultArgs = EASY;
                    break;
                case "MEDIUM":
                    defaultArgs = MEDIUM;
                    break;
                case "HARD":
                    defaultArgs = HARD;
                    break;
                default:
                    break;
            }

            if (defaultArgs.Length == 0)
            {
                throw new Exception("Invalid arguments provided");
            }

            return defaultArgs;
        }

        static String getSearchRequested()
        {
            Console.WriteLine("Choose one of the following options:");
            Console.WriteLine("BFS: Breadth-First Search");
            Console.WriteLine("DFS: Depth-First Search");
            Console.WriteLine("IDS: Iterative Deepening");
            Console.WriteLine("UCS: Uniform Cost Search");
            Console.WriteLine("GBF: Greedy Best-First Search with No. Tiles Out of Place Heuristic");
            Console.WriteLine("A*1: A* with No. Tiles Out of Place Heuristic");
            Console.WriteLine("A*2: A* with Manhattan Distance Heuristic");
            Console.WriteLine("A*3: A* with Mystery Heuristic");
            String entry = Console.ReadLine();
            return entry;
        }

        #endregion
    }
}
