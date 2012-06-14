using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class JoueurInternet : JoueurHuman
	{
		public JoueurInternet(Color couleur, string nom, ContentManager content)
			: base(couleur, nom, content, "internet")
		{ }
		public JoueurInternet(Joueur j, ContentManager content)
			: base(j.Color, j.Name, content, "internet")
		{ }

		public override void Update(GameTime gameTime, Camera2D camera, HUD hud, List<MovibleSprite> units, List<Building> buildings, List<ResourceMine> resources, Sprite[,] matrice, List<Sprite> toDraw)
		{
			throw new NotImplementedException();
		}
	}
}
