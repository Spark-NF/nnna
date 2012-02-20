using System;

namespace NNNA
{
	class IslandGenerator
	{
		static public float[,] Generate(int width, int height)
		{
			float[,] map = new float[width, height], perlin = GeneratePerlinNoise(width, height), mask = Normalize(RollingParticle.Generate(width, height, 5000, 50));
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{ map[x, y] = perlin[x, y] * mask[x, y]; }
			}
			return Normalize(map);
		}

		static private float[,] Normalize(float[,] map, float from = 0, float to = 1, bool removemin = true)
		{
			float[,] n = map;
			float max = Int32.MinValue, min = Int32.MaxValue;
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{
					if (map[x, y] > max)
					{ max = map[x, y]; }
					if (map[x, y] < min)
					{ min = map[x, y]; }
				}
			}
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{ n[x, y] = ((removemin ? map[x, y] - min : map[x, y]) / (removemin ? max - min : max)) * (to - from) + from; }
			}
			return n;
		}

		static private float[,] GeneratePerlinNoise(int width, int height)
		{
			float[,] map = new float[width, height];
			Random r = new Random();
			PerlinNoise.Seed = r.Next();
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{ map[x, y] = (float)PerlinNoise.Noise(x, y); }
			}
			return Normalize(map);
		}

		static private float[,] GenerateRadialMask(int width, int height)
		{
			float[,] map = new float[width, height];
			int centerX = width / 2, centerY = height / 2;
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{ map[x, y] = (float)Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2)); }
			}
			float max = map[0, 0];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{ map[x, y] = 1 - (map[x, y] / max); }
			}
			return map;
		}

		static private float[,] GenerateMask(int width, int height)
		{
			float[,] map = new float[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (x < width / 10 || y < height / 10 || width - x < width / 10 || height - y < height / 10)
					{ map[x, y] = 0.5f; }
					else if (x < width / 5 || y < height / 5 || width - x < width / 5 || height - y < height / 5)
					{ map[x, y] = 0.8f; }
					else
					{ map[x, y] = 1; }
				}
			}
			return map;
		}
	}
}
