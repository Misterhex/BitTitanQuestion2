using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitTitanQuestion2
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = new Node();

            var level1 = Builder<Node>.CreateListOfSize(3)
                .TheFirst(1)
                .With(i => i.Children, Builder<Node>.CreateListOfSize(2).Build().ToArray())
                .TheLast(1)
                .With(i => i.Children, Builder<Node>.CreateListOfSize(1).Build().ToArray())
                .Build()
                .ToArray();

            root.Children = level1;

            foreach (var group in BreadthFirstTopDownTraversal(root, node => node.Children).GroupBy(i => i.Depth).ToArray())
            {
                var length = group.Count();
                for (int i = 0; i < length; i++)
                {
                    var current = group.ElementAt(i);
                    var right = group.ElementAtOrDefault(i + 1);

                    current.Right = right;
                }
            }

            Console.ReadLine();
        }

        public static IEnumerable<Node> BreadthFirstTopDownTraversal(Node root, Func<Node, IEnumerable<Node>> children)
        {

            var q = new Queue<Node>();
            q.Enqueue(root);

            int currentDepth = 0,
                elementsToDepthIncrease = 1,
                nextElementsToDepthIncrease = 0;

            while (q.Count != 0)
            {
                Node current = q.Dequeue();

                current.Depth = currentDepth;
                yield return current;

                nextElementsToDepthIncrease += current.Children == null ? 0 : current.Children.Count();
                if (--elementsToDepthIncrease == 0)
                {
                    currentDepth++;
                    elementsToDepthIncrease = nextElementsToDepthIncrease;
                    nextElementsToDepthIncrease = 0;
                }
                if (current.Children != null)
                {
                    foreach (var child in current.Children)
                    {
                        q.Enqueue(child);
                    }
                }
            }

        }

    }

    public class Node
    {
        public Node[] Children;
        public Node Right;
        public int Depth;
    }


}
