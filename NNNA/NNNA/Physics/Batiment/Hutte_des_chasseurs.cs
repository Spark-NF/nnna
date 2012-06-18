using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// 1ere Ere
	[Serializable]
	class HutteDesChasseurs : ProductionBuilding
	{
		public HutteDesChasseurs(int x = 0, int y = 0)
			: base(x, y)
		{
			Type = "caserne";
			Prix.Add("Bois", 75);
		}

		public HutteDesChasseurs(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			Joueur = joueur;
			Type = "caserne";
			LoadContent(content, "Batiments/caserne_" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 100;
			LineSight = 2 * 64;
			Prix.Add("Bois", 75);
		}
	}
}

