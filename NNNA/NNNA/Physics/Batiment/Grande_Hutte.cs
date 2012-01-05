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
	class Grande_Hutte : Batiment
	{
		public Grande_Hutte(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			LoadContent(content, "Batiments/hutte");

			//Initialisation du batiment
			vie = 500;
			joueur.Resource("Bois").Remove(1000);
			joueur.Resource("Nourriture").Remove(500);

			//Si Population Maximale du jeux non atteint
			if (joueur.Population_Max < 200)
			{
				if (joueur.Population_Max > 185)
				{ joueur.Population_Max += 200 - joueur.Population_Max; }
				else
				{ joueur.Population_Max += 15; }
			}
		}
		public Grande_Hutte(int x, int y, ContentManager content, Joueur joueur, string sans_ressources)
			: base(x, y)
		{
			LoadContent(content, "Batiments/hutte");

			//Initialisation du batiment
			vie = 500;
			
			//Si Population Maximale du jeux non atteint
			if (joueur.Population_Max < 200)
			{
				if (joueur.Population_Max > 185)
				{ joueur.Population_Max += 200 - joueur.Population_Max; }
				else
				{ joueur.Population_Max += 15; }
			}
		}
	}
}
