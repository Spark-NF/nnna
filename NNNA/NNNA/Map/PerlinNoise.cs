﻿using System;

namespace NNNA
{
	public class PerlinNoise
	{
		public static int Seed { get; set; }
		public static int Octaves { get; set; }
		public static double Amplitude { get; set; }
		public static double Persistence { get; set; }
		public static double Frequency { get; set; }

		static PerlinNoise()
		{
			var r = new Random();
			Seed = r.Next(Int32.MaxValue);
			Octaves = 8;
			Amplitude = 1;
			Frequency = 0.015;
			Persistence = 0.65;
		}

		public static double Noise(int x, int y)
		{
			//returns -1 to 1
			double total = 0.0;
			double freq = Frequency, amp = Amplitude;
			for (int i = 0; i < Octaves; ++i)
			{
				total = total + Smooth(x * freq, y * freq) * amp;
				freq *= 2;
				amp *= Persistence;
			}
			if (total < -2.4) total = -2.4;
			else if (total > 2.4) total = 2.4;

			return (total/ 2.4);
		}

		public static double NoiseGeneration(int x, int y)
		{
			int n = x + y * 57;
			n = (n << 13) ^ n;

			return (1.0 - ((n * (n * n * 15731 + 789221) + Seed) & 0x7fffffff) / 1073741824.0);
		}

		private static double Interpolate(double x, double y, double a)
		{
			double value = (1 - Math.Cos(a * Math.PI)) * 0.5;
			return x * (1 - value) + y * value;
		}

		private static double Smooth(double x, double y)
		{
			double n1 = NoiseGeneration((int)x, (int)y);
			double n2 = NoiseGeneration((int)x + 1, (int)y);
			double n3 = NoiseGeneration((int)x, (int)y + 1);
			double n4 = NoiseGeneration((int)x + 1, (int)y + 1);

			double i1 = Interpolate(n1, n2, x - (int)x);
			double i2 = Interpolate(n3, n4, x - (int)x);

			return Interpolate(i1, i2, y - (int)y);
		}
	}
}
