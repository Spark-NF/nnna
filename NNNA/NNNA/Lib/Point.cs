using System;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace NNNA
{
	class Point
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }

		// Constructeurs depuis plusieurs types de nombres
		public Point(int x, int y, int z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}
		public Point(float x, float y, float z = 0)
		{
			X = (int)x;
			Y = (int)y;
			Z = (int)z;
		}

		// Calcul de distances
		public double DistanceTo(Point p)
		{ return Math.Sqrt(Math.Pow(p.X - X, 2) + Math.Pow(p.Y - Y, 2) + Math.Pow(p.Z - Z, 2)); }
		public double DistanceTo(Vector2 v, bool useZ = true)
		{ return Math.Sqrt(Math.Pow(v.X - X, 2) + Math.Pow(v.Y - Y, 2) + Math.Pow(useZ ? Z : 0, 2)); }
		public double DistanceTo(Vector3 v)
		{ return Math.Sqrt(Math.Pow(v.X - X, 2) + Math.Pow(v.Y - Y, 2) + Math.Pow(v.Z - Z, 2)); }
		public double DistanceTo(Rectangle r, bool useZ = true)
		{
			int x, y;
			if (X < r.X)
			{ x = r.X - X; }
			else if (X > r.X + r.Width)
			{ x = X - (r.X + r.Width); }
			else
			{ x = 0; }
			if (Y < r.Y)
			{ y = r.Y - Y; }
			else if (Y > r.Y + r.Height)
			{ y = Y - (r.Y + r.Height); }
			else
			{ y = 0; }
			int z = useZ ? Z : 0;
			return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
		}

		// Addition
		public static Point operator +(Point a, Point b)
		{ return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }
		public static Point operator +(Point a, Vector2 b)
		{ return new Point(a.X + b.X, a.Y + b.Y, a.Z); }
		public static Point operator +(Point a, Vector3 b)
		{ return new Point(a.X + b.X, a.Y + b.Y, a.Z + b.Z); }

		// Soustraction
		public static Point operator -(Point a, Point b)
		{ return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }
		public static Point operator -(Point a, Vector2 b)
		{ return new Point(a.X - b.X, a.Y - b.Y, a.Z); }
		public static Point operator -(Point a, Vector3 b)
		{ return new Point(a.X - b.X, a.Y - b.Y, a.Z - b.Z); }

		// Comparaison
		public override bool Equals(Object obj)
		{
			var p = obj as Point;
			if ((object)p == null)
			{ return false; }
			return X == p.X && Y == p.Y && Z == p.Z;
		}
		public bool Equals(Point p)
		{ return X == p.X && Y == p.Y && Z == p.Z; }
		public static bool operator ==(Point a, Point b)
		{
			if (ReferenceEquals(a, b))
			{ return true; }
			if (((object)a == null) || ((object)b == null))
			{ return false; }
			return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
		}
		public static bool operator !=(Point a, Point b)
		{ return !(a == b); }
		public override int GetHashCode()
		{ return X ^ Y ^ Z; }

		// Conversion
		public static implicit operator Point(Vector2 v)
		{ return new Point(v.X, v.Y); }
		public static implicit operator Vector2(Point p)
		{ return new Vector2(p.X, p.Y); }
		public static implicit operator Point(Vector3 v)
		{ return new Point(v.X, v.Y, v.Z); }
		public static implicit operator Vector3(Point p)
		{ return new Vector3(p.X, p.Y, p.Z); }

		// ToString()
		public override string ToString()
		{ return "(" + X.ToString(CultureInfo.CurrentCulture) + "; " + Y.ToString(CultureInfo.CurrentCulture) + "; " + Z.ToString(CultureInfo.CurrentCulture) + ")"; }
	}
}
