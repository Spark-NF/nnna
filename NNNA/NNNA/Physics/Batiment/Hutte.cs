using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NNNA
{
	// 1ere Ere
	class Hutte : Building
	{
		public Hutte(int x = 0, int y = 0)
			: base(x, y)
		{
			m_cost.Add("Bois", 50);
		}

		public Hutte(int x, int y, ContentManager content, Joueur joueur, byte a)
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
