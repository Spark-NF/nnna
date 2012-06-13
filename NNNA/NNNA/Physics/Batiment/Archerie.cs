using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Archerie : ProductionBuilding
    {
        // Ere 2
        public Archerie(int x = 0, int y = 0)
			: base(x, y)
		{
            Prix.Add("Bois", 150);
            Prix.Add("Pierre", 100);
		}

		public Archerie(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			Joueur = joueur;
			LoadContent(content, "Batiments/archerie" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 100;
            LineSight = 128;
			Prix.Add("Bois", 150);
            Prix.Add("Pierre", 100);

		}
    }
}
