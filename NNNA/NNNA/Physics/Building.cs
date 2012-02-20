﻿using System.Collections.Generic;

namespace NNNA
{
	class Building : Static_Sprite
	{
		protected int vie;

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
	}
}
