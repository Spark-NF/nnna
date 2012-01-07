using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

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
