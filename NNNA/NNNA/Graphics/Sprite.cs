using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Sprite
	{
		public static int CompareByY(Sprite s1, Sprite s2)
		{
			if (s1.Position.Y + s1.Texture.Height > s2.Position.Y + s2.Texture.Height)
			{ return 0; }
			else
			{ return 1; }
		}

		public virtual void RightClick(Vector2 coos, Camera2D camera)
		{ }

        private bool decouvert = false;

		protected bool m_crossable = true;
		public bool Crossable
		{
			get { return m_crossable; }
			set { m_crossable = value; }
		}
		protected Vector2 m_position;
		public Vector2 Position
		{
			get { return m_position; }
			set { m_position = value; }
		}

        public Vector2 Position_Center
        { get { return m_position + new Vector2(m_texture.Width/2, m_texture.Height/2); } }

        private Vector2 position_matrice;
        /// <summary>
        /// Retourne la position dans la matrice. /!\ Si = (-1, -1), alors la position n'apas été assignée /!\
        /// </summary>
        public Vector2 Position_matrice
        { get { return position_matrice; } }

		protected Texture2D m_texture, m_go, m_dots;
		public Texture2D Texture
		{
			get { return m_texture; }
			set { m_texture = value; }
		}
		private Char m_name;
		public Char Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public Sprite(Char name)
		{ m_name = name; }
		public Sprite(Vector2 position)
		{ m_position = position; }
		public Sprite(int x, int y) : this(new Vector2(x, y)) { }
		public Sprite(ContentManager content, string assetName, int x, int y, bool crossable = true, int i = -1, int j = -1)
		{
			m_position = new Vector2(x, y);
            position_matrice = new Vector2(i, j);
			m_texture = content.Load<Texture2D>(assetName);
			m_crossable = crossable;
		}

		public void LoadContent(ContentManager content, string assetName)
		{
			m_texture = content.Load<Texture2D>(assetName);
			m_go = content.Load<Texture2D>("go");
		}

		public void Update(Vector2 translation)
		{ m_position += translation; }

		public void Draw(SpriteBatch spriteBatch)
		{ spriteBatch.Draw(m_texture, m_position, Color.White); }
		public void DrawMap(SpriteBatch spriteBatch, Camera2D camera, float mul)
		{
            if (mul > 0.25f) decouvert = true;
            mul = (decouvert && mul < 0.25f) ? 0.25f : mul;
            spriteBatch.Draw(m_texture, m_position - camera.Position, new Color(mul, mul, mul));
        }

		public bool Collides(List<Movible_Sprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			Rectangle rec = new Rectangle((int)m_position.X, (int)m_position.Y + ((m_texture.Height * 2) / 3), m_texture.Width / 4, m_texture.Height / 3);
			foreach (Sprite sprite in sprites)
			{
				if (sprite != this)
				{
					Rectangle sprec = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y + ((sprite.Texture.Height * 2) / 3), sprite.Texture.Width / 4, sprite.Texture.Height / 3);
					if (sprec.Intersects(rec))
					{ return true; }
				}
			}
			foreach (Sprite sprite in buildings)
			{
				if (sprite != this)
				{
					Rectangle sprec = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, sprite.Texture.Width, sprite.Texture.Height);
					if (sprec.Intersects(rec))
					{ return true; }
				}
			}
			Vector2 coos = Game1.xy2matrice(new Vector2(m_position.X + m_texture.Width / 8, m_position.Y + (m_texture.Height * 4) / 5));
			if (coos.X >= 0 && coos.Y >= 0 && coos.X < matrice.GetLength(1) && coos.Y < matrice.GetLength(0))
			{
				if (!matrice[(int)coos.Y, (int)coos.X].Crossable)
				{ return true; }
			}
			return false;
		}
	}
}
