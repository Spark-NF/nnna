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
            _cost.Add("Pierre", 60);
            _cost.Add("Nourriture", 60);
        }

        public Moulin(int x, int y, ContentManager content, Joueur joueur, GameTime time)
            : base(x, y)
        {
            _joueur = joueur;
			LoadContent(content, "Batiments/siege");
            Life = 200;
            LineSight = 128;

            _cost.Add("Nourriture", 60);
            _cost.Add("Pierre", 60); ;
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