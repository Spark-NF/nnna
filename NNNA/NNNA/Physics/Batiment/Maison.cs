﻿using Microsoft.Xna.Framework.Content;


namespace NNNA
{
    class Maison : Building
    {
        public Maison(int x = 0, int y = 0)
			: base(x, y)
		{
			m_cost.Add("Bois", 50);
		}

		public Maison(int x, int y, ContentManager content, Joueur joueur, byte a)
			: base(x, y)
		{
            if (a == 0)
			    LoadContent(content, "Batiments/hutte1");
            else
                LoadContent(content, "Batiments/hutte2");
			vie = 100;
			m_cost.Add("Bois", 50);

			if (joueur.Population_Max < 200)
			{
				if (joueur.Population_Max > 190)
				{ joueur.Population_Max += 200 - joueur.Population_Max; }
				else
				{joueur.Population_Max += 10; }
			}
		}
    }
}
