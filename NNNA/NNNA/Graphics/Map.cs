using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace NNNA
{
    class Map
    {
        private int map_Width;
        public int Map_Width
        {
            get { return map_Width; }
            set { map_Width = value; }
        }
        private int map_Height;
        public int Map_Height
        {
            get { return map_Height; }
            set { map_Height = value; }
        }
        public Map()
        {
        }
        public void LoadContent(Sprite[,] map, ContentManager content, Minimap minimap, GraphicsDevice device)
        {
            Color[] texture_Color = new Color[(map.GetLength(1) + 4) * (map.GetLength(0) + 4)];
            int x = 0;
            int y = 0;
            int max_X = 0;
            int min_X = 0;
            int max_Y = 0;
            int i = 0;
            while (i < (map.GetLength(1) * 2) + 8)
            {
                texture_Color[i] = Color.Black;
                i++;
            }
            for (int l = 0; l < map.GetLength(0); l++)
            {
                texture_Color[i] = Color.Black;
                i++;
                texture_Color[i] = Color.Black;
                i++;
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    if ((map[l, c]).Name == 'h')
                    {
                        (map[l, c]) = new Sprite(content, "Map/herbe3", x, y);
                        texture_Color[i] = new Color(46, 180, 4);
                    }
                    else if ((map[l, c]).Name == 'i')
                    {
                        (map[l, c]) = new Sprite(content, "Map/herbe2", x, y);
                        texture_Color[i] = new Color(136, 188, 10);
                    }
                    else if ((map[l, c]).Name == 'p')
                    {
                        (map[l, c]) = new Sprite(content, "Map/pave", x, y);
                        texture_Color[i] = Color.Gray;
                    }
                    else if ((map[l, c]).Name == 's')
                    {
                        (map[l, c]) = new Sprite(content, "Map/sable2", x, y);
                        texture_Color[i] = new Color(196, 196, 150);
                    }
                    else if ((map[l, c]).Name == 'e')
                    {
						(map[l, c]) = new Sprite(content, "Map/eau2", x, y, false);
						texture_Color[i] = new Color(23, 115, 154);
                    }
                    else if ((map[l, c]).Name == 't')
                    {
                        if ((c + 1) < map.GetLength(1) && (c - 1) >= 0 && (l + 1) < map.GetLength(0) && (l - 1) >= 0)
                        {

                            if ((map[l, c + 1]).Name == 's' && (map[l + 1, c]).Name == 's' && (map[l, c - 1]).Name != 's' && (map[l - 1, c]).Name != 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(-90)", x, y);
                            else if ((map[l, c - 1]).Name == 's' && (map[l - 1, c]).Name == 's' && (map[l, c + 1]).Name != 's' && (map[l + 1, c]).Name != 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(90)", x, y);
                            else if ((map[l, c - 1]).Name == 's' && (map[l + 1, c]).Name == 's' && (map[l, c + 1]).Name != 's' && (map[l - 1, c]).Name != 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(180)", x, y);
                            else if ((map[l, c + 1]).Name == 's' && (map[l - 1, c]).Name == 's' && (map[l, c - 1]).Name != 's' && (map[l + 1, c]).Name != 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(0)", x, y);
                            else if ((map[l - 1, c]).Name == 's' && (map[l + 1, c]).Name == 's' && (map[l, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_U(45)", x, y);
                            else if ((map[l - 1, c]).Name == 's' && (map[l + 1, c]).Name == 's' && (map[l, c + 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_U(-45)", x, y);
                            else if ((map[l - 1, c]).Name == 's' && (map[l, c + 1]).Name == 's' && (map[l, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_U(135)", x, y);
                            else if ((map[l + 1, c]).Name == 's' && (map[l, c + 1]).Name == 's' && (map[l, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_U(-135)", x, y);
                            //else if (((map[l, c + 1]).Name != 't' && (map[l, c + 1]).Name != 'e') && ((map[l, c - 1]).Name != 't' && (map[l, c - 1]).Name != 'e') && ((map[l + 1, c]).Name != 't' && (map[l + 1, c]).Name != 'e') && ((map[l - 1, c]).Name != 't' && (map[l - 1, c]).Name != 'e'))
                            //(map[l, c]) = new Sprite(content, "Map/flaque", x, y);
                            else if ((map[l + 1, c]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(-135)", x, y);
                            else if ((map[l - 1, c]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(45)", x, y);
                            else if ((map[l, c + 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(-45)", x, y);
                            else if ((map[l, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_(135)", x, y);
                            else if ((map[l - 1, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_coin(90)", x, y);
                            else if ((map[l + 1, c + 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_coin(-90)", x, y);
                            else if ((map[l - 1, c + 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_coin(0)", x, y);
                            else if ((map[l + 1, c - 1]).Name == 's')
                                (map[l, c]) = new Sprite(content, "Map/sable_coin(180)", x, y);
                            else
                                (map[l, c]) = new Sprite(content, "Map/eau3", x, y, false);
                        }
                        else
                            (map[l, c]) = new Sprite(content, "Map/eau3", x, y, false);

                        texture_Color[i] = new Color(18, 147, 166);
                    }
                    x += (map[l, c]).Texture.Width / 2;
                    y += (map[l, c]).Texture.Height / 2;
                    if ((c + 1) == map.GetLength(1))
                    {
                        if (x > max_X)
                            max_X = x + ((map[l, c]).Texture.Width / 2);
                        if (y > max_Y)
                            max_Y = y + ((map[l, c]).Texture.Height / 2);

                        x -= ((map.GetLength(1) + 1) * ((map[l, c]).Texture.Width / 2));
                        y -= ((map.GetLength(1) - 1) * ((map[l, c]).Texture.Height / 2));

                        if (x < min_X)
                            min_X = x + ((map[l, c]).Texture.Width / 2);
                    }
                    i++;
                }
                texture_Color[i] = Color.Black;
                i++;
                texture_Color[i] = Color.Black;
                i++;
            }
            while (i < texture_Color.Length)
            {
                texture_Color[i] = Color.Black;
                i++;
            }
            map_Width = (max_X - min_X) / 64;
            map_Height = (max_Y) / 32;
            minimap.Texture = new Texture2D(device, map.GetLength(1) + 4, map.GetLength(0) + 4, false, SurfaceFormat.Color);
            minimap.Texture.SetData(texture_Color);
        }
    }
}
