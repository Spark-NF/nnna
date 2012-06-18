using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    // 1ere Ere
    [Serializable]
    class Hopital : ProductionBuilding
    {
        public Hopital(int x = 0, int y = 0)
            : base(x, y)
        {
            Type = "hopital";
            Prix.Add("Bois", 150);
            Prix.Add("Pierre", 200);
        }

        public Hopital(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            Joueur = joueur;
            Type = "hopital";
            LoadContent(content, "Batiments/hopital" + joueur.Ere.ToString(CultureInfo.CurrentCulture));
            Life = 125;
            LineSight = 2 * 64;
            Prix.Add("Bois", 150);
            Prix.Add("Pierre", 200);
        }
    }
}
