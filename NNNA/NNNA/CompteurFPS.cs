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
    public class CompteurFPS : Microsoft.Xna.Framework.GameComponent
    {
        public double FPS;
        ulong _last;
        
        public CompteurFPS(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            FPS = 0.0d;
            _last = ConvertToTotalMilliseconds(DateTime.Now);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ulong now = ConvertToTotalMilliseconds(DateTime.Now);
            FPS = 1000.0d / (now - _last);
            _last = now;
            base.Update(gameTime);
        }

        private static ulong ConvertToTotalMilliseconds(DateTime time)
        {
            return ((ulong)time.Minute * 60000 + (ulong)time.Second * 1000 + (ulong)time.Millisecond);
        }
    }
}
