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
	class Hutte_des_chasseurs : ProductionBuilding
    {
		public Hutte_des_chasseurs(int x = 0, int y = 0)
			: base(x, y)
		{
			m_cost.Add("Bois", 75);
		}

		public Hutte_des_chasseurs(int x, int y, ContentManager content, Joueur joueur)
            : base(x, y)
        {
            LoadContent(content, "Batiments/hutte_des_chasseurs");
			vie = 100;
			m_cost.Add("Bois", 75);
        }
    }
}
