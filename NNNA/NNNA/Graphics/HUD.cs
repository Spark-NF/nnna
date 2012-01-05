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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace NNNA
{
    class HUD
    {
		private Vector2 ressource;
		private int interval_ressource;
        private Rectangle position;
        public Rectangle Position
        {
            get { return position; }
        }

        private Texture2D texture_Background;
        public HUD(int x, int y, Minimap minimap, GraphicsDeviceManager graphics)
        {
            position = new Rectangle(x, y, graphics.PreferredBackBufferWidth - x, graphics.PreferredBackBufferHeight - y);
			ressource = new Vector2((int)(position.X + (position.Width * 288) / 1366), (int)(position.Y + (position.Height * 45) / 164));
			interval_ressource = (int)((position.Width * 44) / graphics.PreferredBackBufferWidth);
        }
        public void LoadContent(ContentManager content, string assetName)
        {
            texture_Background = content.Load<Texture2D>(assetName);
        }
        public void Draw(SpriteBatch spriteBatch, Minimap minimap, Joueur joueur, SpriteFont font)
        {
            spriteBatch.Draw(texture_Background, position, Color.White);
            minimap.Draw(spriteBatch);
			int i = 0;
			foreach (Resource res in joueur.Resources(1))
			{
				spriteBatch.Draw(res.Texture(1), new Vector2(5 + i * 140, 5), Color.White);
				spriteBatch.DrawString(font, res.Count.ToString(), new Vector2(10 + i * 140 + res.Texture(1).Width, 5 + ((res.Texture(1).Height - font.MeasureString(res.Count.ToString()).Y) / 2)), Color.White);
				i++;
			}
			spriteBatch.Draw(joueur.Pop_Texture, new Vector2(5 + joueur.Resources(1).Count * 140, 5), Color.White);
			spriteBatch.DrawString(font, joueur.Population.ToString() + "/" + joueur.Population_Max.ToString(), new Vector2(10 + joueur.Resources(1).Count * 140 + joueur.Pop_Texture.Width, 5 + ((joueur.Pop_Texture.Height - font.MeasureString(joueur.Population.ToString() + "/" + joueur.Population_Max.ToString()).Y) / 2)), Color.White);
        }
    }
}
