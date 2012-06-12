﻿using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	// Ere 2
	[Serializable]
	class Ferme : Building
	{
		public Ferme(int x = 0, int y = 0)
			: base(x, y)
		{
            _cost.Add("Bois", 100);
		}

		public Ferme(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_joueur = joueur;
			LoadContent(content, "Batiments/ferme" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 100;
			LineSight = 4 * 64;
			_cost.Add("Bois", 100);

		}
    }
}
