using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	[Serializable]
	public class StaticSprite : Sprite
	{
		private bool _selected;
		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}

	    public string Type { get; protected set; }

	    protected StaticSprite(int x, int y)
			: base(x, y)
		{ _selected = false; }

		public void Draw(SpriteBatch spriteBatch, Camera2D camera, Color col)
		{ Texture.Draw(spriteBatch, Position - camera.Position, Selected ? Color.Peru : col); }
	}
}
