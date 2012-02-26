using Microsoft.Xna.Framework.Content;
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

        protected int m_line_sight;
        public int Line_sight
        {
            get { return m_line_sight; }
            set { m_line_sight = value; }
        }

		public Unit(int x, int y)
            : base(x, y)
        {
        }
	}
}
