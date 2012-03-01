using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Ecurie : ProductionBuilding
    {
        // Ere 2
        public Ecurie(int x = 0, int y = 0)
			: base(x, y)
		{
            _cost.Add("Bois", 300);
            _cost.Add("Pierre", 200);
		}

		public Ecurie(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_joueur = joueur;
			LoadContent(content, "Batiments/ecurie");
			Life = 100;
            LineSight = 128;
			_cost.Add("Bois", 300);
            _cost.Add("Pierre", 200);

		}
    }
}
