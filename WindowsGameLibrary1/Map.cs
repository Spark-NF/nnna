using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Editeur;

namespace WindowsGameLibrary1
{
    class Map
    {
        Tile[][] tiles;
        public Tile[][] Tiles
        {
        get{return tiles;}
        set{tiles = value;}
        }
        public Map()
        { }
        public Map(Vector2 size)
        {
            tiles = new Tile[(int)size.Y][];
            for(int i = 0; i < tiles.Length; i ++)
            tiles[i] = new Tile[(int)size.X];
        }
        public void LoadContent(ContentManager Content)
        {
          for (int y = 0; y < tiles.Length; y++)
          {
              for (int x = 0; x < tiles[0].Length; x++)
              {
                tiles[y][x].LoadContent(Content);
              }
           }
         }
         public void Draw(SpriteBatch spriteBatch)
         {
          for (int y = 0; y < tiles.Length; y++)
          {
            for (int x = 0; x < tiles[0].Length; x++)
            {
                // tiles[y][x].LoadContent(/*Content*/);
            }
           }
         }

    }
}
