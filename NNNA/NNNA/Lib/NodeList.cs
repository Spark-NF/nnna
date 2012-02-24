using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNNA
{
    class NodeList<T> : List<T> where T : Node
    {
        public new bool Contains(T node)
        {
            return this[node] != null;
        }

        public T this[T node]
        {
            get
            {
                int count = this.Count;
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
            int left = 0, right = this.Count - 1, center = 0;

            while (left <= right)
            {
                center = (left + right) / 2;
                if (node.Manhattan_dist < this[center].Manhattan_dist)
                    right = center - 1;
                else if (node.Manhattan_dist > this[center].Manhattan_dist)
                    left = center + 1;
                else
                {
                    left = center;
                    break;
                }
            }
            this.Insert(left, node);
        }
    }
}
