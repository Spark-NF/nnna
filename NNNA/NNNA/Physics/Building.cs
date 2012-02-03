using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNNA
{
	class Building : Static_Sprite
	{
		protected int vie;

		protected Dictionary<string, int> m_cost = new Dictionary<string, int>();
		public Dictionary<string, int> Prix
		{ get { return m_cost; } }

		public Building(int x, int y)
            : base(x, y)
        { }
	}
}
