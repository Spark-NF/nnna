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
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (this[i].Tile.Position == node.Tile.Position)
                        return this[i];
                }
                return default(T);
            }
        }

        public void PivotInsertion(T node)
        {
        	int left = 0, right = Count - 1;

        	while (left <= right)
            {
            	int center = (left + right) / 2;
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
