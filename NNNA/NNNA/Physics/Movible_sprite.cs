using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class MovibleSprite : Sprite
	{
		protected Dictionary<int, Texture2D> _textures = new Dictionary<int, Texture2D>();
		public Rectangle Rectangle(Camera2D cam)
		{
			return new Rectangle(
				(int)(Position.X - cam.Position.X),
				(int)(Position.Y - cam.Position.Y),
				Texture.Width / 4,
				Texture.Height
			);
		}

		protected Texture2D _icon;
		public Texture2D Icon
		{
			get { return _icon; }
			set { _icon = value; }
		}

		private byte _a;
		public int Updates;
		private int _dec = 90;

		protected bool _createMaison;
		public bool CreateMaison
		{ get { return _createMaison; } }

		protected bool _createHutteChasseurs;
		public bool CreateHutteChasseurs
		{ get { return _createHutteChasseurs; } }

		protected string _type;
		public string Type
		{ get { return _type; } }

		private Vector2 _positionIni;
		public Vector2 PositionIni
		{
			get { return _positionIni; }
			set { _positionIni = value; }
		}

		private Vector2 _cparcourir;
		public Vector2 Cparcourir
		{
			get { return _cparcourir; }
			set { _cparcourir = value; }
		}

		private Vector2 _cparcouru;
		public Vector2 Cparcouru
		{
			get { return _cparcouru; }
			set { _cparcouru = value; }
		}

		double _angle = 90;
		public double Angle
		{
			get { return _angle; }
			set { _angle = value; }
		}

		private Vector2 _direction;
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

		private List<Sprite> _pathList;
		private int _pathIterator;

		protected Dictionary<string, int> _cost = new Dictionary<string, int>();
		public Dictionary<string, int> Prix
		{ get { return _cost; } }

		private bool _clickInterne;

		private bool _click;
		public bool Click
		{
			get { return _click; }
			set { _click = value; }
		}
		private Vector2 _clickPosition;
		public Vector2 ClickPosition
		{
			get { return _clickPosition; }
			set { _clickPosition = value; }
		}
		private bool _selected;
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
				_clickPosition = coordinates;
				_angle = Math.Atan2(_clickPosition.Y - _position.Y, _clickPosition.X - _position.X);
				_direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
				_cparcourir = new Vector2(_clickPosition.X - _position.X, _clickPosition.Y - _position.Y);
				_cparcouru = Vector2.Zero;
				_positionIni = _position;
			}
		}

		public void ClickMouvement(Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (_click || _selected)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (_selected || !_click))
				{ Move(curseur.Position + camera.Position - new Vector2(Texture.Width / 8, (Texture.Height * 4) / 5), sprites, buildings, matrice); }
				if (_click == true)
				{
					if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
					{ _click = false; }
					else
					{
						_cparcouru = _position - _positionIni;
						Vector2 translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
						Update(translation);
						if (Collides(sprites, buildings, matrice))
						{ _position -= translation; }
					}
				}
			}
		}
		public void ClickMouvement(Sprite[,] map, Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (_click || _selected)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (_selected == true || _click == false))
				{
					_click = true;
					_clickInterne = false;
					_positionIni = _position;
					_clickPosition = curseur.Position + camera.Position - new Vector2(Texture.Width / 8, (Texture.Height * 4) / 5);
					Vector2 start = Game1.Xy2Matrice(_positionIni);
					Vector2 destination = Game1.Xy2Matrice(curseur.Position + camera.Position - new Vector2(Texture.Width / 8, (Texture.Height * 4) / 5));
					_pathList = PathFinding.FindPath(map, map[(int)start.Y, (int)start.X], map[(int)destination.Y, (int)destination.X]);
					if (_pathList != null)
					{
						_pathIterator = _pathList.Count - 1;
					}
					else _click = false;
				}
				if (_click)
				{
					if (_pathIterator > 0)
					{
						if (!_clickInterne)
						{
							_angle = Math.Atan2(_pathList[_pathIterator].PositionCenter.Y - _position.Y, _pathList[_pathIterator].PositionCenter.X - _position.X);
							_direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
							_cparcourir = new Vector2(_pathList[_pathIterator].PositionCenter.X - _position.X, _pathList[_pathIterator].PositionCenter.Y - _position.Y);
							_cparcouru = Vector2.Zero;
							_clickInterne = true;
						}
						if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
						{
							_pathIterator--;
							_positionIni = _position;
							_clickInterne = false;
						}
						else
						{
							_cparcouru = _position - _positionIni;
							Vector2 translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
							Update(translation);
							if (Collides(sprites, buildings, matrice))
							{ _position -= translation; }
						}
					}
					else if (_pathIterator == 0)
					{
						if (!_clickInterne)
						{
							_angle = Math.Atan2(_clickPosition.Y - _position.Y, _clickPosition.X - _position.X);
							_direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
							_cparcourir = new Vector2(_clickPosition.X - _position.X, _clickPosition.Y - _position.Y);
							_cparcouru = Vector2.Zero;
							_clickInterne = true;
						}
						if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
						{
							_pathIterator--;
							_positionIni = _position;
							_clickInterne = false;
						}
						else
						{
							_cparcouru = _position - _positionIni;
							Vector2 translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
							Update(translation);
							if (Collides(sprites, buildings, matrice))
							{ _position -= translation; }
						}
					}
					else _click = false;
				}
			}
		}

		public void Create_Maison(Sprite curseur, List<StaticSprite> staticSpriteList, ContentManager content, Joueur joueur, Camera2D camera, Random random)
		{
			if (joueur.Resource("Bois").Count >= 50)
			{
				if (_createMaison == false)
				{
					if (random.Next(0, 2) == 1)
					{
						curseur.Texture = content.Load<Texture2D>("Batiments/hutte1");
						_a = 0;
					}
					else
					{
						curseur.Texture = content.Load<Texture2D>("Batiments/hutte2");
						_a = 1;
					}
					_createMaison = true;
				}
				if (Souris.Get().Clicked(MouseButton.Right))
				{
					_createMaison = false;
					curseur.Texture = content.Load<Texture2D>("pointer");
				}
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
					curseur.Texture = content.Load<Texture2D>("pointer");
					_createMaison = false;
					staticSpriteList.Add(new Hutte((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), content, joueur, _a));
					MessagesManager.Messages.Add(new Msg("Nouvelle hutte !", Color.White, 5000));
				}
			}
			else
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
				_createMaison = false;
			}
		}
		public void Create_Hutte_Chasseurs(Sprite curseur, List<StaticSprite> Static_Sprite_List, ContentManager content, Joueur joueur, Camera2D camera)
		{
			if (joueur.Resource("Bois").Count >= 75)
			{
				if (_createHutteChasseurs == false)
				{
					curseur.Texture = content.Load<Texture2D>("Batiments/hutte_des_chasseurs");
					_createHutteChasseurs = true;
				}
				if (Souris.Get().Clicked(MouseButton.Right))
				{
					_createHutteChasseurs = false;
					curseur.Texture = content.Load<Texture2D>("pointer");
				}
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
					curseur.Texture = content.Load<Texture2D>("pointer");
					_createHutteChasseurs = false;
					Static_Sprite_List.Add(new HutteDesChasseurs((int)(curseur.Position.X + camera.Position.X), (int)(curseur.Position.Y + camera.Position.Y), content, joueur));
					MessagesManager.Messages.Add(new Msg("Nouvelle hutte des chasseurs !", Color.White, 5000));
				}
			}
			else
			{
				MessagesManager.Messages.Add(new Msg("Vous n'avez pas assez de bois.", Color.Red, 5000));
				_createHutteChasseurs = false;
			}
		}
		public void Draw(SpriteBatch spriteBatch, Camera2D camera, int index, Color col)
		{
			int tex = 0;

			//MODE 8 ANGLES
			if (_dec == 45)
			{
				if (_angle > 1 * (Math.PI / 8) && _angle <= 3 * (Math.PI / 8))
				{ tex = 45; }
				else if (_angle > 3 * (Math.PI / 8) && _angle <= 5 * (Math.PI / 8))
				{ tex = 90; }
				else if (_angle > 5 * (Math.PI / 8) && _angle <= 7 * (Math.PI / 8))
				{ tex = 135; }
				else if (_angle > 7 * (Math.PI / 8) || _angle <= -7 * (Math.PI / 8))
				{ tex = 180; }
				else if (_angle > -7 * (Math.PI / 8) && _angle <= -5 * (Math.PI / 8))
				{ tex = 225; }
				else if (_angle > -5 * (Math.PI / 8) && _angle <= -3 * (Math.PI / 8))
				{ tex = 270; }
				else if (_angle > -3 * (Math.PI / 8) && _angle <= -1 * (Math.PI / 8))
				{ tex = 315; }
			}

			//MODE 4 ANGLES
			else
			{
				if (_angle > 1 * (Math.PI / 4) && _angle <= 3 * (Math.PI / 4))
				{ tex = 90; }
				else if (_angle > 3 * (Math.PI / 4) || _angle <= -3 * (Math.PI / 4))
				{ tex = 180; }
				else if (_angle > -3 * (Math.PI / 4) && _angle <= -1 * (Math.PI / 4))
				{ tex = 270; }
			}

			_texture = _textures[tex];
			if (Selected)
			{
				if (Click)
				{
					var distance = (int)Math.Sqrt(Math.Pow((ClickPosition.X/* + m_go.Width / 2*/) - (Position.X/* + m_texture.Width / 8*/), 2) + Math.Pow((ClickPosition.Y/* + m_go.Height / 2*/) - (Position.Y/* + (m_texture.Height * 4) / 5*/), 2));
					for (int i = 0; i < distance; i += 4)
					{ spriteBatch.Draw(_dots, ClickPosition - camera.Position + new Vector2(_go.Width, _texture.Height - (_go.Height / 2) - 1) - new Vector2((float)(i * Math.Cos(Angle)), (float)(i * Math.Sin(Angle))), Color.White); }
					if (_go != null)
					{ spriteBatch.Draw(_go, ClickPosition - camera.Position + new Vector2(_go.Width / 2, _texture.Height - (_go.Height)), Color.White); }
				}
				spriteBatch.Draw(_texture, _position - camera.Position, new Rectangle(_click ? index * 32 : 0, 0, 32, 48), Color.Peru);
			}
			else
			{ spriteBatch.Draw(_texture, _position - camera.Position, new Rectangle(_click ? index * 32 : 0, 0, 32, 48), col); }
		}
		public void DrawIcon(SpriteBatch spriteBatch, Vector2 position)
		{ spriteBatch.Draw(_icon, position, new Rectangle(0, 0, _icon.Width, _icon.Height), Color.White); }
		public void SetTextures(ContentManager content, string name, int dec = 90)
		{
			_dec = dec;
			for (int i = 0; i <= 315; i += dec)
			{ _textures.Add(i, content.Load<Texture2D>("Units/" + name + "/" + name + "_" + i.ToString())); }
			_texture = _textures[0];
			_go = content.Load<Texture2D>("go");
			_dots = content.Load<Texture2D>("dots");
			_icon = content.Load<Texture2D>("Units/" + name + "/" + name + "_icon");
		}
	}
}
