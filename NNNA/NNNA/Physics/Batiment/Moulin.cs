using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NNNA
{
	[Serializable]
    class Moulin : ProductionBuilding
    {
        // Ere 2/3
        public Moulin(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Pierre", 60);
            Prix.Add("Nourriture", 60);
        }

        public Moulin(int x, int y, ContentManager content, Joueur joueur, GameTime time)
            : base(x, y)
        {
            Joueur = joueur;
			LoadContent(content, "Batiments/moulin");
            Life = 200;
            LineSight = 128;

            Prix.Add("Nourriture", 60);
            Prix.Add("Pierre", 60);
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