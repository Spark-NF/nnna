using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	[Serializable]
	public class Sprite
	{
		protected bool _decouvert;
		protected string _assetName;

		public Image Go;

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

		protected Vector2 _positionMatrice;
		public Vector2 PositionMatrice
		{ get { return _positionMatrice; } }

		private Char _name;
		public Char Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public bool Liquid;

		public Image Dots;

		public Sprite()
		{
			_crossable = true;
			_decouvert = false;
		}
		public Sprite(Char name)
			: this()
		{
			_name = name;
			Liquid = name == 'e' || name == 't';
		}
		public Sprite(Vector2 position)
			: this()
		{ _position = position; }
		public Sprite(int x, int y) : this(new Vector2(x, y)) { }
		public Sprite(ContentManager content, string assetName, int x, int y, bool crossable = true, int i = -1, int j = -1, char name = '\0')
			: this()
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
			_texture.Animation = false;
			_assetName = assetName;
			Go = new Image(content, "go", 8, 1, 5);
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
		public bool Collides(IEnumerable<MovibleSprite> units, IEnumerable<Building> buildings, IEnumerable<ResourceMine> resources, Sprite[,] matrice)
		{
			// On teste la collision entre notre rectangle et celui de tous les autres sprites
			var rec = (Texture != null) ? new Rectangle((int)_position.X + Texture.Collision.X, (int)_position.Y + Texture.Collision.Y, Texture.Collision.Width, Texture.Collision.Height) : new Rectangle((int) _position.X, (int) _position.Y, 1, 1);
			if ((from sprite in units.Cast<Sprite>().ToList().Concat(buildings.Cast<Sprite>().ToList()).Concat(resources.Cast<Sprite>().ToList())
				 where	sprite != this
				 select new Rectangle((int)sprite.Position.X + sprite.Texture.Collision.X, (int)sprite.Position.Y + sprite.Texture.Collision.Y, sprite.Texture.Collision.Width, sprite.Texture.Collision.Height))
				 .Any(sprec => sprec.Intersects(rec)))
			{ return true; }

			// On teste la collision avec la carte
			var coos = (_texture != null) ? Game1.Xy2Matrice(new Vector2(_position.X + _texture.Width, _position.Y + _texture.Height * 4 / 5)) : Game1.Xy2Matrice(_position);
			if (coos.X >= 0 && coos.Y >= 0 && coos.X < matrice.GetLength(1) && coos.Y < matrice.GetLength(0))
			{
				if (!matrice[(int)coos.Y, (int)coos.X].Crossable)
				{ return true; }
			}

			// Si aucune collision n'a été détéctée jusqu'ici, alors c'est que l'on est pas en collision
			return false;
		}

        public bool NonCollides(IEnumerable<MovibleSprite> units, IEnumerable<Building> buildings, IEnumerable<ResourceMine> resources, Sprite[,] matrice, Vector2 diago_collision)
        {
            // On teste la collision entre notre rectangle et celui de tous les autres sprites
            var rec = (Texture != null) ? new Rectangle((int)_position.X + Texture.Collision.X, (int)_position.Y + Texture.Collision.Y, Texture.Collision.Width, Texture.Collision.Height) : new Rectangle((int)(_position.X - diago_collision.X), (int)(_position.Y - diago_collision.Y), (int) (diago_collision.X * 2), (int)(diago_collision.Y * 2));
            if ((from sprite in units.Cast<Sprite>().ToList().Concat(buildings.Cast<Sprite>().ToList()).Concat(resources.Cast<Sprite>().ToList())
                 where sprite != this
                 select new Rectangle((int)sprite.Position.X + sprite.Texture.Collision.X, (int)sprite.Position.Y + sprite.Texture.Collision.Y, sprite.Texture.Collision.Width, sprite.Texture.Collision.Height))
                 .Any(sprec => sprec.Intersects(rec)))
            { return false; }

            // On teste la collision avec la carte
            var coos = (_texture != null) ? Game1.Xy2Matrice(new Vector2(_position.X + _texture.Width, _position.Y + _texture.Height * 4 / 5)) : Game1.Xy2Matrice(_position);
            if (coos.X >= 0 && coos.Y >= 0 && coos.X < matrice.GetLength(1) && coos.Y < matrice.GetLength(0))
            {
                if (!matrice[(int)coos.Y, (int)coos.X].Crossable)
                { return false; }
            }

            // Si aucune collision n'a été détéctée jusqu'ici, alors c'est que l'on est pas en collision
            return true;
        }

		// Comparateur de Sprite selon leur coordonnées en Y
		public static int CompareByY(Sprite s1, Sprite s2)
        { return ((s1.Position.Y + (s1.Texture == null ? 0 : s1.Texture.Height) == s2.Position.Y + (s2.Texture == null ? 0 : (s2.Texture.Height))) ? 0 : (s1.Position.Y + (s1.Texture == null ? 0 : s1.Texture.Height) > s2.Position.Y + (s2.Texture == null ? 0 : s2.Texture.Height)) ? 1 : -1); }

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
