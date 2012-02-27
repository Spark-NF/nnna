using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace NNNA
{
	class Building : Static_Sprite
	{
		protected int m_life = 0;
		public int Life
		{
			get { return m_life; }
			set
			{
				m_life = value;
				if (value > m_maxLife)
				{ m_maxLife = value; }
			}
		}
		protected int m_maxLife = 0;
		public int MaxLife
		{
			get { return m_maxLife; }
			set { m_maxLife = value; }
		}

        private int iterator;
        public int Iterator
        { get { return iterator; } set { iterator = value; } }

        protected int m_line_sight;
        public int Line_sight
        {
            get { return m_line_sight; }
            set { m_line_sight = value; }
        }

        protected Dictionary<string, int> m_cost = new Dictionary<string, int>();
        public Dictionary<string, int> Prix
        { get { return m_cost; } }

		public Building(int x, int y)
            : base(x, y)
        { iterator = 0; }

        public void Update_ere(ContentManager content, Joueur joueur)
        {
            LoadContent(content, m_texture.Name.Substring(0, m_texture.Name.Length - 1) + joueur.Ere.ToString());
        }
	}
}
