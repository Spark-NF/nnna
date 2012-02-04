﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;

namespace NNNA
{
	class Unit : Movible_Sprite
	{
		protected Joueur m_joueur;

		protected int m_life;
		public int Life
		{
			get { return m_life; }
			set { m_life = value; }
		}

		protected int m_attaque;
		public int Attaque
		{
			get { return m_attaque; }
			set { m_attaque = value; }
		}

		protected int m_portee;
		public int Portee
		{
			get { return m_portee; }
			set { m_portee = value; }
		}

		protected int m_regeneration;
		public int Regeneration
		{
			get { return m_regeneration; }
			set { m_regeneration = value; }
		}

		public Unit(int x, int y)
            : base(x, y)
        {
        }
	}
}
