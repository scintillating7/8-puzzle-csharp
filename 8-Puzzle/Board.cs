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
        static Board endState = new Board("1 2 3 8 0 4 7 6 5");

        //for a particular board
        private int[,] mTiles;
        private Heuristic h;

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

        #endregion

        #region Constructors

        private Board(int[,] tiles) {
            Board b = new Board();
            b.mTiles = tiles;
        }

        private Board(string tileString)
        {
            string[] tileStringArray = tileString.Split();

            int[,] tiles = new int[n, n];
            int k = 0;
            while (k < tileStringArray.Length)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        tiles[i, j] = Convert.ToInt32(tileStringArray[k]);
                        k += 1;
                    }
                }
            }

            Board b = new Board(tiles);
        }

        private Board() { }

        #endregion

        #region Factory Methods

        public static Board CreateNew(string args)
        {
            return new Board(args);
        }

        //public static Board CreateInitial(string[] args)
        //{
        //    Board returnBoard = new Board();

        //    int[,] myTiles = new int[n,n];
        //    for (int i = 0; i < args.Length; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            for (int k = 0; k < n; k++)
        //            {
        //                int parsed = 0;
        //                int.TryParse(args[i], out parsed);
        //                myTiles[j, k] = parsed;
        //                i += 1;
        //            }
                    
        //        }
                
        //        if (i != args.Length)
        //        {
        //            Console.WriteLine("Alert! More arguments provided than expected");
        //        }
        //    }

        //    returnBoard.Tiles = myTiles;

        //    return returnBoard;
        //}

        #endregion

        #region Public Methods

        public Boolean isInEndState()
        {
            
        }

        public static List<Board> generateNextStates()
        {
            List<Board> childStates = new List<Board>();



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
    }
}
