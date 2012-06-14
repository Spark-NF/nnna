using System.Collections.Generic;

namespace NNNA
{
    class NodeList<T> : List<T> where T : Node
    {
        public new bool Contains(T node)
        { return this[node] != null; }

        public T this[T node]
        {
            get
            {
                var count = Count;
                for (var i = 0; i < count; i++)
                {
                    if (this[i].Position == node.Position)
                        return this[i];
                }
                return default(T);
            }
        }

        public void PivotInsertion(T node)
        {
            var left = 0;
            var right = Count - 1;

        	while (left <= right)
            {
            	var center = (left + right) / 2;
            	if (node.ManhattanDist < this[center].ManhattanDist)
                { right = center - 1; }
                else if (node.ManhattanDist > this[center].ManhattanDist)
                { left = center + 1; }
                else
                {
                    left = center;
                    break;
                }
            }
        	Insert(left, node);
        }
    }
}
