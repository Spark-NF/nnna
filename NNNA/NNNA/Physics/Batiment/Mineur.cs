using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NNNA
{
	[Serializable]
    class Mineur : ProductionBuilding
    {
        // Ere 2/3
        public Mineur(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Bois", 100);
        }

        public Mineur(int x, int y, ContentManager content, Joueur joueur, GameTime time)
            : base(x, y)
        {
            Joueur = joueur;
			LoadContent(content, "Batiments/mineur");
            Life = 200;
            LineSight = 128;
            Prix.Add("Bois", 100);
        }
        public void Collect(Joueur joueur, int ere, Resource ressource, GameTime time)
        {
            if ((time.TotalGameTime.Seconds % 5) == 0)
            {
                Joueur.Resource(ressource.Name(ere)).Add(5);
            }
        }
    }
}