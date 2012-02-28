using System;
using Microsoft.Xna.Framework;

namespace NNNA
{
	public static class Vector2DExtander
	{
		public static double DistanceTo(this Vector2 t, Point p, bool useZ = true)
		{ return Math.Sqrt(Math.Pow(p.X - t.X, 2) + Math.Pow(p.Y - t.Y, 2) + (useZ ? p.Z * p.Z : 0)); }
		public static double DistanceTo(this Vector2 t, Vector3 v, bool useZ = true)
		{ return Math.Sqrt(Math.Pow(v.X - t.X, 2) + Math.Pow(v.Y - t.Y, 2) + (useZ ? v.Z * v.Z : 0)); }
		public static double DistanceTo(this Vector2 t, Rectangle r)
		{
			double x, y;
			if (t.X < r.X)
			{ x = r.X - t.X; }
			else if (t.X > r.X + r.Width)
			{ x = t.X - (r.X + r.Width); }
			else
			{ x = 0; }
			if (t.Y < r.Y)
			{ y = r.Y - t.Y; }
			else if (t.Y > r.Y + r.Height)
			{ y = t.Y - (r.Y + r.Height); }
			else
			{ y = 0; }
			return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
		}
	}
}
