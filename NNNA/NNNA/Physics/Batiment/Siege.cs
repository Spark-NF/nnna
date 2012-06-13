using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Siege : ProductionBuilding
    {
        // Ere 2/3
        public Siege(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Or", 500);
            Prix.Add("Pierre", 500);
        }

        public Siege(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
			LoadContent(content, "Batiments/siege");
            Life = 200;
            LineSight = 128;

            Prix.Add("Or", 500);
            Prix.Add("Pierre", 500);
        }
    }
}