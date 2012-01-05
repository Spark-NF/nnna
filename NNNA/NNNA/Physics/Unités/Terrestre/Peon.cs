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
	class Peon : LandUnit
	{
		public Peon(int x, int y, ContentManager content, Joueur joueur)
			: base(x, y)
		{
			create_maison = false;
			m_joueur = joueur;
			joueur.Population += 1;
			type = "peon";
			joueur.Resource("Nourriture").Remove(50);
			m_attaque = 2;
			m_life = 30;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.06f;
			SetTextures(content, "peon", 90);
		}
		public Peon(int x, int y, ContentManager content, Joueur joueur, string sans_ressources)
			: base(x, y)
		{
			create_maison = false;
			m_joueur = joueur;
			joueur.Population += 1;
			type = "peon";
			m_attaque = 2;
			m_life = 30;
			m_portee = 1;
			m_regeneration = 1;
			m_speed = 0.06f;
			SetTextures(content, "peon", 90);
		}
	}
}
