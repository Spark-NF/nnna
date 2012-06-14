using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NNNA
{
	class PathFinding
	{
		public static List<Vector2> FindPath(Vector2 start, Vector2 destination, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, Vector2 diago_collision)
		{
			if (new Sprite(destination).Collides(new List<MovibleSprite>(), buildings, resources, matrice))
			{ return null; }

			var openList = new NodeList<Node>();
			var closedList = new NodeList<Node>();
			var startNode = new Node(start, null, destination);

			openList.Add(startNode);

			while (openList.Count > 0)
			{
				var current = openList[0];
				openList.RemoveAt(0);
				closedList.Add(current);

				if ((current.Position - destination).LengthSquared() < 200)
				{
					var sol = new List<Vector2>();
                    sol.Add(destination);
					while (current.Parent != null)
					{
						sol.Add(current.Position);
						current = current.Parent;
					}
                    sol.Reverse();
					return sol;
				}

				List<Node> possibleNodes = current.Neightborhood(destination, buildings, resources, matrice, diago_collision);
				var possibleNodesCount = possibleNodes.Count;
				for (var i = 0; i < possibleNodesCount; i++)
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
