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
		protected int Dec = 90;
		public string Type { get; protected set; }
		public Dictionary<string, int> Prix { get; private set; }
		public double Angle { get; set; }
		public float Speed { get; set; }
		public bool Click { get; protected set; }
		protected Vector2 ClickPosition { get; set; }
		public bool Selected { get; set; }
		protected Vector2 PositionIni;
		protected Vector2 Cparcourir;
		protected Vector2 Cparcouru;
		protected Vector2 Direction;
		protected List<Sprite> PathList;
		protected int PathIterator;
		protected bool ClickInterne;

	    private Image _icon;
		protected Image Selection;

		public int MaxLife { get; private set; }
		private int _life;
		public int Life
		{
			get { return _life; }
			set
			{
				_life = value;
				if (value > MaxLife)
				{ MaxLife = value; }
			}
		}

	    public MovibleSprite(int x, int y)
			: base(x, y)
		{
	    	Speed = 0.1f;
	    	Angle = 90;
	    	Prix = new Dictionary<string, int>();
	        Selected = false;
			Click = false;
			ClickInterne = false;
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
                    Cparcourir = ClickPosition - pos;
                    Angle = Math.Atan2(Cparcourir.Y, Cparcourir.X);
                    Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
                    Cparcouru = Vector2.Zero;
                    PositionIni = pos;
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
                    Direction = new Vector2((float) Math.Cos(Angle), (float) Math.Sin(Angle));
                    Cparcourir = new Vector2(ClickPosition.X - _position.X, ClickPosition.Y - _position.Y);
                    Cparcouru = Vector2.Zero;
                    PositionIni = _position;
                }
                else
                    coordinates.RemoveAt(0);
            }
		}

		public void DrawIcon(SpriteBatch spriteBatch, Vector2 position)
		{ _icon.Draw(spriteBatch, position, Color.White); }
		protected void SetTextures(ContentManager content, string name, int dec = 90)
		{
			Dec = dec;
			_texture = new Image(content, "Units/" + name + "/" + name, 4, 360/dec) { Animation = false };
			Go = new Image(content, "go", 8, 1, 5);
			Dots = new Image(content, "dots");
			Selection = new Image(content, "selected");
			_icon = new Image(content, "Units/" + name + "/" + name + "_icon");
		}
	}
}
