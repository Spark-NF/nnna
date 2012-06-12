﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	public class Building : StaticSprite
	{
		protected Joueur _joueur;
		public Joueur Joueur
		{
			get { return _joueur; }
			set { _joueur = value; }
		}

		protected int _life;
		public int Life
		{
			get { return _life; }
			set
			{
				_life = value;
				if (value > _maxLife)
				{ _maxLife = value; }
			}
		}
		protected int _maxLife;
		public int MaxLife
		{
			get { return _maxLife; }
			set { _maxLife = value; }
		}

		private int _iterator;
		public int Iterator
		{
			get { return _iterator; }
			set { _iterator = value; }
		}

		protected int _lineSight;
		public int LineSight
		{
			get { return _lineSight; }
			set { _lineSight = value; }
		}

		protected Dictionary<string, int> _cost = new Dictionary<string, int>();
		public Dictionary<string, int> Prix
		{ get { return _cost; } }

		public Building(int x, int y)
			: base(x, y)
		{ _iterator = 0; }

		public void UpdateEre(ContentManager content, Joueur joueur)
		{
			LoadContent(content, _assetName.Substring(0, _assetName.Length - 1) + joueur.Ere.ToString(CultureInfo.CurrentCulture));
		}
	}
}
