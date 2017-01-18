using System;
using System.Collections.Generic;

namespace _8_Puzzle
{
    interface Heuristic
    {
        int getHeuristicScore(Node parent, Board child);
    }

    #region Implementations
    //IMPLEMENTATIONS

    //g(n)
    internal class CostHeuristic : Heuristic
    {

        internal class CostComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if (x.cost < y.cost)
                {
                    return -1;
                }
                else if (x.cost > y.cost)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int getHeuristicScore(Node parent,
                                     Board child)
        {
            return parent.cost + child.TileMoved; //total g so far
        }
    }

    //h(n)
    internal class OutOfPlaceHeuristic : Heuristic
    {

        internal class OutOfPlaceComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                int xMisplacedTiles = getTilesOutOfPlace(x);
                int yMisplacedTiles = getTilesOutOfPlace(y);
                if (xMisplacedTiles < yMisplacedTiles)
                {
                    return -1;
                }
                else if (xMisplacedTiles > yMisplacedTiles)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            public int getTilesOutOfPlace(Node n)
            {
                return n.board.getTilesOutOfPlace();
            }
        }

        public int getHeuristicScore(Node parent,
                                     Board child)
        {
            return child.getTilesOutOfPlace();
        }

    }

    internal class ManhattanHeuristic : Heuristic
    {
        internal class ManhattanComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                int xManhattan = x.board.getManhattanDistance();
                int yManhattan = y.board.getManhattanDistance();
                if (xManhattan < yManhattan)
                {
                    return -1;
                }
                else if (xManhattan > yManhattan)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

        }

        public int getHeuristicScore(Node parent, Board child)
        {
            return child.getManhattanDistance();
        }

    }        

    //f(n) = g(n) + h(n)
    internal class AStarOneHeuristic : Heuristic
    {
        internal class AStarOneComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                int hX = x.board.getTilesOutOfPlace();
                int hY = y.board.getTilesOutOfPlace();

                int gX = x.cost;
                int gY = y.cost;

                int fX = hX + gX;
                int fY = hY + gY;

                //sanity check
                int heuristicX = x.heuristicCost;
                int heuristicY = y.heuristicCost;

                if ((heuristicX != fX) || (heuristicY != fY)) {
                    throw new Exception("WTF");
                }

                if (fX > fY)
                {
                    return 1;
                }
                else if (fX < fY)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }          
        }

        public int getHeuristicScore(Node parent,
                                     Board child)
        {
            return parent.cost + 
                   child.TileMoved + //together these two = g(n)
                   child.getTilesOutOfPlace(); //h(n)
        }

    }

    internal class AStarTwoHeuristic : Heuristic
    {
        public int getHeuristicScore(Node parent, Board child)
        {
            return parent.cost + 
                   child.TileMoved + //together = g(n)
                   child.getManhattanDistance(); //h(n)
        }

        internal class AStarTwoComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                int mX = x.board.getManhattanDistance();
                int mY = y.board.getManhattanDistance();

                int costX = x.cost; //tileMoved??
                int costY = y.cost;

                int totX = mX + costX;
                int totY = mY + costY;

                int heuristicX = x.heuristicCost;
                int heuristicY = y.heuristicCost;

                if ((heuristicX != totX) || (heuristicY != totY))
                {
                    throw new Exception("WTF too");
                }

                if (totX > totY)
                {
                    return 1;
                }
                else if (totX < totY)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    internal class AStarThreeHeuristic : Heuristic
    {
        public int getHeuristicScore(Node parent, Board child)
        {
            return parent.cost + 
                   child.TileMoved + //together g(n)
                   child.getMultiplicativeHeuristic(); //h(n)
        }

        internal class AStarThreeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {

                int x1 = x.board.getMultiplicativeHeuristic();
                int y1 = y.board.getMultiplicativeHeuristic();

                int costX = x.cost; 
                int costY = y.cost;

                int totX = x1 + costX;
                int totY = y1 + costY;

                if ((totX != x.heuristicCost) || (totY != y.heuristicCost))
                {
                    throw new Exception("WTF three");
                }

                if (totX < totY)
                {
                    return -1;
                }
                else if (totX > totY)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
    #endregion
}

