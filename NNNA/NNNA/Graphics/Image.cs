using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	/// <summary>
	/// Cette classe permet de gérer des animations très facilement.
	/// </summary>
	class Image
	{
		private readonly Texture2D _texture;
		public Texture2D Texture
		{ get { return _texture; } }
		private readonly int _columns;
		private readonly int _rows;
		private readonly int _speed;
		private int _current;
		public readonly int Width;
		public readonly int Height;
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

		public Image(Texture2D texture, int columns = 1, int rows = 1, int speed = 15)
		{
			_texture = texture;
			_columns = columns;
			_rows = rows;
			_speed = speed;
			_current = 0;
			Width = _texture.Width / _columns;
			Height = _texture.Height / _rows;
			_animation = columns > 1;

		}
		public Image(ContentManager contentManager, string assetName, int columns = 1, int rows = 1, int speed = 15)
			: this(contentManager.Load<Texture2D>(assetName), columns, rows, speed)
		{ }

		public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, int which = 1)
		{
			if (_animation && !Finished && Game1.Frame % _speed == 0)
			{ _current = (_current + 1) % _columns; }
			spriteBatch.Draw(_texture, position, new Rectangle(_current * Width, (which - 1) * Height, Width, Height), color);
		}
	}
}
