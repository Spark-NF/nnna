﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Ecurie : ProductionBuilding
    {
        // Ere 2
        public Ecurie(int x = 0, int y = 0)
			: base(x, y)
		{
            m_cost.Add("Bois", 300);
            m_cost.Add("Pierre", 200);
		}

		public Ecurie(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			LoadContent(content, "Batiments/ecurie");
			vie = 100;
			m_cost.Add("Bois", 300);
            m_cost.Add("Pierre", 200);

		}
    }
}
