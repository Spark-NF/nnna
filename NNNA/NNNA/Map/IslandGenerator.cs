using System;

namespace NNNA
{
	class IslandGenerator
	{
		static public float[,] Generate(int width, int height)
		{
			float[,] map = new float[width, height], perlin = GeneratePerlinNoise(width, height), mask = Normalize(RollingParticle.Generate(width, height, 5000));
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
			var map = new float[width, height];
			var r = new Random();
			PerlinNoise.Seed = r.Next();
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{ map[x, y] = (float)PerlinNoise.Noise(x, y); }
			}
			return Normalize(map);
		}
	}
}
