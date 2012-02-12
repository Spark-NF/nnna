using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace NNNA
{
	class Guerrier : LandUnit
	{
        public Guerrier(int x = 0, int y = 0)
            : base(x, y)
        {
            m_cost.Add("Nourriture", 70);
        }
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, bool remove_resources = true)
			: base(x, y)
		{
			m_joueur = joueur;
			joueur.Population += 1;
			type = "guerrier";
			if (remove_resources)
			{ joueur.Resource("Nourriture").Remove(60); }
			m_attaque = 8;
			m_life = 50;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.05f;
			SetTextures(content, "guerrier", 45);
            m_cost.Add("Nourriture", 70);
		}
	}
}

