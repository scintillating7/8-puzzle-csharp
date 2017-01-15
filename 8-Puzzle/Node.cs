using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    public class Node : IComparable<Node>
    {
        public Board board;
        public int cost;
        public int depth;
        public int heuristicCost;
        public C5.IPriorityQueueHandle<Node> handle;
        public Boolean expanded;
        public Node(Board b,
                    Node parent)
        {
            this.board = b;
            this.cost = 0;
            this.depth = 0;
            this.Parent = parent;
        }

        public Node(Board b,
                    Node parent,
                    int cost,
                    int depth)
        {
            this.board = b;
            this.cost = cost;
            this.depth = depth;
            this.Parent = parent;

        }

        public Node(Board b,
                    Node parent,
                    int cost,
                    int depth,
                    int heuristicCost)
        {
            this.board = b;
            this.cost = cost;
            this.heuristicCost = heuristicCost;
            this.depth = depth;
            this.Parent = parent;
        }

        public Node Parent { get; private set; }

        public int CompareTo(Node other)
        {
            if (this.cost > other.cost)
            {
                return 1;
            } else if (this.cost < other.cost)
            {
                return -1;
            } else
            {
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Node n = (Node)obj;
            return (board == n.board);
        }

        public override int GetHashCode()
        {
            return board.Id;
        }


    }
}
