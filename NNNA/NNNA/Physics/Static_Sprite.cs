using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class StaticSprite : Sprite
	{
		public Rectangle Rectangle(Camera2D cam)
		{
			return new Rectangle(
				(int)(Position.X - cam.Position.X),
				(int)(Position.Y - cam.Position.Y),
				Texture.Width,
				Texture.Height
			);
		}

		private bool _selected;
		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}

		protected string _type;
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public StaticSprite(int x, int y)
			: base(x, y)
		{ _selected = false; }

		public void Draw(SpriteBatch spriteBatch, Camera2D camera, Color col)
		{ spriteBatch.Draw(Texture, Position - camera.Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Selected ? Color.Peru : col); }
	}
}
