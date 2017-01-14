using System;
using System.Collections.Generic;

namespace _8_Puzzle
{
    internal class Heuristic : IComparer<Node>
    {
       
        public int Compare(Node x, Node y)
        {
            int xMisplacedTiles = getTilesOutOfPlace(x);
            int yMisplacedTiles = getTilesOutOfPlace(y);
            if (xMisplacedTiles < yMisplacedTiles)
            {
                return -1;
            } else if (xMisplacedTiles > yMisplacedTiles)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        private int getTilesOutOfPlace(Node n)
        {
            return n.board.getTilesOutOfPlace();
        }
    }
}