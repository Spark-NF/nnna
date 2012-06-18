using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace NNNA
{
	[Serializable]
	public class Unit : MovibleSprite
	{
        public Joueur Joueur { get; protected set;  }
		public Building Affiliate { get; protected set; }
		public int PochesMax { get; private set; }
	    private string PochesContent { get; set; }
	    public int Attaque { get; set;  }
		public int VitesseCombat { private get; set; }
		public int Portee { private get; set; }
		public int Regeneration { get; set; }
		public int LineSight { get; protected set; }
	    public List<Vector2> Moving { get; set; }

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

        public void ClickMouvement(GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> sprites, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, ContentManager content)
		{
			if (Click || Selected || DestinationUnit != null || DestinationBuilding != null)
			{
				if (Souris.Get().Clicked(MouseButton.Right) && (Souris.Get().Y <= hud.Position.Y + ((hud.Position.Height * 1) / 5) || hud.IsSmart) && (Selected || !Click))
				{
                    if (!Clavier.Get().Pressed(Keys.LeftControl) && !Clavier.Get().Pressed(Keys.RightControl))
                        Moving = new List<Vector2>();
				    var lol = Moving.Count == 0
				                  ? PathFinding.FindPath(Position +
				                                         new Vector2(Texture.Collision.X + Texture.Collision.Width/2,
				                                                     Texture.Collision.Y + Texture.Collision.Height/2),
				                                         Souris.Get().Position + camera.Position, buildings, resources, matrice,
				                                         new Vector2(Texture.Collision.Width/2 + 10, Texture.Collision.Height/2 + 10))
				                  : PathFinding.FindPath(Moving[Moving.Count - 1], Souris.Get().Position + camera.Position,
				                                         buildings, resources, matrice,
				                                         new Vector2(Texture.Collision.Width/2 + 10, Texture.Collision.Height/2 + 10));
				    if (lol != null && lol.Count > 0)
				        Moving.AddRange(lol);
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
                        Cparcouru = Vector2.Zero;
                        Texture.Animation = false;
                    }
                    else
                    {
                        translation = Direction * gameTime.ElapsedGameTime.Milliseconds * Speed;
                        Cparcouru = _position + new Vector2(Texture.Collision.X + Texture.Collision.Width / 2.0f, Texture.Collision.Y + Texture.Collision.Height / 2.0f) - PositionIni;
                        Update(translation);
                        Texture.Animation = true;
                    }
                    if (Cparcouru.LengthSquared() > Cparcourir.LengthSquared())
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
                                    Moving = new List<Vector2>
                                                 {
                                                     DestinationResource.Position +
                                                     new Vector2(
                                                         (float)
                                                         Math.Round((double) DestinationResource.Texture.Width/2),
                                                         (float)
                                                         Math.Round((double) DestinationResource.Texture.Height*0.95f)) -
                                                     new Vector2((float) Math.Round(Texture.Width/2.0f), Texture.Height)
                                                 };
                                    Move(Moving);
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
								    Moving =
								        new List<Vector2>
								             {
								                 DestinationBuilding.Position +
								                 new Vector2((float) Math.Round((double) DestinationBuilding.Texture.Width/2), 0)
								             };
									Move(Moving);
								}
							}
						}
						else
						{
                            if (Type == "archer" && DestinationUnit != null && Will == "attack" && Game1.Frame % VitesseCombat == 0 && (DestinationUnit.PositionCenter - PositionCenter).LengthSquared() < Portee*Portee)
                            {
                                ((Archer) this).Tirer(DestinationUnit, content);
                            }
                            if (DestinationUnit != null && Will == "attack" && Game1.Frame % VitesseCombat == 0 && Collides(new List<MovibleSprite> { DestinationUnit }, new List<Building>(), new List<ResourceMine>(), new Sprite[,] { }))
							{
								DestinationUnit.Life -= Attaque + Joueur.AdditionalAttack;
                                if (DestinationUnit.Life + DestinationUnit.Joueur.AdditionalLife <= 0)
								{ DestinationUnit = null; }
							}
							if (Collides(sprites, buildings, resources, matrice))
							{
                                if (Collides(sprites) && Will != "attack")
                                {
                                    var pos = Position + Minimap.Rotate(translation, Vector2.Zero, Math.PI/2);
                                    if (!(new Sprite(pos).Collides(sprites, buildings, resources, matrice)))
                                        Position = pos;
                                }
							    _position -= translation;
                                Move(Moving);
							}
						}
					}
				}
			}
		}

        private bool Collides(IEnumerable<MovibleSprite> units)
        {
            // On teste la collision entre notre rectangle et celui de toutes les autres unités
            var rec = new Rectangle((int)_position.X + Texture.Collision.X, (int)_position.Y + Texture.Collision.Y, Texture.Collision.Width, Texture.Collision.Height);
            if ((from sprite in units.Cast<Sprite>().ToList()
                 where sprite != this
                 select new Rectangle((int)sprite.Position.X + sprite.Texture.Collision.X, (int)sprite.Position.Y + sprite.Texture.Collision.Y, sprite.Texture.Collision.Width, sprite.Texture.Collision.Height))
                 .Any(sprec => sprec.Intersects(rec)))
            { return true; }

            // Si aucune collision n'a été détéctée jusqu'ici, alors c'est que l'on est pas en collision
            return false;
        }
		
		public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera, Color col)
		{
			var tex = 1;

			//MODE 8 ANGLES
			if (Dec == 45)
			{
				if (Angle > 1 * (Math.PI / 8) && Angle <= 3 * (Math.PI / 8))
				{ tex = 2; }
				else if (Angle > 3 * (Math.PI / 8) && Angle <= 5 * (Math.PI / 8))
				{ tex = 3; }
				else if (Angle > 5 * (Math.PI / 8) && Angle <= 7 * (Math.PI / 8))
				{ tex = 4; }
				else if (Angle > 7 * (Math.PI / 8) || Angle <= -7 * (Math.PI / 8))
				{ tex = 5; }
				else if (Angle > -7 * (Math.PI / 8) && Angle <= -5 * (Math.PI / 8))
				{ tex = 6; }
				else if (Angle > -5 * (Math.PI / 8) && Angle <= -3 * (Math.PI / 8))
				{ tex = 7; }
				else if (Angle > -3 * (Math.PI / 8) && Angle <= -1 * (Math.PI / 8))
				{ tex = 8; }
			}

			//MODE 4 ANGLES
			else
			{
				if (Angle > 1 * (Math.PI / 4) && Angle <= 3 * (Math.PI / 4))
				{ tex = 2; }
				else if (Angle > 3 * (Math.PI / 4) || Angle <= -3 * (Math.PI / 4))
				{ tex = 3; }
				else if (Angle > -3 * (Math.PI / 4) && Angle <= -1 * (Math.PI / 4))
				{ tex = 4; }
			}

			if (Selected)
			{ Selection.Draw(spriteBatch, _position - camera.Position + new Vector2(0, 32), Joueur.Color); }
			_texture.Draw(spriteBatch, _position - camera.Position, col, tex);
		}
	}
}
