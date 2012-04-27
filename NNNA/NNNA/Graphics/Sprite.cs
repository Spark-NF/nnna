using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Sprite
	{
		protected bool _decouvert;
		protected string _assetName;
		protected Texture2D _dots;
		protected Image _go;

		protected Image _texture;
		public Image Texture
		{ get { return _texture; } }

		protected bool _crossable;
		public bool Crossable
		{
			get { return _crossable; }
			set { _crossable = value; }
		}

		protected bool _visible;
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
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
		public Vector2 PositionMatrice
		{ get { return _positionMatrice; } }

		private Char _name;
		public Char Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public bool Liquid;

		public Sprite(Char name)
		{
			_crossable = true;
			_decouvert = false;
			_name = name;
			Liquid = name == 'e' || name == 't';
		}
		public Sprite(Vector2 position)
		{
			_crossable = true;
			_decouvert = false;
			_position = position;
		}
		public Sprite(int x, int y) : this(new Vector2(x, y)) { }
		public Sprite(ContentManager content, string assetName, int x, int y, bool crossable = true, int i = -1, int j = -1, char name = '\0')
		{
			_decouvert = false;
			_position = new Vector2(x, y);
			_positionMatrice = new Vector2(i, j);
			_texture = new Image(content, assetName);
			_assetName = assetName;
			_crossable = crossable;
			_name = name;
			Liquid = name == 'e' || name == 't';
		}

		public void LoadContent(ContentManager content, string assetName, int columns = 1)
		{
			_texture = new Image(content, assetName, columns);
			_assetName = assetName;
			_go = new Image(content, "go", 8, 1, 5);
		}

		public void Update(Vector2 translation)
		{ _position += translation; }

		public void Draw(SpriteBatch spriteBatch)
		{ _texture.Draw(spriteBatch, _position, Color.White); }

		public void DrawMap(SpriteBatch spriteBatch, Camera2D camera, float mul, int weather, Color color)
		{
			if (weather == 1)
			{
				if (mul > 0.25f) _decouvert = true;
				mul = (_decouvert && mul < 0.25f) ? 0.25f : mul;
			}
			if (mul <= 0 || color == Color.Transparent)
			{ _texture.Draw(spriteBatch, _position - camera.Position, new Color(mul, mul, mul)); }
			else
			{ _texture.Draw(spriteBatch, _position - camera.Position, color); }
		}
		public void DrawMap(SpriteBatch spriteBatch, Camera2D camera, float mul, int weather)
		{ DrawMap(spriteBatch, camera, mul, weather, Color.Transparent); }

		/// <summary>
		/// Détermine si le sprite actuel est en collision avec d'autres chose sur la carte.
		/// </summary>
		/// <param name="units">La liste des sprites de la carte.</param>
		/// <param name="buildings">La liste des bâtiments de la carte.</param>
		/// <param name="resources">La liste des ressources minables de la carte.</param>
		/// <param name="matrice">La matrice de la carte.</param>
		/// <returns>Un booléen indiquant si l'on est en collision ou non.</returns>
		public bool Collides(List<MovibleSprite> units, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice)
		{
			// On teste la collision entre notre rectangle et celui de tous les autres sprites
			var rec = new Rectangle((int)_position.X, (int)_position.Y + (_texture.Height - _texture.CollisionHeight), _texture.Width, _texture.CollisionHeight);
			if ((from sprite in units.Cast<Sprite>().ToList().Concat(buildings.Cast<Sprite>().ToList()).Concat(resources.Cast<Sprite>().ToList())
				 where	sprite != this
				 select new Rectangle((int)sprite.Position.X, (int)sprite.Position.Y + (sprite.Texture.Height - sprite.Texture.CollisionHeight), sprite.Texture.Width, sprite.Texture.CollisionHeight))
				 .Any(sprec => sprec.Intersects(rec)))
			{ return true; }

			// On teste la collision avec la carte
			var coos = Game1.Xy2Matrice(new Vector2(_position.X + _texture.Width, _position.Y + _texture.Height * 4 / 5));
			if (coos.X >= 0 && coos.Y >= 0 && coos.X < matrice.GetLength(1) && coos.Y < matrice.GetLength(0))
			{
				if (!matrice[(int)coos.Y, (int)coos.X].Crossable)
				{ return true; }
			}

			// Si aucune collision n'a été détéctée jusqu'ici, alors c'est que l'on est pas en collision
			return false;
		}

		// Comparateur de Sprite selon leur coordonnées en Y
		public static int CompareByY(Sprite s1, Sprite s2)
		{ return s1.Position.Y + (s1.Texture == null ? 0 : s1.Texture.Height) > s2.Position.Y + (s2.Texture == null ? 0 : s2.Texture.Height) ? 0 : 1; }

		public Rectangle Rectangle(Camera2D cam)
		{
			return new Rectangle(
				(int)(Position.X - cam.Position.X),
				(int)(Position.Y - cam.Position.Y),
				_texture.Width,
				_texture.Height
			);
		}
	}
}
