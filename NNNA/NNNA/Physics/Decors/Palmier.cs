using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Palmier : Building
	{
		public Palmier(int x, int y, ContentManager content)
			: base (x,y)
		{
			int ra = 0;
			Random rand = new Random();
			ra = rand.Next(2);
			if (ra == 0)			
			LoadContent(content, "Decors/palmier1");
			else if (ra == 1)
			LoadContent(content, "Decors/palmier2");
			else
			LoadContent(content, "Decors/palmier3");
		}
	}
}
