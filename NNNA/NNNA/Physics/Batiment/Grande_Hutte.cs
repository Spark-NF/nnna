using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	[Serializable]
	class GrandeHutte : ProductionBuilding
	{
		public GrandeHutte(int x = 0, int y = 0)
			: base(x, y)
		{
			Type = "forum";
			Prix.Add("Bois", 1000);
			Prix.Add("Nourriture", 500);
		}

		public GrandeHutte(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			Joueur = joueur;
			Type = "forum";
			LoadContent(content, "Batiments/forum_" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 500;
			Prix.Add("Bois", 1000);
			Prix.Add("Nourriture", 500);
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
