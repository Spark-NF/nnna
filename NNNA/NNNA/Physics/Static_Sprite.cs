using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
    class Static_Sprite : Sprite
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

		private bool m_selected = false;
		public bool Selected
		{
			get { return m_selected; }
			set { m_selected = value; }
		}

        protected string type;
        public string Type
        { get { return type; } set { type = value; } }

        public Static_Sprite(int x, int y)
            : base(x, y)
		{ }
        
		public void Draw(SpriteBatch spriteBatch, Camera2D camera, Color col)
		{ spriteBatch.Draw(Texture, Position - camera.Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Selected ? Color.Peru : col); }
    }
}
