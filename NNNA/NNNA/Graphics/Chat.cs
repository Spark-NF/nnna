using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Chat
	{
		static public List<ChatMessage> Messages = new List<ChatMessage>();
		public static int Timeout;

		static public void Add(ChatMessage msg, int timeout = 20000)
		{
            if (msg.Text != "")
            {
                Timeout = timeout;
                Messages.Add(msg);
            }
		}

		static public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D background, int y = 0)
		{
			if (Timeout > 0)
			{
				int maxLength = 0;
				for (int i = Messages.Count - 1; i >= Math.Max(0, Messages.Count - 10); i--)
				{
					ChatMessage msg = Messages[i];
					if ((int)font.MeasureString(msg.Author + " : " + msg.Text).X > maxLength)
					{ maxLength = (int)font.MeasureString(msg.Author + " : " + msg.Text).X; }
				}
				spriteBatch.Draw(background, new Rectangle(1, y, maxLength + 10, 20 * (Messages.Count - Math.Max(0, Messages.Count - 6)) + 10), Color.White);
				int j = 0;
				for (int i = Messages.Count - 1; i >= Math.Max(0, Messages.Count - 6); i--)
				{
					ChatMessage msg = Messages[i];
					int decay = 0;
					if (msg.Author != "")
					{
						spriteBatch.DrawString(font, msg.Author + " : ", new Vector2(6, y + j * 20 + 5), msg.Color);
						decay = (int)font.MeasureString(msg.Author + " : ").X;
					}
					spriteBatch.DrawString(font, msg.Text, new Vector2(decay + 6, y + j * 20 + 5), Color.White);
					j++;
				}
				Timeout--;
			}
		}

        static public void Clear()
        {
            Messages.Clear();
        }
	}
}
