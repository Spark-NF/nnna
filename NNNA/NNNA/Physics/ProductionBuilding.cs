using System;
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
	/// <summary>
	/// ProductionBuilding représente un bâtiment qui permet de produire des unités.
	/// </summary>
	class ProductionBuilding : Building
	{
		private Vector2 m_destination;

		public void RightClick(Vector2 coos, Camera2D camera)
		{ m_destination = coos; }

		public ProductionBuilding(int x, int y)
            : base(x, y)
        { }
	}
}
