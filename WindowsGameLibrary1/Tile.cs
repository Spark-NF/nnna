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

namespace Editeur
{
    public class Tile
    {
        Texture2D texture;
        string assetName;
        public string AssetName
        {
           get{return assetName;}
           set{assetName = value;}
        }
        Vector2 position;
        public Vector2 Position
        {
            get{return position;}
            set{position = value;}
        }
    
        public Tile()
        { }
        public Tile(string assetName, Vector2 position)
        {
          this.assetName = assetName;
          this.position = position;
        }
        public void LoadContent(ContentManager Content)
        {
          texture = Content.Load<Texture2D>(assetName);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
          spriteBatch.Draw(texture, new Vector2(position.X * texture.Width, position.Y * texture.Height), Color.White);
        }
     }
}

