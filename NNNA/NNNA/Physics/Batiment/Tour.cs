using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class Tour : Building
	{
		 // Ere 2
		public Tour(int x = 0, int y = 0)
			: base(x, y)
		{
			_cost.Add("Bois", 50);
			_cost.Add("Pierre", 200);
		}

		public Tour(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_joueur = joueur;
			LoadContent(content, "Batiments/tour" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 200;
			LineSight = 512;
			_cost.Add("Bois", 50);
			_cost.Add("Pierre", 200);

		}
	}
}
