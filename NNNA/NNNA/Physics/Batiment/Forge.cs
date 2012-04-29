using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Forge : ProductionBuilding
    {
        // Ere 2/3
        public Forge(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Fer", 200);
            _cost.Add("Pierre", 100);
            _cost.Add("Or", 200);
        }

        public Forge(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            _joueur = joueur;
            LoadContent(content, "Batiments/forge");
            Life = 200;
            LineSight = 128;
            _cost.Add("Fer", 200);
            _cost.Add("Pierre", 100);
            _cost.Add("Or", 200);

        }
    }
}