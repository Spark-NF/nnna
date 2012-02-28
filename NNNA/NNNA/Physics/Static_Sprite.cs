using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class StaticSprite : Sprite
	{
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
		{ _texture.Draw(spriteBatch, Position - camera.Position, Selected ? Color.Peru : col); }
	}
}
