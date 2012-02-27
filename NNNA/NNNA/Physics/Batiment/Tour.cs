﻿using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Tour : Building
    {
         // Ere 2
        public Tour(int x = 0, int y = 0)
			: base(x, y)
		{
            m_cost.Add("Bois", 50);
            m_cost.Add("Pierre", 200);
		}

        public Tour(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			LoadContent(content, "Batiments/ecurie");
			Life = 200;
            Line_sight = 512;
			m_cost.Add("Bois", 50);
            m_cost.Add("Pierre", 200);

		}
    }
}
