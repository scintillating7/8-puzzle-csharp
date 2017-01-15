using System;
using System.Collections.Generic;

namespace _8_Puzzle
{

    internal class OutOfPlaceHeuristic : IComparer<Node>
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

    internal class ManhattanHeuristic : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            int xManhattan = getManhattanDistance(x);
            int yManhattan = getManhattanDistance(y);
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

        public int getManhattanDistance(Node x)
        {
            return x.board.getManhattanDistance();
        }
    }

    internal class AStarOneHeuristic : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            OutOfPlaceHeuristic outOfPlace = new OutOfPlaceHeuristic();
            int oX = outOfPlace.getTilesOutOfPlace(x);
            int oY = outOfPlace.getTilesOutOfPlace(y);

            int costX = x.cost; //tileMoved??
            int costY = y.cost;

            int totX = oX + costX;
            int totY = oY + costY;

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
    internal class AStarTwoHeuristic : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            ManhattanHeuristic manhattan = new ManhattanHeuristic();
            int mX = manhattan.getManhattanDistance(x);
            int mY = manhattan.getManhattanDistance(y);

            int costX = x.cost; //tileMoved??
            int costY = y.cost;

            int totX = mX + costX;
            int totY = mY + costY;

            if (totX > totY)
            {
                return 1;
            } else if (totX < totY)
            {
                return -1;
            } else
            {
                return 0;
            }
        }

    }

    internal class AStarThreeHeuristic : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {

            int x1 = getBiggestHeuristic(x);
            int y1 = getBiggestHeuristic(y);

            int costX = x.cost; //tileMoved??
            int costY = y.cost;

            int totX = x1 + costX;
            int totY = y1 + costY;

            if (totX < totY)
            {
                return -1;
            } else if (totX > totY)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        private int getBiggestHeuristic(Node x)
        {
            return x.board.getBiggestHeuristic();
        }

        //private int getMultiplicativeHeuristic(Node x)
        //{
        //    return x.board.getMultiplicativeHeuristic();
        //}

    }
}