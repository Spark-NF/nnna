﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace NNNA
{
	class Unit : MovibleSprite
	{
        public Joueur Joueur { get; protected set;  }
		public Building Affiliate { get; protected set; }
		public int PochesMax { get; private set; }
	    private string PochesContent { get; set; }
		public int MaxLife { get; private set; }
	    protected int Attaque { get; set;  }
		public int VitesseCombat { private get; set; }
		public int Portee { private get; set; }
		public int Regeneration { get; set; }
		public int LineSight { get; protected set; }
	    public List<Vector2> Moving { get; private set; }

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

	    private int _poches;
		public int Poches
		{
			get { return _poches; }
		    private set { _poches = value >= PochesMax ? PochesMax : value; }
		}

	    protected Unit(int x, int y)
			: base(x, y)
		{
			PochesMax = 50;
            Moving = new List<Vector2>();
			Poches = 0;
		}

		public void Attack(Unit obj)
		{
			DestinationUnit = obj;
			Will = "attack";
		}
		public void Build(Building building)
		{
                DestinationBuilding = building;
                DestinationBuilding.Texture.Part = 0;
                Will = "build";
		}
		public void Mine(ResourceMine resource)
		{
			DestinationResource = resource;
			Will = "mine";
		}

		public void ClickMouvement(Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, ContentManager content)
		{
			if (Click || Selected || DestinationUnit != null || DestinationBuilding != null)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && (curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) || hud.IsSmart) && (Selected || !Click))
				{
                    if (!Clavier.Get().Pressed(Keys.LeftControl) && !Clavier.Get().Pressed(Keys.RightControl))
                        Moving = new List<Vector2>();
                    Moving.Add(curseur.Position + camera.Position);
                    Move(Moving);
					DestinationUnit = null;
					DestinationBuilding = null;
					DestinationResource = null;
				}
                else if (DestinationUnit != null)
                { Moving = new List<Vector2> { DestinationUnit.Position + new Vector2(DestinationUnit.Texture.Collision.X + DestinationUnit.Texture.Collision.Width / 2, DestinationUnit.Texture.Collision.Y + DestinationUnit.Texture.Collision.Height / 2) }; Move(Moving); }
				else if (DestinationBuilding != null)
                { Moving = new List<Vector2> { DestinationBuilding.Position + new Vector2(DestinationBuilding.Texture.Collision.X + DestinationBuilding.Texture.Collision.Width / 2, DestinationBuilding.Texture.Collision.Y + DestinationBuilding.Texture.Collision.Height / 2) }; Move(Moving); }
				else if (DestinationResource != null)
                { Moving = new List<Vector2> { DestinationResource.Position + new Vector2(DestinationResource.Texture.Collision.X + DestinationResource.Texture.Collision.Width / 2, DestinationResource.Texture.Collision.Y + DestinationResource.Texture.Collision.Height / 2) }; Move(Moving); }
                if (Click)
                {
                    var translation = Vector2.Zero;
                    if (Type == "archer" && Will == "attack" && DestinationUnit != null && (DestinationUnit.PositionCenter - PositionCenter).LengthSquared() < Portee * Portee)
                    {
                        _cparcouru = Vector2.Zero;
                        Texture.Animation = false;
                    }
                    else
                    {
                        translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
                        _cparcouru = _position + new Vector2(Texture.Collision.X + Texture.Collision.Width / 2.0f, Texture.Collision.Y + Texture.Collision.Height / 2.0f) - _positionIni;
                        Update(translation);
                        Texture.Animation = true;
                    }
                    if (Math.Abs(_cparcouru.X) >= Math.Abs(_cparcourir.X) && Math.Abs(_cparcouru.Y) >= Math.Abs(_cparcourir.Y))
					{
						Click = false;
						_texture.Animation = false;
						_position -= translation;
                        if (Moving.Count > 0)
                            Moving.RemoveAt(0);
                        Move(Moving);
					}
					else
					{
						if (DestinationBuilding != null && (Will == "build" || Will == "poches") && Collides(new List<MovibleSprite>(), new List<Building> { DestinationBuilding }, new List<ResourceMine>(), matrice))
						{
							_position -= translation;
							if (Will == "build")
							{
								if (DestinationBuilding.Texture.Part == 0)
								{ Joueur.Buildings.Add(DestinationBuilding); }
								if (DestinationBuilding.Texture.Part < DestinationBuilding.Texture.Height)
								{ DestinationBuilding.Texture.Part++; }
								else
								{ DestinationBuilding = null; }
							}
							else // Will == "poches"
							{
								Joueur.Resource(PochesContent).Add(Poches);
								DestinationBuilding = null;
								Poches = 0;
								PochesContent = "";
								if (DestinationResource != null)
								{
									Will = "mine";
									Move(new List<Vector2> { DestinationResource.Position + new Vector2((float)Math.Round((double)DestinationResource.Texture.Width / 2), (float)Math.Round((double)DestinationResource.Texture.Height * 0.95f)) - new Vector2((float)Math.Round(Texture.Width / 2.0f), Texture.Height) });
								}
							}
						}
						else if (DestinationResource != null && Will == "mine" && Collides(new List<MovibleSprite>(), new List<Building>(), new List<ResourceMine> { DestinationResource }, new Sprite[,] { }))
						{
							if (DestinationResource.Quantity <= 0)
							{ DestinationResource = null; }
							else
							{
								_position -= translation;
								// Si on change de type de ressource à miner, on vide ses poches d'abord
								if (DestinationResource.Resource.Id != PochesContent)
								{
									Poches = 0;
									PochesContent = DestinationResource.Resource.Id;
								}
								Poches += DestinationResource.Mine(PochesMax - Poches, this);
								// Si l'unité a les poches pleines, on doit aller les vider à la base affiliée à l'unité
								if (Poches >= PochesMax)
								{
									Will = "poches";
									DestinationBuilding = Affiliate;
									Move(new List<Vector2> { DestinationBuilding.Position + new Vector2((float)Math.Round((double)DestinationBuilding.Texture.Width / 2), 0) }); //, sprites, buildings, matrice);
								}
							}
						}
						else
						{
                            if (Type == "archer" && DestinationUnit != null && Will == "attack" && Game1.Frame % VitesseCombat == 0 && (DestinationUnit.PositionCenter - PositionCenter).LengthSquared() < Portee*Portee)
                            {
                                ((Archer) this).Tirer(DestinationUnit, content);
                                DestinationUnit.Life -= Attaque + Joueur.AdditionalAttack;
                                if (DestinationUnit.Life + DestinationUnit.Joueur.AdditionalLife <= 0)
                                { DestinationUnit = null; }
                            }
                            if (DestinationUnit != null && Will == "attack" && Game1.Frame % VitesseCombat == 0 && Collides(new List<MovibleSprite> { DestinationUnit }, new List<Building>(), new List<ResourceMine>(), new Sprite[,] { }))
							{
								DestinationUnit.Life -= Attaque + Joueur.AdditionalAttack;
                                if (DestinationUnit.Life + DestinationUnit.Joueur.AdditionalLife <= 0)
								{ DestinationUnit = null; }
							}
							if (Collides(sprites, buildings, resources, matrice))
							{ _position -= translation; }
						}
					}
				}
			}
		}
		public void ClickMouvement(Sprite[,] map, Sprite curseur, GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice)
		{
			if (Click || Selected)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && curseur.Position.Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) && (Selected || !Click))
				{
					Click = true;
					_clickInterne = false;
					_positionIni = _position;
					ClickPosition = curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 2), (float)Math.Round((double)Texture.Height * 4 / 5));
					var start = Game1.Xy2Matrice(_positionIni);
					var destination = Game1.Xy2Matrice(curseur.Position + camera.Position - new Vector2((float)Math.Round((double)Texture.Width / 8), (float)Math.Round((double)Texture.Height * 4 / 5)));
					_pathList = PathFinding.FindPath(map, map[(int)start.Y, (int)start.X], map[(int)destination.Y, (int)destination.X]);
					if (_pathList != null)
					{ _pathIterator = _pathList.Count - 1; }
					else Click = false;
				}
				if (Click)
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
							var translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
							Update(translation);
							if (Collides(sprites, buildings, resources, matrice))
							{ _position -= translation; }
						}
					}
					else if (_pathIterator == 0)
					{
						if (!_clickInterne)
						{
							_angle = Math.Atan2(ClickPosition.Y - _position.Y, ClickPosition.X - _position.X);
							_direction = new Vector2((float)Math.Cos(_angle), (float)Math.Sin(_angle));
							_cparcourir = new Vector2(ClickPosition.X - _position.X, ClickPosition.Y - _position.Y);
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
							var translation = _direction * gameTime.ElapsedGameTime.Milliseconds * _speed;
							Update(translation);
							if (Collides(sprites, buildings, resources, matrice))
							{ _position -= translation; }
						}
					}
					else Click = false;
				}
			}
		}
		public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera, Color col)
		{
			var tex = 1;

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
				spriteBatch.Draw(_selection, _position - camera.Position + new Vector2(0, 32), Joueur.Color);
			}
			_texture.Draw(spriteBatch, _position - camera.Position, col, tex);
		}
	}
}
