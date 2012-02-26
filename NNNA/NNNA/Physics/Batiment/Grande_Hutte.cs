using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	class Grande_Hutte : ProductionBuilding
	{
		public Grande_Hutte(int x = 0, int y = 0)
			: base(x, y)
		{
            type = "forum";
			m_cost.Add("Bois", 1000);
			m_cost.Add("Nourriture", 500);
		}

		public Grande_Hutte(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
            type = "forum";
			LoadContent(content, "Batiments/forum_" + joueur.Ere.ToString());
			vie = 500;
			m_cost.Add("Bois", 1000);
			m_cost.Add("Nourriture", 500);
            Line_sight = 12 * 64;
			if (joueur.Population_Max < 200)
			{
				if (joueur.Population_Max > 185)
				{ joueur.Population_Max += 200 - joueur.Population_Max; }
				else
				{ joueur.Population_Max += 15; }
			}
		}
	}
}
