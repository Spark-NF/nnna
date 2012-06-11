using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class MovibleSprite : Sprite
	{
	    public string Will { get; set; }

	    public Building DestinationBuilding { get; set; }

	    public Unit DestinationUnit { get; set; }

	    public ResourceMine DestinationResource { get; set; }

	    private Texture2D _icon;

	    protected Texture2D _selection;

	    public int Updates;
		protected int _dec = 90;

		protected string _type;
		public string Type
		{ get { return _type; } }

		protected Vector2 _positionIni;

	    protected Vector2 _cparcourir;

	    protected Vector2 _cparcouru;

	    protected double _angle = 90;
		public double Angle
		{
			get { return _angle; }
			set { _angle = value; }
		}

		protected Vector2 _direction;

	    protected float _speed = 0.1f;
		public float Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		protected List<Sprite> _pathList;
		protected int _pathIterator;

	    public Dictionary<string, int> Prix { get; private set; }

	    protected bool _clickInterne;

	    public bool Click { get; protected set; }

	    protected Vector2 ClickPosition { get; set; }

	    public bool Selected { get; set; }

	    public MovibleSprite(int x, int y)
			: base(x, y)
		{
	        Prix = new Dictionary<string, int>();
	        Selected = false;
			Click = false;
			_clickInterne = false;
			Updates = 0;
		}
        public void Move(List<Vector2> coordinates)
        {
        	var pos = _position + new Vector2(Texture.Collision.X + Texture.Collision.Width/2.0f, Texture.Collision.Y + Texture.Collision.Height/2.0f);
            if (coordinates.Count > 0)
            {
                if (coordinates[0] != _position)
                {
                    Click = true;
                    _texture.Animation = true;
                    ClickPosition = coordinates[0];
                    _cparcourir = ClickPosition - pos;
                    _angle = Math.Atan2(_cparcourir.Y, _cparcourir.X);
                    _direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
                    _cparcouru = Vector2.Zero;
                    _positionIni = pos;
                }
            }
        }


		/// <summary>
		/// Initialise le déplacement de l'objet.
		/// </summary>
		/// <param name="coordinates">Les coordonnées de destination.</param>
		/// <param name="sprites">Les unités du monde.</param>
		/// <param name="buildings">Les bâtiments du monde.</param>
		/// <param name="matrice">La matrice de la carte.</param>
		public void Move(List<Vector2> coordinates, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
            if (coordinates.Count > 0)
            {
                if (coordinates[0] != _position)
                {
                    Click = true;
                    _texture.Animation = true;
                    ClickPosition = coordinates[0];
                    _angle = Math.Atan2(ClickPosition.Y - _position.Y, ClickPosition.X - _position.X);
                    _direction = new Vector2((float) Math.Cos(_angle), (float) Math.Sin(_angle));
                    _cparcourir = new Vector2(ClickPosition.X - _position.X, ClickPosition.Y - _position.Y);
                    _cparcouru = Vector2.Zero;
                    _positionIni = _position;
                }
                else
                    coordinates.RemoveAt(0);
            }
		}

		public void DrawIcon(SpriteBatch spriteBatch, Vector2 position)
		{ spriteBatch.Draw(_icon, position, new Rectangle(0, 0, _icon.Width, _icon.Height), Color.White); }
		public void SetTextures(ContentManager content, string name, int dec = 90)
		{
			_dec = dec;
			_texture = new Image(content.Load<Texture2D>("Units/" + name + "/" + name), 4, 360/dec) { Animation = false };
			Go = new Image(content, "go", 8, 1, 5);
			Dots = content.Load<Texture2D>("dots");
			_selection = content.Load<Texture2D>("selected");
			_icon = content.Load<Texture2D>("Units/" + name + "/" + name + "_icon");
		}
	}
}
