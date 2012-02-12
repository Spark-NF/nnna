using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNNA
{
	class Building : Static_Sprite
	{
		protected int vie;

        private int iterator;
        public int Iterator
        { get { return iterator; } set { iterator = value; } }

        protected Dictionary<string, int> m_cost = new Dictionary<string, int>();
        public Dictionary<string, int> Prix
        { get { return m_cost; } }

		public Building(int x, int y)
            : base(x, y)
        { iterator = 0; }
	}
}
