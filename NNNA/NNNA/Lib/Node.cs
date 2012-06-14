using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NNNA
{
    class Node
    {
        public Node Parent { get; set; }
        public int ManhattanDist { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Destination { get; private set; }

        public Node(Vector2 current, Node parent, Vector2 destination)
        {
            Destination = destination;
            Position = current;
            Parent = parent;
            ManhattanDist = (int) (destination - current).LengthSquared();
        }

        public List<Node> Neightborhood(Vector2 destination, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, Vector2 diago_collision)
        {
            var neight = new List<Node>();
            var espacement = 10;

            //Up
            var pos = new Vector2(0, espacement);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Down
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Right
            pos = new Vector2(espacement, 0);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Left
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Up Right
            pos = new Vector2(espacement, espacement);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Down Left
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Up Left
            pos = new Vector2(-espacement, espacement);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Down Right
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Up Up Right
            pos = new Vector2((espacement/2), espacement);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Down Down Left
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Up Up Left
            pos = new Vector2(-(espacement / 2), espacement);
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Down Down Right
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Right Right Up
            pos = new Vector2(espacement, (espacement/2));
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Left Left Down
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));

            //Left Left Up
            pos = new Vector2(-espacement, (espacement / 2));
            if (new Sprite(Position + pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position + pos, this, destination));

            //Right Right Down
            if (new Sprite(Position - pos).NonCollides(new List<MovibleSprite>(), buildings, resources, matrice, diago_collision))
                neight.Add(new Node(Position - pos, this, destination));


            return neight;
        }
    }
}
