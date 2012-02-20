using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class MessagesManager
	{
		static public List<Msg> Messages = new List<Msg>();

		static public uint X = 0;

		static public void Draw(SpriteBatch spriteBatch, SpriteFont font)
		{
			for (int i = 0; i < Messages.Count; i++)
			{
				spriteBatch.DrawString(font, Messages[i].Message, new Vector2(X, i * 20 + 5), Messages[i].Couleur);
				Messages[i].Timeout -= 17;
				if (Messages[i].Timeout <= 0)
				{ Messages.RemoveAt(i); }
			}
		}
	}
}
