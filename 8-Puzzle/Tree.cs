using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Puzzle
{
    class Tree
    {

        private Node root;
        private List<Node> children = new List<Node>();
        private Node mParent;
        private Tree(Node root)
        {
            this.root = root;
        }
       
        public static Tree CreateNew(Board b)
        {
            Node n = new Node(b, null);
            Tree t = new Tree(n);
            return t;
        }

        public void AddChildren(Node toAdd, 
                                List<Board> bList)
        {
            foreach(Board b in bList)
            {
                Node n = new Node(b, null);
                //toAdd.children.Add(n);
            }
        }

    }
}
