using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Maison : Building
    {
        public Maison(int x = 0, int y = 0)
			: base(x, y)
		{
			Prix.Add("Bois", 50);
		}

		public Maison(int x, int y, ContentManager content, Joueur joueur, byte a)
			: base(x, y)
		{
			Joueur = joueur;
			LoadContent(content, "Batiments/hutte" + (a == 0 ? 1 : 2));
			Life = 100;
			Prix.Add("Bois", 50);
            LineSight = 128;
			if (joueur.PopulationMax < 200)
			{
				if (joueur.PopulationMax > 190)
				{ joueur.PopulationMax += 200 - joueur.PopulationMax; }
				else
				{joueur.PopulationMax += 10; }
			}
		}
    }
}
