using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    // 1ere Ere
	class Hutte_des_chasseurs : ProductionBuilding
    {
		public Hutte_des_chasseurs(int x = 0, int y = 0)
			: base(x, y)
		{
            type = "caserne";
			m_cost.Add("Bois", 75);
		}

		public Hutte_des_chasseurs(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            type = "caserne";
            LoadContent(content, "Batiments/hutte_des_chasseurs");
			vie = 100;
			m_cost.Add("Bois", 75);
        }
    }
}
