using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    [Serializable]
    class Port : ProductionBuilding
    {
        // Ere 2/3
        public Port(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Or", 750);
            Prix.Add("Pierre", 750);
            Prix.Add("bois", 500);
        }

        public Port(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
            LoadContent(content, "Batiments/port");
            Life = 200;
            LineSight = 128;

            Prix.Add("Or", 750);
            Prix.Add("Pierre", 750);
            Prix.Add("bois", 500);

        }
    }
}