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


namespace NNNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class CompteurFPS : Microsoft.Xna.Framework.GameComponent
    {
        public double FPS;
        public CompteurFPS(Game game)
            : base(game)
        {
            
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            FPS = 0.0d;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            FPS = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;

            base.Update(gameTime);
        }
    }
}
