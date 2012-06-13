using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    [Serializable]
    class Universite : Building
    {
        // Ere 3 diminue le temps des technoogies
        public Universite(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Or", 1500);
            Prix.Add("Pierre", 1500);
        }

        public Universite(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
            LoadContent(content, "Batiments/universite");
            Life = 200;
            LineSight = 128;

            Prix.Add("Or", 1500);
            Prix.Add("Pierre", 1500);
        }
    }
}
