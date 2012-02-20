using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            reduced_Map = new Rectangle(x + width/2, y, width*3/4, height*3/4);
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
            spriteBatch.Draw(texture, reduced_Map, null, Color.White, (float)(Math.PI / 4), Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}