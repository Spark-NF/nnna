using System;
using System.Collections.Generic;
using System.Linq;

namespace NNNA
{
	class RollingParticle
	{
		private static float[,] _tiles;

		public static float[,] Generate(int width, int height, int iterations = 3000, int length = 50, bool centerBias = true, int edgeBias = 12, float outerBlur = 0.75f, float innerBlur = 0.88f)
		{
			var r = new Random();
			_tiles = new float[width, height];

			for (int j = 0; j < iterations; j++)
			{
				// Start nearer the center
				int sourceX, sourceY;
				if (centerBias)
				{
					sourceX = (int)(r.NextDouble() * (width - (edgeBias * 2)) + edgeBias);
					sourceY = (int)(r.NextDouble() * (height - (edgeBias * 2)) + edgeBias);
				}
				// Random starting location
				else
				{
					sourceX = (int)(r.NextDouble() * (width - 1));
					sourceY = (int)(r.NextDouble() * (height - 1));
				}

				for (int l = 0; l < length; l++)
				{
					sourceX += (int)(r.NextDouble() * 2.0 - 1.0);
					sourceY += (int)(r.NextDouble() * 2.0 - 1.0);

					if (sourceX < 1 || sourceX > width - 2 || sourceY < 1 || sourceY > height - 2)
					{ break; }

					List<Point> hood = GetNeighborhood(sourceX, sourceY);

					for (int i = 0; i < hood.Count(); i++)
					{
						if (_tiles[hood[i].X, hood[i].Y] < _tiles[sourceX, sourceY])
						{
							sourceX = hood[i].X;
							sourceY = hood[i].Y;
							break;
						}
					}

					_tiles[sourceX, sourceY]++;
				}
			}
			
			if (centerBias)
			{ BlurEdges(outerBlur, innerBlur); }

			return _tiles;
		}
		
		/// <summary>
		/// Get the Moore neighborhood (3x3, 8 surrounding tiles, minus the center tile).
		/// </summary>
		/// <param name="x">The x position of the center of the neighborhood.</param>
		/// <param name="y">The y position of the center of the neighborhood.</param>
		/// <returns>An array of neighbor Points, shuffled.</returns>
		private static List<Point> GetNeighborhood(int x, int y)
		{
			var result = new List<Point>();
			
			for (int a = -1; a <= 1; a++)
			{
				for (int b = -1; b <= 1; b++)
				{
					if (a != 0 || b != 0)
					{
						if (x + a >= 0 && x + a < _tiles.GetLength(0) && y + b >= 0 && y + b < _tiles.GetLength(1))
						{
							result.Add(new Point(x + a, y + b));
						}
					}
				}
			}
			
			// Return the neighborhood in no particular order
			var r = new Random();
			for (int i = 1; i < result.Count(); i++)
			{
				int pos = r.Next(i + 1);
				Point p = result[i];
				result[i] = result[pos];
				result[pos] = p;
			}

			return result;
		}
		
		/// <summary>
		/// "Blur" the edges of the tile array to ensure no hard edges.
		/// </summary>
		/// <param name="outerBlur">Outer blur density.</param>
		/// <param name="innerBlur">Inner blur density.</param>
		private static void BlurEdges(float outerBlur, float innerBlur)
		{
			for (int ix = 0; ix < _tiles.GetLength(0); ix++)
			{
				for (int iy = 0; iy < _tiles.GetLength(1); iy++)
				{
					// Multiply the outer edge and the second outer edge by some constants to ensure the world does not touch the edges.
					if (ix == 0 || ix == _tiles.GetLength(0) - 1 || iy == 0 || iy == _tiles.GetLength(1) - 1) _tiles[ix, iy] *= outerBlur;
					else if (ix == 1 || ix == _tiles.GetLength(0) - 2 || iy == 1 || iy == _tiles.GetLength(1) - 2) _tiles[ix, iy] *= innerBlur;
				}
			}
		}
	}
}
