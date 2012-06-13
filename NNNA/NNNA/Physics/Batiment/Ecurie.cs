using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Ecurie : ProductionBuilding
    {
        // Ere 2
        public Ecurie(int x = 0, int y = 0)
			: base(x, y)
		{
            Prix.Add("Bois", 300);
            Prix.Add("Pierre", 200);
		}

		public Ecurie(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			Joueur = joueur;
			LoadContent(content, "Batiments/ecurie" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
			Life = 100;
            LineSight = 128;
			Prix.Add("Bois", 300);
            Prix.Add("Pierre", 200);

		}
    }
}
