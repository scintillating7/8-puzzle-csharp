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
        private Tree(Node root)
        {
            this.root = root;
        }

        public class Node
        {
            public Board b;
            public List<Node> children;
            public Node(Board b)
            {
                this.b = b;
                this.children = new List<Node>();
            }
        }

        public List<Node> Children
        {
            get
            {
                return this.children;
            }
        }
        
        public static Tree CreateNew(Board b)
        {
            Node n = new Node(b);
            Tree t = new Tree(n);
            return t;
        }

        public void AddChildren(Node toAdd, 
                                List<Board> bList)
        {
            foreach(Board b in bList)
            {
                Node n = new Node(b);
                toAdd.children.Add(n);
            }
        }

    }
}
