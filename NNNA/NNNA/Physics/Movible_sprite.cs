using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class MovibleSprite : Sprite
	{
		protected string _will;
		public string Will
		{
			get { return _will; }
			set { _will = value; }
		}

		protected Building _destinationBuilding;
		public Building DestinationBuilding
		{
			get { return _destinationBuilding; }
			set { _destinationBuilding = value; }
		}
		protected Unit _destinationUnit;
		public Unit DestinationUnit
		{
			get { return _destinationUnit; }
			set { _destinationUnit = value; }
		}
		protected ResourceMine _destinationResource;
		public ResourceMine DestinationResource
		{
			get { return _destinationResource; }
			set { _destinationResource = value; }
		}

		protected Texture2D _icon;
		public Texture2D Icon
		{
			get { return _icon; }
			set { _icon = value; }
		}

		protected Texture2D _selection;
		public Texture2D Selection
		{
			get { return _selection; }
			set { _selection = value; }
		}

		private byte _a;
		public int Updates;
		protected int _dec = 90;

		protected bool _createMaison;
		public bool CreateMaison
		{ get { return _createMaison; } }

		protected bool _createHutteChasseurs;
		public bool CreateHutteChasseurs
		{ get { return _createHutteChasseurs; } }

		protected string _type;
		public string Type
		{ get { return _type; } }

		protected Vector2 _positionIni;
		public Vector2 PositionIni
		{
			get { return _positionIni; }
			set { _positionIni = value; }
		}

		protected Vector2 _cparcourir;
		public Vector2 Cparcourir
		{
			get { return _cparcourir; }
			set { _cparcourir = value; }
		}

		protected Vector2 _cparcouru;
		public Vector2 Cparcouru
		{
			get { return _cparcouru; }
			set { _cparcouru = value; }
		}

		protected double _angle = 90;
		public double Angle
		{
			get { return _angle; }
			set { _angle = value; }
		}

		protected Vector2 _direction;
		public Vector2 Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}

		protected float _speed = 0.1f;
		public float Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		protected List<Sprite> _pathList;
		protected int _pathIterator;

		protected Dictionary<string, int> _cost = new Dictionary<string, int>();
		public Dictionary<string, int> Prix
		{ get { return _cost; } }

		protected bool _clickInterne;

		protected bool _click;
		public bool Click
		{
			get { return _click; }
			set { _click = value; }
		}

		protected Vector2 _clickPosition;
		public Vector2 ClickPosition
		{
			get { return _clickPosition; }
			set { _clickPosition = value; }
		}

		internal bool _selected;
		public bool Selected
		{
			get { return _selected; }
			set { _selected = value; }
		}

		public MovibleSprite(int x, int y)
			: base(x, y)
		{
			_a = 0;
			_selected = false;
			_click = false;
			_clickInterne = false;
			Updates = 0;
		}
        public void Move(Vector2 coordinates)
        {
        	Vector2 pos = _position + new Vector2(Texture.Collision.X + Texture.Collision.Width/2.0f, Texture.Collision.Y + Texture.Collision.Height/2.0f);
			if (coordinates != pos)
            {
                _click = true;
                _texture.Animation = true;
				_clickPosition = coordinates;
				_cparcourir = _clickPosition - pos;
				_angle = Math.Atan2(_cparcourir.Y, _cparcourir.X);
                _direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
                _cparcouru = Vector2.Zero;
				_positionIni = pos;
            }
        }


		/// <summary>
		/// Initialise le déplacement de l'objet.
		/// </summary>
		/// <param name="coordinates">Les coordonnées de destination.</param>
		/// <param name="sprites">Les unités du monde.</param>
		/// <param name="buildings">Les bâtiments du monde.</param>
		/// <param name="matrice">La matrice de la carte.</param>
		public void Move(Vector2 coordinates, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (coordinates != _position)
			{
				_click = true;
				_texture.Animation = true;
				_clickPosition = coordinates;
				_angle = Math.Atan2(_clickPosition.Y - _position.Y, _clickPosition.X - _position.X);
				_direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
				_cparcourir = new Vector2(_clickPosition.X - _position.X, _clickPosition.Y - _position.Y);
				_cparcouru = Vector2.Zero;
				_positionIni = _position;
			}
		}

		public void Create_Maison(List<StaticSprite> staticSpriteList, ContentManager content, Joueur joueur, Camera2D camera, Random random)
		{
			if (joueur.Resource("Bois").Count >= 50)
			{
				if (_createMaison == false)
				{
					_a = (byte) (random.Next(0, 2) == 1 ? 0 : 1);
					_createMaison = true;
				}
				if (Souris.Get().Clicked(MouseButton.Right))
				{ _createMaison = false; }
				else if (Souris.Get().Clicked(MouseButton.Left))
				{/*
				Click = true;
				ClickPosition = curseur.Position + camera.Position;
				Angle = Math.Atan2(ClickPosition.Y - m_position.Y, ClickPosition.X - m_position.X);
				Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
				Cparcourir = new Vector2(ClickPosition.X - m_position.X, ClickPosition.Y - m_position.Y);
				Cparcouru = Vector2.Zero;
				PositionIni = m_position;
				curseur.Texture = content.Load<Texture2D>("pointer");
			}
			else if (create_maison == true && Click == false)
			{*/
					_createMaison = false;
					staticSpriteList.Add(new Hutte((int)(Souris.Get().X + camera.Position.X), (int)(Souris.Get().Y + camera.Position.Y), content, joueur, _a));
					MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
				}
			}
			else
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
				_createMaison = false;
			}
		}
		public void Create_Hutte_Chasseurs(List<StaticSprite> staticSpriteList, ContentManager content, Joueur joueur, Camera2D camera)
		{
			if (joueur.Resource("Bois").Count >= 75)
			{
				_createHutteChasseurs = true;
				if (Souris.Get().Clicked(MouseButton.Right))
				{ _createHutteChasseurs = false; }
				else if (Souris.Get().Clicked(MouseButton.Left))
				{/*
				Click = true;
				ClickPosition = curseur.Position + camera.Position;
				Angle = Math.Atan2(ClickPosition.Y - m_position.Y, ClickPosition.X - m_position.X);
				Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
				Cparcourir = new Vector2(ClickPosition.X - m_position.X, ClickPosition.Y - m_position.Y);
				Cparcouru = Vector2.Zero;
				PositionIni = m_position;
				curseur.Texture = content.Load<Texture2D>("pointer");
			}
			else if (create_maison == true && Click == false)
			{*/
					_createHutteChasseurs = false;
					staticSpriteList.Add(new HutteDesChasseurs((int)(Souris.Get().X + camera.Position.X), (int)(Souris.Get().Y + camera.Position.Y), content, joueur));
					MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
				}
			}
			else
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
				_createHutteChasseurs = false;
			}
		}
		public void DrawIcon(SpriteBatch spriteBatch, Vector2 position)
		{ spriteBatch.Draw(_icon, position, new Rectangle(0, 0, _icon.Width, _icon.Height), Color.White); }
		public void SetTextures(ContentManager content, string name, int dec = 90)
		{
			_dec = dec;
			_texture = new Image(content.Load<Texture2D>("Units/" + name + "/" + name), 4, 360/dec) { Animation = false };
			_go = new Image(content, "go", 8, 1, 5);
			_dots = content.Load<Texture2D>("dots");
			_selection = content.Load<Texture2D>("selected");
			_icon = content.Load<Texture2D>("Units/" + name + "/" + name + "_icon");
		}
	}
}
