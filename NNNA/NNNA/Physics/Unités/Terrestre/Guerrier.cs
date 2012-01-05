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
		public Guerrier(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			m_joueur = joueur;
			joueur.Population += 1;
			type = "guerrier";
			joueur.Resource("Nourriture").Remove(60);
			m_attaque = 8;
			m_life = 50;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.05f;
			SetTextures(content, "guerrier", 45);
		}
		public Guerrier(int x, int y, ContentManager content, Joueur joueur, string sans_ressources)
			: base(x, y)
		{
			m_joueur = joueur;
			joueur.Population += 1;
			type = "guerrier";
			m_attaque = 8;
			m_life = 50;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.05f;
			SetTextures(content, "guerrier", 45);
		}
	}
}

