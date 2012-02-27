﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Peon : LandUnit
	{
		//SoundEffect creationPeon;
		public Peon(int x = 0, int y = 0)
			: base(x, y)
		{
			_cost.Add("Nourriture", 50);
		}

		public Peon(int x, int y, ContentManager content, Joueur joueur, bool removeResources = true)
			: base(x, y)
		{
			_createMaison = false;
			_joueur = joueur;
			joueur.Population++;
			_type = "peon";
			if (removeResources)
			{ joueur.Resource("Nourriture").Remove(50); }
			Attaque = 2;
			Life = 30;
			LineSight = 512;
			Portee = 1;
			Regeneration = 1;
			Speed = 0.06f;
			SetTextures(content, "peon");
			_cost.Add("Nourriture", 50);
			//creationPeon = content.Load<SoundEffect>("sounds/creationpeon");
		   // creationPeon.Play();
		}

		public void Collect(Resource ressource, int ere, GameTime time)
		{
			if ((time.TotalGameTime.Seconds % 5) == 0)
			{
				ressource.Remove(5);
				_joueur.Resource(ressource.Name(ere)).Add(5);
			}
		}
	}
}
