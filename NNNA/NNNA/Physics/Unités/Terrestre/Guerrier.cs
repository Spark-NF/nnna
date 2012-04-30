using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Guerrier : LandUnit
	{
		// SoundEffect creationguerrier;
        public Guerrier(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Nourriture", 70);
        }
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, bool removeResources_et_pop = true)
			: base(x, y)
		{
			Joueur = joueur;
			_type = "guerrier";
			if (removeResources_et_pop)
			{
                if (Joueur.Population < joueur.PopulationMax)
                    joueur.Population++;
                joueur.Resource("Nourriture").Remove(70);
            }
			Attaque = 10;
			VitesseCombat = 30;
			Life = 100;
            LineSight = 384;
			Portee = 1;
			Regeneration = 1;
			Speed = 0.05f;
			SetTextures(content, "guerrier", 45);
            _cost.Add("Nourriture", 70);
			// creationguerrier =content.Load<SoundEffect>("sounds/creationguerrier");
			// creationguerrier.Play();
		}
	}
}

