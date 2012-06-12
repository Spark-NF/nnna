using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	/// <summary>
	/// Cette classe permet de gérer des animations très facilement.
	/// </summary>
	[Serializable]
	public class Image
	{
		public int Part { get; set; }
		private readonly int _columns;
		private readonly int _rows;
		private readonly int _speed;
		private int _current;
		public readonly int Width;
		public readonly int Height;
		public readonly Rectangle Collision;
		private bool _animation;
		public bool Animation
		{
			get { return _animation; }
			set
			{
				_animation = value;
				if (!_animation)
				{ _current = 0; }
			}
		}
		private bool _single;
		public bool Single
		{
			get { return _single; }
			set { _single = value; }
		}
		public bool Finished
		{ get { return _single && _current == _columns - 1; } }

		[field: NonSerialized]
		private readonly Texture2D _texture;
		public Texture2D Texture
		{ get { return _texture; } }

		public Image(Texture2D texture, int columns = 1, int rows = 1, int speed = 15)
		{
			_texture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
			_columns = columns;
			_rows = rows;
			_speed = speed;
			_current = 0;
			Width = _texture.Width / _columns;
			Height = _texture.Height / _rows;
			Part = Height;
			_animation = columns > 1;

			// Copie de la texture
			if (_rows > 1 || _columns > 1)
			{
				var dta = new Color[texture.Width * texture.Height];
				texture.GetData(dta);
				_texture.SetData(dta);
			}

			// Détection du rectangle de collision
			Collision = new Rectangle(0, 0, Width, Height);
			var data = new Color[Width * Height];
			texture.GetData(0, new Rectangle(0, 0, Width, Height), data, 0, Width * Height);
			var coll = new Color(254, 0, 254, 254);
			bool origin = false, dest = false;
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (!origin && x < Width - 1 && y < Height - 1 && data[y * Width + x] == coll && data[y * Width + x + 1] == coll && data[(y + 1) * Width + x] == coll)
					{
						Collision.X = x;
						Collision.Y = y;
						data[y * Width + x] = Color.Transparent;
						data[y * Width + x + 1] = Color.Transparent;
						data[(y + 1) * Width + x] = Color.Transparent;
						origin = true;
					}
					else if (origin && !dest && x > Collision.X && y > Collision.Y && data[y * Width + x] == coll && data[y * Width + x - 1] == coll && data[(y - 1) * Width + x] == coll)
					{
						Collision.Width = x - Collision.X + 1;
						Collision.Height = y - Collision.Y + 1;
						data[y * Width + x] = Color.Transparent;
						data[y * Width + x - 1] = Color.Transparent;
						data[(y - 1) * Width + x] = Color.Transparent;
						dest = true;
					}
				}
			}
			_texture.SetData(0, new Rectangle(0, 0, Width, Height), data, 0, Width * Height);
		}
		public Image(ContentManager contentManager, string assetName, int columns = 1, int rows = 1, int speed = 15)
			: this(contentManager.Load<Texture2D>(assetName), columns, rows, speed)
		{ }

		public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, int which = 1)
		{
			if (_animation && !Finished && Game1.Frame % _speed == 0)
			{ _current = (_current + 1) % _columns; }
			spriteBatch.Draw(_texture, position + new Vector2(0, Height - Part), new Rectangle(_current * Width, which * Height - Part, Width, Part), color);
		}
	}
}
