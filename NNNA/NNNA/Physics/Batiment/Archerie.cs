﻿using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Archerie : ProductionBuilding
    {
        // Ere 2
        public Archerie(int x = 0, int y = 0)
			: base(x, y)
		{
            m_cost.Add("Bois", 150);
            m_cost.Add("Pierre", 100);
		}

		public Archerie(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			LoadContent(content, "Batiments/archerie1");
			Life = 100;
            Line_sight = 128;
			m_cost.Add("Bois", 150);
            m_cost.Add("Pierre", 100);

		}
    }
}
