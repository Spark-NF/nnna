using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	[Serializable]
	public class Building : StaticSprite
	{
	    public Joueur Joueur { get; set; }

	    protected int _life;
		public int Life
		{
			get { return _life + Joueur.AdditionalBuildingLife; }
		    protected set
			{
				_life = value;
                if (value > MaxLife + Joueur.AdditionalBuildingLife)
				{ MaxLife = value - Joueur.AdditionalBuildingLife; }
			}
		}

	    private int _maxLife;
        public int MaxLife { get { return (Joueur != null) ? _maxLife + Joueur.AdditionalBuildingLife : _maxLife; } private set { _maxLife = value; } }

	    public int Iterator { get; set; }

	    public int LineSight { get; set; }

	    public Dictionary<string, int> Prix { get; private set; }

	    protected Building(int x, int y)
			: base(x, y)
		{
		    Prix = new Dictionary<string, int>();
		    Iterator = 0;
		}

	    public void UpdateEre(ContentManager content, Joueur joueur)
		{
			LoadContent(content, _assetName.Substring(0, _assetName.Length - 1) + joueur.Ere.ToString(CultureInfo.CurrentCulture));
		}
	}
}
