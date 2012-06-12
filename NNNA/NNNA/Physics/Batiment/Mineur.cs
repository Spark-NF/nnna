using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Mineur : ProductionBuilding
    {
        // Ere 2/3
        public Mineur(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Pierre", 100);
        }

        public Mineur(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            _joueur = joueur;
			LoadContent(content, "Batiments/mineur");
            Life = 200;
            LineSight = 128;
            _cost.Add("Pierre", 100);

        }
    }
}