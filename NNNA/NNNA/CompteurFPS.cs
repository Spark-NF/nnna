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
using System.Threading;


namespace NNNA
{
    public class CompteurFPS : Microsoft.Xna.Framework.GameComponent
    {
        public double FPS;
        System.Timers.Timer time;
        public CompteurFPS(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            FPS = 0.0d;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            FPS = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
        }
    }
}
