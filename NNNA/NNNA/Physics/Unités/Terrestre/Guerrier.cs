using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Guerrier : LandUnit
	{
        //SoundEffect creationguerrier;
        public Guerrier(int x = 0, int y = 0)
            : base(x, y)
        {
            _cost.Add("Nourriture", 70);
        }
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, bool removeResources = true)
			: base(x, y)
		{
			_joueur = joueur;
            joueur.Population++;
			_type = "guerrier";
			if (removeResources)
			{ joueur.Resource("Nourriture").Remove(70); }
			Attaque = 8;
			Life = 50;
            LineSight = 384;
			Portee = 1;
			Regeneration = 1;
			Speed = 0.05f;
			SetTextures(content, "guerrier", 45);
            _cost.Add("Nourriture", 70);
            //creationguerrier =content.Load<SoundEffect>("sounds/creationguerrier");
           // creationguerrier.Play();
		}
	}
}

