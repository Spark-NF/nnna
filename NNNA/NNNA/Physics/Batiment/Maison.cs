using Microsoft.Xna.Framework.Content;


namespace NNNA
{
    class Maison : Building
    {
        public Maison(int x = 0, int y = 0)
			: base(x, y)
		{
			_cost.Add("Bois", 50);
		}

		public Maison(int x, int y, ContentManager content, Joueur joueur, byte a)
			: base(x, y)
		{
			LoadContent(content, "Batiments/hutte" + (a == 0 ? 1 : 2));
			Life = 100;
			_cost.Add("Bois", 50);
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
