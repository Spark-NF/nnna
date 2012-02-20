using System;
using System.Collections.Generic;
using System.Linq;

namespace NNNA
{
	class RollingParticle
	{
		private static float[,] tiles;

		/**
		 * Constructor.
		 * Generate a rolling particle map, blur edges, and normalize.
		 */
		public static float[,] Generate(int width, int height, int iterations = 3000, int length = 50, bool centerBias = true, int edgeBias = 12, float outerBlur = 0.75f, float innerBlur = 0.88f)
		{
			Random r = new Random();
			tiles = new float[width, height];

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

					List<Point> hood = getNeighborhood(sourceX, sourceY);

					for (int i = 0; i < hood.Count(); i++)
					{
						if (tiles[hood[i].X, hood[i].Y] < tiles[sourceX, sourceY])
						{
							sourceX = hood[i].X;
							sourceY = hood[i].Y;
							break;
						}
					}

					tiles[sourceX, sourceY]++;
				}
			}
			
			if (centerBias)
			{
				blurEdges(outerBlur, innerBlur);
			}

			return tiles;
		}
		
		
		/**
		 * Get the Moore neighborhood (3x3, 8 surrounding tiles, minus the center tile).
		 * @param	x	The x position of the center of the neighborhood.
		 * @param	y	The y position of the center of the neighborhood.
		 * @return	An array of neighbor Points, shuffled.
		 */
		private static List<Point> getNeighborhood(int x, int y)
		{
			List<Point> result = new List<Point>();
			
			for (int a = -1; a <= 1; a++)
			{
				for (int b = -1; b <= 1; b++)
				{
					if (a != 0 || b != 0)
					{
						if (x + a >= 0 && x + a < tiles.GetLength(0) && y + b >= 0 && y + b < tiles.GetLength(1))
						{
							result.Add(new Point(x + a, y + b));
						}
					}
				}
			}
			
			// Return the neighborhood in no particular order
			Random r = new Random();
			for (int i = 1; i < result.Count(); i++)
			{
				int pos = r.Next(i + 1);
				Point p = result[i];
				result[i] = result[pos];
				result[pos] = p;
			}

						
			return result;
		}
		
		
		/**
		 * "Blur" the edges of the tile array to ensure no hard edges.
		 */
		private static void blurEdges(float outer_blur, float inner_blur)
		{
			for (int ix = 0; ix < tiles.GetLength(0); ix++)
			{
				for (int iy = 0; iy < tiles.GetLength(1); iy++)
				{
					// Multiply the outer edge and the second outer edge by some constants to ensure the world does not touch the edges.
					if (ix == 0 || ix == tiles.GetLength(0) - 1 || iy == 0 || iy == tiles.GetLength(1) - 1) tiles[ix, iy] *= outer_blur;
					else if (ix == 1 || ix == tiles.GetLength(0) - 2 || iy == 1 || iy == tiles.GetLength(1) - 2) tiles[ix, iy] *= inner_blur;
				}
			}
		}
	}
}
