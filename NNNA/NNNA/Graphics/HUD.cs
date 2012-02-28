using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    class HUD
	{
		private Texture2D _textureBackground;

    	private Rectangle _position;
        public Rectangle Position
		{ get { return _position; } }

		private bool _isSmart;
		public bool IsSmart
		{
			get { return _isSmart; }
			set { _isSmart = value; }
		}
		public int SmartPos, Height;

        public HUD(int x, int y, bool isSmart, GraphicsDeviceManager graphics)
        {
			_position = new Rectangle(x, y, graphics.PreferredBackBufferWidth - x, graphics.PreferredBackBufferHeight - y);
			_isSmart = isSmart;
			SmartPos = 0;
			Height = graphics.PreferredBackBufferHeight;
        }

        public void LoadContent(ContentManager content, string assetName)
        { _textureBackground = content.Load<Texture2D>(assetName); }

        public void Draw(SpriteBatch spriteBatch, Minimap minimap, Joueur joueur, SpriteFont font)
        {
			// HUD
			if (Souris.Get().Position.Y > Height - 10)
			{ SmartPos = 0; }
			int posY = _isSmart ? _position.Y + SmartPos : _position.Y;
			if (posY < Height)
			{
				spriteBatch.Draw(_textureBackground, new Rectangle(_position.X, posY, _position.Width, _position.Height), Color.White);
				minimap.Draw(SmartPos, spriteBatch);
			}
			if (Souris.Get().Position.Y < _position.Y && _position.Y + SmartPos < Height && _isSmart)
			{ SmartPos += 5; }

			// Ressources
			int i = 0;
			foreach (Resource res in joueur.Resources())
			{
				spriteBatch.Draw(res.Icon(1), new Vector2(5 + i * 140, 5), Color.White);
				spriteBatch.DrawString(font, res.Count.ToString(CultureInfo.CurrentCulture), new Vector2(10 + i * 140 + res.Icon(1).Width, 5 + ((res.Icon(1).Height - font.MeasureString(res.Count.ToString(CultureInfo.CurrentCulture)).Y) / 2)), Color.White);
				i++;
			}
			spriteBatch.Draw(joueur.PopTexture, new Vector2(5 + joueur.Resources().Count * 140, 5), Color.White);
			spriteBatch.DrawString(font, joueur.Population.ToString(CultureInfo.CurrentCulture) + "/" + joueur.PopulationMax.ToString(CultureInfo.CurrentCulture), new Vector2(10 + joueur.Resources().Count * 140 + joueur.PopTexture.Width, 5 + ((joueur.PopTexture.Height - font.MeasureString(joueur.Population.ToString(CultureInfo.CurrentCulture) + "/" + joueur.PopulationMax.ToString(CultureInfo.CurrentCulture)).Y) / 2)), Color.White);
        }
    }
}
