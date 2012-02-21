﻿using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Guerrier : LandUnit
	{
        //SoundEffect creationguerrier;
        public Guerrier(int x = 0, int y = 0)
            : base(x, y)
        {
            m_cost.Add("Nourriture", 70);
        }
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, bool remove_resources = true)
			: base(x, y)
		{
			m_joueur = joueur;
            joueur.Population++;
			type = "guerrier";
			if (remove_resources)
			{ joueur.Resource("Nourriture").Remove(70); }
			m_attaque = 8;
			m_life = 50;
            m_line_sight = 384;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.05f;
			SetTextures(content, "guerrier", 45);
            m_cost.Add("Nourriture", 70);
            //creationguerrier =content.Load<SoundEffect>("sounds/creationguerrier");
           // creationguerrier.Play();
		}
	}
}

