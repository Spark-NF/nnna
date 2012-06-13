using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NNNA
{
    [Serializable]
    class Marche : Building
    {
        // Ere 2/3
        public Marche(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Bois", 500);
            _cost.Add("Pierre", 100); 
        }

        public Marche(int x, int y, ContentManager content, Joueur joueur, GameTime time)
            : base(x, y)
        {
            _joueur = joueur;
            LoadContent(content, "Batiments/marche");
            Life = 200;
            LineSight = 128;
            _cost.Add("Bois", 250);
            _cost.Add("Pierre", 100); 
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