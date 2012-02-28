using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NNNA
{
	class Unit : MovibleSprite
	{
		protected Joueur _joueur;

		protected int _life;
		public int Life
		{
			get { return _life; }
			set
			{
				_life = value;
				if (value > _maxLife)
				{ _maxLife = value; }
			}
		}
		protected int _maxLife;
		public int MaxLife
		{
			get { return _maxLife; }
			set { _maxLife = value; }
		}

		protected int _attaque;
		public int Attaque
		{
			get { return _attaque; }
			set { _attaque = value; }
		}

		private int _vitesseCombat;
		public int VitesseCombat
		{
			get { return _vitesseCombat; }
			set { _vitesseCombat = value; }
		}

		protected int _portee;
		public int Portee
		{
			get { return _portee; }
			set { _portee = value; }
		}

		protected int _regeneration;
		public int Regeneration
		{
			get { return _regeneration; }
			set { _regeneration = value; }
		}

		protected int _lineSight;
		public int LineSight
		{
			get { return _lineSight; }
			set { _lineSight = value; }
		}

		public Unit(int x, int y)
			: base(x, y)
		{ }

		public void Attack(Unit obj)
		{ Destination = obj; }

		public void ClickMouvement(Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (_click || _selected || Destination != null)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (_selected || !_click))
				{
					Move(curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 8), (float)Math.Round((double)Texture.Height * 4 / 5)), sprites, buildings, matrice);
					Destination = null;
				}
				else if (Destination != null)
				{ Move(Destination.Position, sprites, buildings, matrice); }
				if (_click)
				{
					if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
					{ _click = false; }
					else
					{
						if (Destination != null && Destination.Position.DistanceTo(Position) < Math.Round((double)Texture.Width / 3) && Game1.Frame % _vitesseCombat == 0)
						{
							Destination.Life -= _attaque;
							if (Destination.Life <= 0)
							{ Destination = null; }
						}
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
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (_selected || !_click))
				{
					_click = true;
					_clickInterne = false;
					_positionIni = _position;
					_clickPosition = curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 8), (float)Math.Round((double)Texture.Height * 4 / 5));
					Vector2 start = Game1.Xy2Matrice(_positionIni);
					Vector2 destination = Game1.Xy2Matrice(curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 8), (float)Math.Round((double)Texture.Height * 4 / 5)));
					_pathList = PathFinding.FindPath(map, map[(int)start.Y, (int)start.X], map[(int)destination.Y, (int)destination.X]);
					if (_pathList != null)
					{ _pathIterator = _pathList.Count - 1; }
					else _click = false;
				}
				if (_click)
				{
					if (_pathIterator > 0)
					{
						if (!_clickInterne && _pathList != null)
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
	}
}
