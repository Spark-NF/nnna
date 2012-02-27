using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	class Hutte : Building
	{
		public Hutte(int x = 0, int y = 0)
			: base(x, y)
		{
			m_cost.Add("Bois", 50);
		}

		public Hutte(int x, int y, ContentManager content, Joueur joueur, byte a)
			: base(x, y)
		{
            if (a == 0)
			    LoadContent(content, "Batiments/maison1_" + joueur.Ere.ToString());
            else
                LoadContent(content, "Batiments/maison2_" + joueur.Ere.ToString());
			Life = 100;
			m_cost.Add("Bois", 50);
			Line_sight = 2 * 64;
			if (joueur.Population_Max < 200)
			{
				if (joueur.Population_Max > 190)
				{ joueur.Population_Max += 200 - joueur.Population_Max; }
				else
				{joueur.Population_Max += 10; }
			}
		}
	}
}
