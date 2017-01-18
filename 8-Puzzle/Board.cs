using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    public class Board
    {
        #region Declarations

        //true for all boards
        static int n = 3;
        static int endState = 123804765;

        //for a particular board
        private int[,] mTiles;
        private int mId;
        private int mTileMoved;
        private int mDirection;

        #endregion

        #region Constructors

        private Board(int[,] tiles,
                      int id)
        {
            this.mTiles = tiles;
            this.mId = id;
        }

        private Board(int[,] tiles,
                      int id,
                      int tileMoved,
                      int direction)
        {
            this.mTiles = tiles;
            this.mId = id;
            this.mTileMoved = tileMoved;
            this.mDirection = direction;
        }

        private Board() { }

        #endregion

        #region Factory Methods

        public static Board CreateNew(string args)
        {
            char[] tileStringArray = args.ToCharArray();
            int id = Convert.ToInt32(args);

            int[,] tiles = new int[n, n];
            int k = 0;
            while (k < tileStringArray.Length)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        tiles[i, j] =(int) Char.GetNumericValue(tileStringArray[k]);
                        k += 1;
                    }
                }
            }
            return new Board(tiles, 
                             id);
        }

        #endregion

        #region Public Properties

        public int[,] Tiles
        {
            get
            {
                return mTiles;
            }
            set
            {
                mTiles = value;
            }
        }

        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }

        public int Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }

        public int TileMoved
        {
            get { return mTileMoved; }
            set { mTileMoved = value; }
        }

        #endregion

        #region Public Methods

        public Boolean isInEndState()
        {
            if (this.Id == endState) { return true; }

            return false;
        }

        public int getTilesOutOfPlace()
        {
            int misPlaced = 0;
            int k = 0;

            int[] temp = toIntArray(endState);
            
            while (k < (n* n))
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (this.Tiles[i, j] != temp[k])
                        {
                            misPlaced += 1;
                        }
                        k++;
                    }
                }

            }

            return misPlaced;
        }

        public int getManhattanDistance()
        {
            int manhattanDistance = 0;

            Board endBoard = Board.CreateNew("123804765");      
            int[,] endTiles = endBoard.Tiles;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int currentNumber = this.Tiles[i, j];
                    Tuple<int, int> indexInEnd = endBoard.findIndexOfTile(currentNumber);
                    manhattanDistance += Math.Abs(i - indexInEnd.Item1) + 
                                         Math.Abs(j - indexInEnd.Item2);
                }
            }

            return manhattanDistance;
        }

        public int getMultiplicativeHeuristic()
        {
            int updatedTotalManhattanDistance = 0;

            Board endBoard = Board.CreateNew("123804765");
            int[,] endTiles = endBoard.Tiles;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int currentNumber = this.Tiles[i, j];
                    Tuple<int, int> indexInEnd = endBoard.findIndexOfTile(currentNumber);
                    int toAdd = currentNumber *
                                (Math.Abs(i - indexInEnd.Item1) + Math.Abs(j - indexInEnd.Item2));

                    updatedTotalManhattanDistance += toAdd;
                }
            }

            return updatedTotalManhattanDistance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>1-4 possible next states</remarks>
        public List<Board> GenerateNextStates()
        {
            List<Board> childStates = new List<Board>();

            //find index of 0/blank tile
            Tuple<int,int> indexOfBlankTile = findIndexOfTile(0);

            int row = indexOfBlankTile.Item1;
            int column = indexOfBlankTile.Item2;

            //four possible valid moves, check if they're valid and add if so
            if (indexIsValid(row - 1))
            {
                Board upChild = swap(indexOfBlankTile, 
                                     new Tuple<int, int>(row - 1, column));
                upChild.Direction = 1;
                upChild.TileMoved = this.mTiles[row - 1, column];
                childStates.Add(upChild);
            }
            if (indexIsValid(row + 1))
            {
                Board downChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row + 1, column));
                downChild.Direction = 0;
                downChild.TileMoved = this.mTiles[row + 1, column];
                childStates.Add(downChild);
            }
            if (indexIsValid(column - 1))
            {
                Board leftChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row, column - 1));
                leftChild.Direction = 2;
                leftChild.TileMoved = this.mTiles[row, column - 1];
                childStates.Add(leftChild);
            }
            if (indexIsValid(column + 1))
            {
                Board rightChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row, column + 1));
                rightChild.Direction = 3;
                rightChild.TileMoved = this.mTiles[row, column + 1];
                childStates.Add(rightChild);
            }


            return childStates;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("\n");

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) {
                    sb.Append(this.mTiles[i, j]);
                    sb.Append(" ");
                }
                sb.Append("\n");
                
            }

            return sb.ToString();
        }

        #endregion

        #region Private Helper Methods

        private Board swap(Tuple<int, int> indexOfBlankTile, 
                           Tuple<int, int> indexToSwapWith)
        {
            Board child = this.Clone();
            int toSwap = this.mTiles[indexToSwapWith.Item1, indexToSwapWith.Item2];
            child.Tiles[indexOfBlankTile.Item1, indexOfBlankTile.Item2] = toSwap;
            child.Tiles[indexToSwapWith.Item1, indexToSwapWith.Item2] = 0; //blank tile

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    sb.Append(child.Tiles[i, j]);
                }
            }

            child.Id = Convert.ToInt32(sb.ToString());

            return child;
        }

        private Board Clone()
        {
            Board b = new Board();
            int[,] tiles = new int[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tiles[i, j] = this.Tiles[i, j];
                }
            }
            b.Tiles = tiles;
            b.Id = this.Id;
            return b;
        }

        private Tuple<int,int> findIndexOfTile(int tileToFind)
        {
            Tuple<int, int> returnIndex = Tuple.Create(-1, -1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (this.Tiles[i, j] == tileToFind)
                    {
                        returnIndex = Tuple.Create(i, j);
                    }
                }
            }

            return returnIndex;
        }

        private Boolean indexIsValid(int index)
        {
            Boolean isValidInd = false;

            if ((0 <= index) && (index <= n - 1))
            {
                isValidInd = true;
            }

            return isValidInd;
        }

        private static int[] toIntArray(int x)
        {
            if (x == 0) { return new int[1] { 0 }; }

            List<int> digits = new List<int>();
            for(; x != 0; x /=10)
            {
                digits.Add(x % 10);
            }

            int[] intArr = digits.ToArray();
            Array.Reverse(intArr);

            return intArr;
        }

        #endregion
    }
}
