using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA
{
	class Map
	{
		private int _mapWidth;
		public int MapWidth
		{
			get { return _mapWidth; }
			set { _mapWidth = value; }
		}
		private int _mapHeight;
		public int MapHeight
		{
			get { return _mapHeight; }
			set { _mapHeight = value; }
		}
		private Sprite[,] _map;
		public Sprite[,] Carte
		{
			get { return _map; }
			set { _map = value; }
		}

		public void LoadContent(Sprite[,] map, ContentManager content, Minimap minimap, GraphicsDevice device)
		{
			var textureColor = new Color[(map.GetLength(1) + 4) * (map.GetLength(0) + 4)];
			int x = 0, y = 0, maxX = 0, minX = 0, maxY = 0, i = 0;
			while (i < (map.GetLength(1) * 2) + 8)
			{
				textureColor[i] = Color.Black;
				i++;
			}
			for (int l = 0; l < map.GetLength(1); l++)
			{
				textureColor[i] = Color.Black;
				i++;
				textureColor[i] = Color.Black;
				i++;
				for (int c = 0; c < map.GetLength(0); c++)
				{
					var name = map[l, c].Name;
					switch (name)
					{
						case 'h':
							map[l, c] = new Sprite(content, "Map/herbe3", x, y, true, l, c, name);
							textureColor[i] = new Color(46, 180, 4);
							break;

						case 'i':
							map[l, c] = new Sprite(content, "Map/herbe2", x, y, true, l, c, name);
							textureColor[i] = new Color(136, 188, 10);
							break;

						case 'p':
							map[l, c] = new Sprite(content, "Map/pave", x, y, true, l, c, name);
							textureColor[i] = Color.Gray;
							break;

						case 's':
							map[l, c] = new Sprite(content, "Map/sable2", x, y, true, l, c, name);
							textureColor[i] = new Color(196, 196, 150);
							break;

						case 'e':
							map[l, c] = new Sprite(content, "Map/eau2", x, y, false, l, c, name);
							textureColor[i] = new Color(23, 115, 154);
							break;

						case 't':
							var texture = "eau3";
							if (c < map.GetLength(0) - 1 && c >= 1 && l < map.GetLength(1) - 1 && l >= 1)
							{
								// Entouré de sable
								if (!map[l + 1, c].Liquid && !map[l - 1, c].Liquid && !map[l, c + 1].Liquid && !map[l, c - 1].Liquid)
								{ texture = "flaque"; }
								// En U
								else if (!map[l - 1, c].Liquid && !map[l + 1, c].Liquid && !map[l, c - 1].Liquid)
								{ texture = "sable_U(135)"; }
								else if (!map[l - 1, c].Liquid && !map[l + 1, c].Liquid && !map[l, c + 1].Liquid)
								{ texture = "sable_U(-45)"; }
								else if (!map[l - 1, c].Liquid && !map[l, c + 1].Liquid && !map[l, c - 1].Liquid)
								{ texture = "sable_U(45)"; }
								else if (!map[l + 1, c].Liquid && !map[l, c + 1].Liquid && !map[l, c - 1].Liquid)
								{ texture = "sable_U(-135)"; }
								// Les angles
								else if (!map[l, c + 1].Liquid && !map[l + 1, c].Liquid)
								{ texture = "sable_(-90)"; }
								else if (!map[l, c - 1].Liquid && !map[l - 1, c].Liquid)
								{ texture = "sable_(90)"; }
								else if (!map[l, c - 1].Liquid && !map[l + 1, c].Liquid)
								{ texture = "sable_(180)"; }
								else if (!map[l, c + 1].Liquid && !map[l - 1, c].Liquid)
								{ texture = "sable_(0)"; }
								// Les côtés
								else if (!map[l + 1, c].Liquid)
								{ texture = "sable_(-135)"; }
								else if (!map[l - 1, c].Liquid)
								{ texture = "sable_(45)"; }
								else if (!map[l, c + 1].Liquid)
								{ texture = "sable_(-45)"; }
								else if (!map[l, c - 1].Liquid)
								{ texture = "sable_(135)"; }
								// Les coins
								else if (!map[l - 1, c - 1].Liquid)
								{ texture = "sable_coin(90)"; }
								else if (!map[l + 1, c + 1].Liquid)
								{ texture = "sable_coin(-90)"; }
								else if (!map[l - 1, c + 1].Liquid)
								{ texture = "sable_coin(0)"; }
								else if (!map[l + 1, c - 1].Liquid)
								{ texture = "sable_coin(180)"; }
							}
							map[l, c] = new Sprite(content, "Map/"+texture, x, y, false, l, c, name);
							textureColor[i] = new Color(18, 147, 166);
							break;
					}
					x += map[l, c].Texture.Width / 2;
					y += map[l, c].Texture.Height / 2;
					if ((c + 1) == map.GetLength(1))
					{
						if (x > maxX)
						{ maxX = x + (map[l, c].Texture.Width / 2); }
						if (y > maxY)
						{ maxY = y + (map[l, c].Texture.Height / 2); }

						x -= ((map.GetLength(1) + 1) * (map[l, c].Texture.Width / 2));
						y -= ((map.GetLength(1) - 1) * (map[l, c].Texture.Height / 2));

						if (x < minX)
						{ minX = x + (map[l, c].Texture.Width / 2); }
					}
					i++;
				}
				textureColor[i] = Color.Black;
				i++;
				textureColor[i] = Color.Black;
				i++;
			}
			while (i < textureColor.Length)
			{
				textureColor[i] = Color.Black;
				i++;
			}
			_mapWidth = (maxX - minX) / 64;
			_mapHeight = (maxY) / 32;
			minimap.Texture = new Texture2D(device, map.GetLength(1) + 4, map.GetLength(0) + 4, false, SurfaceFormat.Color);
			minimap.Texture.SetData(textureColor);
			minimap.BaseTexture = minimap.Texture;
			_map = map;
		}
	}
}
