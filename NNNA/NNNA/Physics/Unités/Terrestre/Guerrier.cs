using System;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	class Guerrier : LandUnit
	{
		// SoundEffect creationguerrier;
        public Guerrier(int x = 0, int y = 0)
            : base(x, y)
        {
            Prix.Add("Nourriture", 70);
        }
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, bool removeResources = true, bool add_pop = true)
			: base(x, y)
		{
            Joueur = joueur;
			Type = "guerrier";
            if (add_pop)
            {
                joueur.Population++;
            }
			if (removeResources)
			{
                joueur.Resource("Nourriture").Remove(70);
            }
			Attaque = 10;
			VitesseCombat = 30;
			Life = 100;
            LineSight = 384;
			Portee = 1;
			Regeneration = 1;
            Speed = 0.05f + joueur.AdditionalSpeed;
			SetTextures(content, "guerrier", 45);
            Prix.Add("Nourriture", 70);
			// creationguerrier =content.Load<SoundEffect>("sounds/creationguerrier");
			// creationguerrier.Play();
		}
	}
}

