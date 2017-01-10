﻿using System;
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
        private Heuristic h;

        #endregion

        #region Constructors

        private Board(int[,] tiles,
                      int id)
        {
            this.mTiles = tiles;
            this.mId = id;
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

        #endregion

        #region Public Methods

        public Boolean isInEndState()
        {
            if (this.Id == endState) { return true; }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>1-4 possible next states</remarks>
        public List<Board> generateNextStates()
        {
            List<Board> childStates = new List<Board>();

            //find index of 0/blank tile
            Tuple<int,int> indexOfBlankTile = findBlankTile();

            int row = indexOfBlankTile.Item1;
            int column = indexOfBlankTile.Item2;

            //four possible valid moves, check if they're valid and add if so
            if (indexIsValid(row - 1))
            {
                Board upChild = swap(indexOfBlankTile, 
                                     new Tuple<int, int>(row - 1, column));
                childStates.Add(upChild);
            }
            if (indexIsValid(row + 1))
            {
                Board downChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row + 1, column));
                childStates.Add(downChild);
            }
            if (indexIsValid(column - 1))
            {
                Board leftChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row, column - 1));
                childStates.Add(leftChild);
            }
            if (indexIsValid(column + 1))
            {
                Board rightChild = swap(indexOfBlankTile,
                                    new Tuple<int, int>(row, column + 1));
                childStates.Add(rightChild);
            }


            return childStates;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) {
                    sb.Append(this.mTiles[i, j]);
                    sb.Append(" ");
                }
                
            }

            return sb.ToString();
        }

        #endregion

        #region Private Helper Methods

        private Board swap(Tuple<int, int> indexOfBlankTile, Tuple<int, int> indexToSwapWith)
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

        private Tuple<int,int> findBlankTile()
        {
            Tuple<int, int> returnIndex = Tuple.Create(-1, -1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (this.Tiles[i, j] == 0)
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

        #endregion
    }
}
