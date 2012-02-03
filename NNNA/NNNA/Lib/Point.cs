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

namespace NNNA
{
	class Point
	{
		private int x, y;
		public int X { get; set; }
		public int Y { get; set; }

		public Point(int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public double distanceTo(Point p)
		{ return Math.Sqrt(Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2)); }
		public double distanceTo(Vector2 v)
		{ return Math.Sqrt(Math.Pow(v.X - X, 2) + Math.Pow(v.Y - Y, 2)); }
		public double distanceTo(Rectangle r)
		{
			int _x, _y;
			if (X < r.X)
			{ _x = r.X - X; }
			else if (X > r.X + r.Width)
			{ _x = X - (r.X + r.Width); }
			else
			{ _x = 0; }
			if (Y < r.Y)
			{ _y = r.Y - Y; }
			else if (Y > r.Y + r.Height)
			{ _y = Y - (r.Y + r.Height); }
			else
			{ _y = 0; }
			return Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2));
		}
	}
}
