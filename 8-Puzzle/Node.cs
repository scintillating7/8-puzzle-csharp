using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    public class Node
    {
        public Board b;
        public List<Node> children;
        public int depth;
        public int cost;
        public Node(Board b)
        {
            this.b = b;
            this.children = new List<Node>();
            this.cost = 0;
            this.depth = 0;
        }

        public Node Parent { get; private set; }
    }
}
