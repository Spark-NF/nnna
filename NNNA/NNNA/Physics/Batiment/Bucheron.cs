using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NNNA
{
    [Serializable]
    class Bucheron : Building
    {
        // Ere 2/3
        public Bucheron(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Bois", 500);
        }

        public Bucheron(int x, int y, ContentManager content, Joueur joueur, GameTime time)
            : base(x, y)
        {
            Joueur = joueur;
            LoadContent(content, "Batiments/bucheron");
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