using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class JoueurAI : Joueur
	{
		public JoueurAI(Color couleur, string nom, ContentManager content)
			: base(couleur, nom, content, "ai")
		{ }

		public override void Update(GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> units, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, List<Sprite> toDraw)
		{
			for (int i = 0; i < Units.Count; i++)
			{
				var unit = (Unit)Units[i];
				if (unit.Life <= 0)
				{
					Units.Remove(unit);
					units.Remove(unit);
					toDraw.Remove(unit);
				}
				else
				{
					if (++unit.Updates == 120)
					{
						unit.Move(new List<Vector2> { unit.Position + new Vector2(Rand.Next(-40, 41), Rand.Next(-40, 41)) }, units, buildings, matrice);
						unit.Updates = Rand.Next(0, 40);
					}
					unit.ClickMouvement(gameTime, camera, hud, units, buildings, resources, matrice, Content);
				}
			}
		}
	}
}
