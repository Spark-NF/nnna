﻿using Microsoft.Xna.Framework.Content;

namespace NNNA
{
    class Archerie : ProductionBuilding
    {
        // Ere 2
        public Archerie(int x = 0, int y = 0)
			: base(x, y)
		{
            _cost.Add("Bois", 150);
            _cost.Add("Pierre", 100);
		}

		public Archerie(int x, int y, ContentManager content)
			: base(x, y)
		{
			LoadContent(content, "Batiments/archerie1");
			Life = 100;
            LineSight = 128;
			_cost.Add("Bois", 150);
            _cost.Add("Pierre", 100);

		}
    }
}
