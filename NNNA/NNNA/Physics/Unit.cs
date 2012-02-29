using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		{
			DestinationUnit = obj;
			Will = "attaque";
		}
		public void Build(Building building)
		{
			DestinationBuilding = building;
			Will = "build";
		}

		public void ClickMouvement(Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, Sprite[,] matrice)
		{
			if (_click || _selected || DestinationUnit != null || DestinationBuilding != null)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (_selected || !_click))
				{
					Move(curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 2), (float)Math.Round((double)Texture.Height * 4 / 5)), sprites, buildings, matrice);
					DestinationUnit = null;
					DestinationBuilding = null;
				}
				else if (DestinationUnit != null)
				{ Move(DestinationUnit.Position, sprites, buildings, matrice); }
				else if (DestinationBuilding != null)
				{ Move(DestinationBuilding.Position + new Vector2((float)Math.Round((double)DestinationBuilding.Texture.Width / 2), 0), sprites, buildings, matrice); }
				if (_click)
				{
					Vector2 translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
					_cparcouru = _position - _positionIni;
					Update(translation);
					if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
					{
						_click = false;
						_texture.Animation = false;
						_position -= translation;
					}
					else
					{
						if (DestinationBuilding != null && Will == "build" && Collides(new List<MovibleSprite>(), new List<Building> { DestinationBuilding }, matrice))
						{
							_position -= translation;
							if (DestinationBuilding.Texture.Animation == false)
							{
								_joueur.Buildings.Add(DestinationBuilding);
								DestinationBuilding.Texture.Animation = true;
								DestinationBuilding.Texture.Single = true;
							}
							else if (DestinationBuilding.Texture.Finished)
							{ DestinationBuilding = null; }
						}
						else
						{
							if (DestinationUnit != null && Will == "attaque" && Game1.Frame % _vitesseCombat == 0 && Collides(new List<MovibleSprite> { DestinationUnit }, new List<Building>(), matrice))
							{
								DestinationUnit.Life -= _attaque;
								if (DestinationUnit.Life <= 0)
								{ DestinationUnit = null; }
							}
							if (Collides(sprites, buildings, matrice))
							{ _position -= translation; }
						}
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
					_clickPosition = curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 2), (float)Math.Round((double)Texture.Height * 4 / 5));
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
		public void Draw(SpriteBatch spriteBatch, Camera2D camera, int index, Color col)
		{
			int tex = 1;

			//MODE 8 ANGLES
			if (_dec == 45)
			{
				if (_angle > 1 * (Math.PI / 8) && _angle <= 3 * (Math.PI / 8))
				{ tex = 2; }
				else if (_angle > 3 * (Math.PI / 8) && _angle <= 5 * (Math.PI / 8))
				{ tex = 3; }
				else if (_angle > 5 * (Math.PI / 8) && _angle <= 7 * (Math.PI / 8))
				{ tex = 4; }
				else if (_angle > 7 * (Math.PI / 8) || _angle <= -7 * (Math.PI / 8))
				{ tex = 5; }
				else if (_angle > -7 * (Math.PI / 8) && _angle <= -5 * (Math.PI / 8))
				{ tex = 6; }
				else if (_angle > -5 * (Math.PI / 8) && _angle <= -3 * (Math.PI / 8))
				{ tex = 7; }
				else if (_angle > -3 * (Math.PI / 8) && _angle <= -1 * (Math.PI / 8))
				{ tex = 8; }
			}

			//MODE 4 ANGLES
			else
			{
				if (_angle > 1 * (Math.PI / 4) && _angle <= 3 * (Math.PI / 4))
				{ tex = 2; }
				else if (_angle > 3 * (Math.PI / 4) || _angle <= -3 * (Math.PI / 4))
				{ tex = 3; }
				else if (_angle > -3 * (Math.PI / 4) && _angle <= -1 * (Math.PI / 4))
				{ tex = 4; }
			}

			if (Selected)
			{
				if (Click)
				{
					var distance = (int)Math.Sqrt(Math.Pow(ClickPosition.X - Position.X, 2) + Math.Pow(ClickPosition.Y - Position.Y, 2));
					for (int i = 0; i < distance; i += 4)
					{ spriteBatch.Draw(_dots, ClickPosition - camera.Position + new Vector2(_go.Width, _texture.Height - (float)Math.Round((double)_go.Height / 2) - 1) - new Vector2((float)(i * Math.Cos(Angle)), (float)(i * Math.Sin(Angle))), Color.White); }
					if (_go != null && DestinationUnit == null)
					{ _go.Draw(spriteBatch, ClickPosition - camera.Position + new Vector2((float)Math.Round((double)_go.Width / 2), _texture.Height - _go.Height), Color.White); }
				}
				spriteBatch.Draw(_selection, _position - camera.Position + new Vector2(0, 32), _joueur.Color);
			}
			_texture.Draw(spriteBatch, _position - camera.Position, col, tex);
		}
	}
}
