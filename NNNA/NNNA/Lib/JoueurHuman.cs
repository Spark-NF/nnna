using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class JoueurHuman : Joueur
	{
		public JoueurHuman(Color couleur, string nom, ContentManager content, string type = "")
			: base(couleur, nom, content, "human")
		{ }
		public JoueurHuman(Joueur j, ContentManager content)
			: base(j.Color, j.Name, content, "internet")
		{ }

		public override void Update(GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> units, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, List<Sprite> toDraw)
		{
			throw new NotImplementedException();
		}
	}
}
