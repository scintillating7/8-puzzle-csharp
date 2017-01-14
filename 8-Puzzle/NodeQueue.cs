using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C5;

namespace _8_Puzzle
{
    public class NodeQueue<Node> : C5.IntervalHeap<Node>
    {
        private readonly C5.IntervalHeap<Node> nodeQueue;
        private readonly C5.IDictionary<int, IPriorityQueueHandle<Node>> nodes;
        
        public NodeQueue()
        {
            this.nodeQueue = new IntervalHeap<Node>();
            this.nodes = new HashDictionary<int, IPriorityQueueHandle<Node>>();
        }

        //public void Add(Node toAdd)
        //{
        //    IPriorityQueueHandle<Node> h = null;
        //    nodeQueue.Add(ref h, toAdd);
        //}

        public void Replace(int id,
                            Node newNode)
        {
            IPriorityQueueHandle<Node> h;
            if (nodes.Remove(id, out h))
            {
                nodeQueue.Replace(h, newNode);

            }
        }

        //internal void Add(ref Handle<Node> handle, _8_Puzzle.Node childNode)
        //{
        //    nodeQueue.Add(ref handle, childNode);
        //}

        new void Add(ref IPriorityQueueHandle<Node> h, Node node)
        {

        }
    }
}
