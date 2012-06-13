using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Forge : ProductionBuilding
    {
		// Ere 2/3
        public Forge(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Fer", 200);
            Prix.Add("Pierre", 100);
            Prix.Add("Or", 200);
        }

        public Forge(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
			LoadContent(content, "Batiments/forge");
            Life = 200;
            LineSight = 128;
            Prix.Add("Fer", 200);
            Prix.Add("Pierre", 100);
            Prix.Add("Or", 200);

        }
    }
}