using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Siege : ProductionBuilding
    {
        // Ere 2/3
        public Siege(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Or", 500);
            _cost.Add("Pierre", 500);
        }

        public Siege(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            _joueur = joueur;
			LoadContent(content, "Batiments/siege", 8);
			Texture.Animation = false;
            Life = 200;
            LineSight = 128;

            _cost.Add("Or", 500);
            _cost.Add("Pierre", 500); ;

        }
    }
}