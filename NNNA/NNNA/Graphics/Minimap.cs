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
    class Minimap
    {
        private Rectangle reduced_Map;
        public Rectangle Reduced_Map
        {
            get { return reduced_Map; }
            set { reduced_Map = value; }
        }
        private float ratio;
        public float Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Minimap(int x, int y, int width, int height)
        {
            reduced_Map = new Rectangle(x, y, width, height);
        }
        public void LoadContent(Map map)
        {
            float ratio1;
            float ratio2;
            ratio1 = (float)reduced_Map.Width / (float)map.Map_Width;
            ratio2 = (float)reduced_Map.Height / (float)map.Map_Height;
            if (ratio1 >= ratio2)
                ratio = ratio2;
            else ratio = ratio1;
            reduced_Map = new Rectangle(reduced_Map.X, reduced_Map.Y, (int)(map.Map_Width * ratio), (int)(map.Map_Height * ratio));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, reduced_Map, Color.White);
        }
    }
}