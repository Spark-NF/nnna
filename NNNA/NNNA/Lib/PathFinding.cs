using System.Collections.Generic;

namespace NNNA
{
	class PathFinding
	{
		public static List<Sprite> FindPath(Sprite[,] map, Sprite start, Sprite destination)
		{
			if (!destination.Crossable)
			{ return null; }

			var openList = new NodeList<Node>();
			var closedList = new NodeList<Node>();
			var startNode = new Node(start, null, destination);

			openList.Add(startNode);

			while (openList.Count > 0)
			{
				Node current = openList[0];
				openList.RemoveAt(0);
				closedList.Add(current);

				if (current.Tile.PositionCenter == destination.PositionCenter)
				{
					var sol = new List<Sprite>();
					while (current.Parent != null)
					{
						sol.Add(current.Tile);
						current = current.Parent;
					}
					return sol;
				}

				List<Node> possibleNodes = current.Neightborhood(map, destination);
				int possibleNodesCount = possibleNodes.Count;
				for (int i = 0; i < possibleNodesCount; i++)
				{
					if (!closedList.Contains(possibleNodes[i]))
					{
						if (openList.Contains(possibleNodes[i]))
						{
							if (possibleNodes[i].ManhattanDist < openList[possibleNodes[i]].ManhattanDist)
							{ openList[possibleNodes[i]].Parent = current; }
						}
						else
						{ openList.PivotInsertion(possibleNodes[i]); }
					}
				}
			}
			return null;
		}
	}
}
