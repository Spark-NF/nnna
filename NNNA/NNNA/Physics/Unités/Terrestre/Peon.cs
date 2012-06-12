using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class Peon : LandUnit
	{
		//SoundEffect creationPeon;
		public Peon(int x = 0, int y = 0)
			: base(x, y)
		{
			Prix.Add("Nourriture", 50);
		}

		public Peon(int x, int y, ContentManager content, Joueur joueur, Building affiliate, bool removeResources = true, bool add_pop = true)
			: base(x, y)
		{
		    Joueur = joueur;
			Type = "peon";
            if (add_pop)
            {
                joueur.Population++;
            }
			if (removeResources)
			{
                joueur.Resource("Nourriture").Remove(50);
            }
			Attaque = 2;
			VitesseCombat = 20;
			Life = 50;
			LineSight = 512;
			Portee = 1;
			Regeneration = 1;
			Speed = 0.06f + joueur.AdditionalSpeed;
			SetTextures(content, "peon");
			Prix.Add("Nourriture", 50);
			Affiliate = affiliate;
		}

		public void Collect(Resource ressource, int ere, GameTime time)
		{
			if ((time.TotalGameTime.Seconds % 5) == 0)
			{
				ressource.Remove(5);
				Joueur.Resource(ressource.Name(ere)).Add(5);
			}
		}
	}
}
