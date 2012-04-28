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
    class Editeur : Tile
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        KeyboardState lastKeyboardState;
        Map map;
        Cursor cursor;
        Vector2 mapSize = new Vector2(5, 5);
    /*    public Editeur()
        {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferHeight = 160;
        graphics.PreferredBackBufferWidth = 160;
        }
        protected override void Initialize()
        {
        base.Initialize();
        }
        protected override void LoadContent()
        {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
    protected override void Update(GameTime gameTime)
    {
        lastKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();
        if(keyboardState.IsKeyDown(Keys.C) && lastKeyboardState.IsKeyUp(Keys.C))
        {
            map = new Map();
            map = new Map(mapSize);
        }
        for (int y = 0; y < map.Tiles.Length; y++)
        {
            for (int x = 0; x < map.Tiles[0].Length; x++)
            {
                map.Tiles[y][x] = new Tile("grass", new Vector2(x, y));
            }
        }
        map.LoadContent(Content);
        cursor = new Cursor();
        cursor.LoadContent(Content);
    }
        else 
        if (keyboardState.IsKeyDown(Keys.S) && lastKeyboardState.IsKeyUp(Keys.S) && map != null)
            {
                
            }
        else
             if (keyboardState.IsKeyDown(Keys.L) && lastKeyboardState.IsKeyUp(Keys.L))
                {
                }
        if (cursor != null && map != null)
            {
                cursor.Update(gameTime, mapSize);
                if (keyboardState.IsKeyDown(Keys.F1) && lastKeyboardState.IsKeyUp(Keys.F1))
                {
                map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].AssetName = "grass";
                map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].LoadContent(Content);
                }
                else 
                    if (keyboardState.IsKeyDown(Keys.F2) && lastKeyboardState.IsKeyUp(Keys.F2))
                    {
                    map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].AssetName = "tree";
                    map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].LoadContent(Content);
                    }
                    else 
                        if (keyboardState.IsKeyDown(Keys.F3) && lastKeyboardState.IsKeyUp(Keys.F3))
                            {
                            map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].AssetName = "sand";
                            map.Tiles[(int)cursor.Position.Y][(int)cursor.Position.X].LoadContent(Content);
                            }
            }
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            if (map != null)
            map.Draw(spriteBatch);
            if (cursor != null)
            cursor.Draw(spriteBatch);
        } */
    }
}
