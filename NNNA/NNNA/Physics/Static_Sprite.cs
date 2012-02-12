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
        
		public void Draw(SpriteBatch spriteBatch, Camera2D camera)
		{ spriteBatch.Draw(Texture, Position - camera.Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Selected ? Color.Peru : Color.White); }
    }
}
