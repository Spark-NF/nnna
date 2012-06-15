using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NNNA
{
    static class PathFinding
    {
        private static Vector2 FindDestination(Vector2 start, Vector2 destination,  List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, Vector2 diagoCollision)
        {
            var closedList = new NodeList<Node>();
            var startNode = new Node(start, null, destination);
            var openList = new NodeList<Node> { startNode };

            while (openList.Count > 0)
            {
                var current = openList[0];
                openList.RemoveAt(0);
                closedList.Add(current);

                if ((current.Position - destination).LengthSquared() < 200)
                {
                    var sol = new List<Vector2> { destination };
                    while (current.Parent != null && new Sprite(current.Position).Collides(new List<MovibleSprite>(), buildings, resources, matrice))
                    {
                        sol.Add(current.Position);
                        current = current.Parent;
                    }
                    return sol[sol.Count - 1];
                }

                var possibleNodes = current.Neightborhood(destination, buildings, resources, matrice, new Vector2(-1,-1));
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
            return destination;
        }

		public static List<Vector2> FindPath(Vector2 start, Vector2 destination, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, Vector2 diagoCollision)
		{
			if (new Sprite(destination).Collides(new List<MovibleSprite>(), new List<Building>(), new List<ResourceMine>(), matrice))
			{
			    destination = FindDestination(start, destination, buildings, resources, matrice,diagoCollision);
			}
            if (!(new Sprite(destination).NonCollides(new List<MovibleSprite>(), buildings, resources, new Sprite[,] { }, diagoCollision)))
            {
                return null;
            }

		    var closedList = new NodeList<Node>();
			var startNode = new Node(start, null, destination);
            var openList = new NodeList<Node> {startNode};

		    while (openList.Count > 0)
			{
				var current = openList[0];
				openList.RemoveAt(0);
				closedList.Add(current);

				if ((current.Position - destination).LengthSquared() < 200)
				{
					var sol = new List<Vector2> {destination};
				    while (current.Parent != null)
					{
						sol.Add(current.Position);
						current = current.Parent;
					}
                    sol.Reverse();
					return sol;
				}

				var possibleNodes = current.Neightborhood(destination, buildings, resources, matrice, diagoCollision);
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
