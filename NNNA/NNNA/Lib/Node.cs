using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NNNA
{
    class Node
    {
        private Node parent;
        public Node Parent
        { get { return parent; } set { parent = value; } }

        private Sprite tile;
        public Sprite Tile
        { get { return tile; } }

        private Sprite destination;
        public Sprite Destination
        { get { return destination; } set { destination = value; } }

        private int manhattan_dist;
        public int Manhattan_dist
        { get { return manhattan_dist; } set { manhattan_dist = value; } }

        public Node(Sprite tile, Node parent, Sprite destination)
        {
            this.tile = tile;
            this.parent = parent;
            this.destination = destination;
            manhattan_dist = (int)(Math.Abs(tile.Position_matrice.X - destination.Position_matrice.X) + Math.Abs(tile.Position_matrice.Y - destination.Position_matrice.Y));
        }

        public List<Node> Neightborhood(Sprite[,] map, Sprite destination)
        {
            List<Node> neight = new List<Node>();
            Vector2 m = tile.Position_matrice;

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
