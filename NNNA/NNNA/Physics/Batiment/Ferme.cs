using Microsoft.Xna.Framework.Content;


namespace NNNA
{   // Ere 2
    class Ferme : Building
    {
        public Ferme(int x = 0, int y = 0)
			: base(x, y)
		{
            m_cost.Add("Bois", 60);
            m_cost.Add("Bois", 30);
		}

		public Ferme(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			LoadContent(content, "Batiments/ferme");
			vie = 100;
			m_cost.Add("Bois", 60);
            m_cost.Add("Bois", 30);

		}
    }
}
