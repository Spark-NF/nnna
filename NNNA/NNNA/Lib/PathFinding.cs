using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNNA
{
    class PathFinding
    {
        public static List<Sprite> FindPath(Sprite[,] map, Sprite start, Sprite destination)
        {
            if (!destination.Crossable)
                return null;

            List<Sprite> path = new List<Sprite>();
            NodeList<Node> openList = new NodeList<Node>();
            NodeList<Node> closedList = new NodeList<Node>();
            List<Node> possibleNodes;
            int possibleNodesCount;
            
            Node startNode = new Node(start, null, destination);

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node current = openList[0];
                openList.RemoveAt(0);
                closedList.Add(current);

                if (current.Tile.Position_Center == destination.Position_Center)
                {
                    List<Sprite> sol = new List<Sprite>();
                    while (current.Parent != null)
                    {
                        sol.Add(current.Tile);
                        current = current.Parent;
                    }
                    return sol;
                }

                possibleNodes = current.Neightborhood(map, destination);
                possibleNodesCount = possibleNodes.Count;
                for (int i = 0; i < possibleNodesCount; i++)
                {
                    if (!closedList.Contains(possibleNodes[i]))
                    {
                        if (openList.Contains(possibleNodes[i]))
                        {
                            if (possibleNodes[i].Manhattan_dist < openList[possibleNodes[i]].Manhattan_dist)
                                openList[possibleNodes[i]].Parent = current;
                        }
                        else openList.PivotInsertion(possibleNodes[i]);
                    }
                }
            }
            return null;
        }
    }
}
