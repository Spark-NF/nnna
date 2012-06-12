using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
    class Rock : Building
    {
        public Rock(int x, int y, ContentManager content)
			: base (x,y)
		{
			int ra;
			var rand = new Random();
			ra = rand.Next(2);
			if (ra == 0)			
			LoadContent(content, "Decors/Rock6");
			else if (ra == 1)
                LoadContent(content, "Decors/Rock2");
			else
                LoadContent(content, "Decors/Rock3");
		}
    }
}
