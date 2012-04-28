using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Usine : ProductionBuilding
    {
        // Ere 4
        public Usine(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Fer", 500);
            _cost.Add("Pierre", 400);
            _cost.Add("Or", 200);
        }

        public Usine(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            _joueur = joueur;
            LoadContent(content, "Batiments/Usine1");
            Life = 200;
            LineSight = 128;
            _cost.Add("Fer", 500);
            _cost.Add("Pierre", 400);
            _cost.Add("Or", 200);

        }
    }
}
