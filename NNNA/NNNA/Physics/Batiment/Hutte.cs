using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	class Hutte : Building
	{
		public Hutte(int x = 0, int y = 0)
			: base(x, y)
		{
			_cost.Add("Bois", 50);
		}

		public Hutte(int x, int y, ContentManager content, Joueur joueur, byte a)
			: base(x, y)
		{
			LoadContent(content, "Batiments/maison" + (a == 0 ? 1 : 2) + "_" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 100;
			_cost.Add("Bois", 50);
			LineSight = 2 * 64;
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
