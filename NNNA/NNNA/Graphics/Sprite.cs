using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NNNA
{
	class Sprite
	{
		public static int CompareByY(Sprite s1, Sprite s2)
		{
			if (s1.Position.Y + s1.Texture.Height > s2.Position.Y + s2.Texture.Height)
			{ return 0; }
			return 1;
		}

		public virtual void RightClick(Vector2 coos, Camera2D camera)
		{ }

		private bool _decouvert;

		protected bool _crossable;
		public bool Crossable
		{
			get { return _crossable; }
			set { _crossable = value; }
		}
		protected Vector2 _position;
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public Vector2 PositionCenter
		{ get { return _position + new Vector2((float)Math.Round((double)_texture.Width / 2), (float)Math.Round((double)_texture.Height / 2)); } }

		private Vector2 _positionMatrice;
		/// <summary>
		/// Retourne la position dans la matrice. /!\ Si = (-1, -1), alors la position n'apas été assignée /!\
		/// </summary>
		public Vector2 PositionMatrice
		{ get { return _positionMatrice; } }

		protected Texture2D _texture, _go, _dots;
		public Texture2D Texture
		{
			get { return _texture; }
			set { _texture = value; }
		}
		private Char _name;
		public Char Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public Sprite(Char name)
		{
			_crossable = true;
			_decouvert = false;
			_name = name;
		}

		public Sprite(Vector2 position)
		{
			_crossable = true;
			_decouvert = false;
			_position = position;
		}

		public Sprite(int x, int y) : this(new Vector2(x, y)) { }
		public Sprite(ContentManager content, string assetName, int x, int y, bool crossable = true, int i = -1, int j = -1)
		{
			_decouvert = false;
			_position = new Vector2(x, y);
			_positionMatrice = new Vector2(i, j);
			_texture = content.Load<Texture2D>(assetName);
			_crossable = crossable;
		}

		public void LoadContent(ContentManager content, string assetName)
		{
			_texture = content.Load<Texture2D>(assetName);
			_texture.Name = assetName;
			_go = content.Load<Texture2D>("go");
		}

		public void Update(Vector2 translation)
		{ _position += translation; }

		public void Draw(SpriteBatch spriteBatch)
		{ spriteBatch.Draw(_texture, _position, Color.White); }
		public void DrawMap(SpriteBatch spriteBatch, Camera2D camera, float mul, int wheather)
		{
			if (wheather == 1)
			{
				if (mul > 0.25f) _decouvert = true;
				mul = (_decouvert && mul < 0.25f) ? 0.25f : mul;
			}
				spriteBatch.Draw(_texture, _position - camera.Position, new Color(mul, mul, mul));
		}

		public bool Collides(List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			var rec = new Rectangle((int)_position.X, (int)_position.Y + ((_texture.Height * 2) / 3), _texture.Width / 4, _texture.Height / 3);
			foreach (MovibleSprite sprite in sprites)
			{
				if (sprite != this)
				{
					var sprec = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y + ((sprite.Texture.Height * 2) / 3), sprite.Texture.Width / 4, sprite.Texture.Height / 3);
					if (sprec.Intersects(rec))
					{ return true; }
				}
			}
			foreach (Building sprite in buildings)
			{
				if (sprite != this)
				{
					var sprec = new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y, sprite.Texture.Width, sprite.Texture.Height);
					if (sprec.Intersects(rec))
					{ return true; }
				}
			}
			Vector2 coos = Game1.Xy2Matrice(new Vector2(_position.X + _texture.Width / 8, _position.Y + (_texture.Height * 4) / 5));
			if (coos.X >= 0 && coos.Y >= 0 && coos.X < matrice.GetLength(1) && coos.Y < matrice.GetLength(0))
			{
				if (!matrice[(int)coos.Y, (int)coos.X].Crossable)
				{ return true; }
			}
			return false;
		}
	}
}
