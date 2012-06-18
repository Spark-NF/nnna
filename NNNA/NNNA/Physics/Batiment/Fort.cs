using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    [Serializable]
    class Fort : Building
    {
        // Ere 3 diminue le temps des technoogies
        public Fort(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Or", 2500);
            Prix.Add("Pierre", 3000);
            Prix.Add("Bois", 2000);
        }

        public Fort(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
            LoadContent(content, "Batiments/fort");
            Life = 1000;
            LineSight = 256;

            Prix.Add("Or", 2500);
            Prix.Add("Pierre", 3000);
            Prix.Add("Bois", 2000);
        }
    }
}
