using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	class GrandeHutte : ProductionBuilding
	{
		public GrandeHutte(int x = 0, int y = 0)
			: base(x, y)
		{
			_type = "forum";
			_cost.Add("Bois", 1000);
			_cost.Add("Nourriture", 500);
		}

		public GrandeHutte(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_joueur = joueur;
			_type = "forum";
			LoadContent(content, "Batiments/forum_" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Texture.Animation = false;
			Life = 500;
			_cost.Add("Bois", 1000);
			_cost.Add("Nourriture", 500);
			LineSight = 12 * 64;
			if (joueur.PopulationMax < 200)
			{
				if (joueur.PopulationMax > 185)
				{ joueur.PopulationMax += 200 - joueur.PopulationMax; }
				else
				{ joueur.PopulationMax += 15; }
			}
		}
	}
}
