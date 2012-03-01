using Microsoft.Xna.Framework.Content;

namespace NNNA
{   // Ere 2
    class Ferme : Building
    {
        public Ferme(int x = 0, int y = 0)
			: base(x, y)
		{
            _cost.Add("Bois", 60);
            _cost.Add("Bois", 30);
		}

		public Ferme(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			_joueur = joueur;
			LoadContent(content, "Batiments/ferme");
			Life = 100;
			LineSight = 4 * 64;
			_cost.Add("Bois", 60);
            _cost.Add("Bois", 30);

		}
    }
}
