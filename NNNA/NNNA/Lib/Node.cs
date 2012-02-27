using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NNNA
{
    class Node
    {
        private Node _parent;
    	public Node Parent
    	{
    		get { return _parent; }
			set { _parent = value; }
    	}

        private Sprite _tile;
        public Sprite Tile
        { get { return _tile; } }

        private Sprite _destination;
    	public Sprite Destination
    	{
    		get { return _destination; }
			set { _destination = value; }
    	}

        private int _manhattanDist;
    	public int ManhattanDist
    	{
    		get { return _manhattanDist; }
			set { _manhattanDist = value; }
    	}

        public Node(Sprite tile, Node parent, Sprite destination)
        {
            _tile = tile;
            _parent = parent;
            _destination = destination;
            _manhattanDist = (int)(Math.Abs(tile.PositionMatrice.X - destination.PositionMatrice.X) + Math.Abs(tile.PositionMatrice.Y - destination.PositionMatrice.Y));
        }

        public List<Node> Neightborhood(Sprite[,] map, Sprite destination)
        {
            var neight = new List<Node>();
            Vector2 m = _tile.PositionMatrice;

            //Up
            if (m.Y > 0 && map[(int)m.X, (int)m.Y - 1].Crossable)
                neight.Add(new Node(map[(int) m.X, (int) m.Y - 1], this, destination));

            //Down
            if (m.Y < map.GetLength(1) - 1 && map[(int)m.X, (int)m.Y + 1].Crossable)
                neight.Add(new Node(map[(int)m.X, (int)m.Y + 1], this, destination));

            //Right
            if (m.X < map.GetLength(0) - 1 && map[(int)m.X + 1, (int)m.Y].Crossable)
                neight.Add(new Node(map[(int)m.X + 1, (int)m.Y], this, destination));

            //Left
            if (m.X > 0 && map[(int)m.X - 1, (int)m.Y].Crossable)
                neight.Add(new Node(map[(int)m.X - 1, (int)m.Y], this, destination));

            ////Up Right
            //if (m.X < map.GetLength(0) - 1 && m.Y > 0 && map[(int)m.X + 1, (int)m.Y - 1].Crossable)
            //    neight.Add(new Node(map[(int)m.X + 1, (int)m.Y - 1], this, destination));

            ////Up Left
            //if (m.X > 0 && m.Y > 0 && map[(int)m.X - 1, (int)m.Y - 1].Crossable)
            //    neight.Add(new Node(map[(int)m.X - 1, (int)m.Y - 1], this, destination));

            ////Down Right
            //if (m.X < map.GetLength(0) - 1 && m.Y < map.GetLength(1) - 1 && map[(int)m.X + 1, (int)m.Y + 1].Crossable)
            //    neight.Add(new Node(map[(int)m.X + 1, (int)m.Y + 1], this, destination));

            ////Down Left
            //if (m.X > 0 && m.Y < map.GetLength(1) - 1 && map[(int)m.X - 1, (int)m.Y + 1].Crossable)
            //    neight.Add(new Node(map[(int)m.X - 1, (int)m.Y + 1], this, destination));

            return neight;
        }
    }
}
