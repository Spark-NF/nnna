﻿using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class Palmier : Building
	{
		public Palmier(int x, int y, ContentManager content)
			: base (x,y)
		{
			var rand = new Random();
			int ra = rand.Next(3);
			LoadContent(content, "Decors/palmier"+(ra+1));
		}
	}
}
