using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	[Serializable]
	public class MovibleSprite : Sprite
	{
	    public string Will { get; set; }
	    public Building DestinationBuilding { get; set; }
	    public Unit DestinationUnit { get; set; }
		public ResourceMine DestinationResource { get; set; }
		public int Updates;
		protected int _dec = 90;
		public string Type { get; protected set; }
		public Dictionary<string, int> Prix { get; private set; }
		public double Angle { get; set; }
		public float Speed { get; set; }
		public bool Click { get; protected set; }
		protected Vector2 ClickPosition { get; set; }
		public bool Selected { get; set; }
		protected Vector2 _positionIni;
		protected Vector2 _cparcourir;
		protected Vector2 _cparcouru;
		protected Vector2 _direction;
		protected List<Sprite> _pathList;
		protected int _pathIterator;
		protected bool _clickInterne;

		[field: NonSerialized]
	    private Texture2D _icon;

		[field: NonSerialized]
	    protected Texture2D _selection;

	    public MovibleSprite(int x, int y)
			: base(x, y)
		{
	    	Speed = 0.1f;
	    	Angle = 90;
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
                    Angle = Math.Atan2(_cparcourir.Y, _cparcourir.X);
                    _direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
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
                    Angle = Math.Atan2(ClickPosition.Y - _position.Y, ClickPosition.X - _position.X);
                    _direction = new Vector2((float) Math.Cos(Angle), (float) Math.Sin(Angle));
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
