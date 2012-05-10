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
        int _now, _last, _ms;
        
        public CompteurFPS(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            FPS = 0.0d;
            _last = DateTime.Now.Millisecond;
            _now = _last + 1;
            _ms = 0;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _now = DateTime.Now.Millisecond;
            _ms = (_now < _last) ? _now - (_last - 1000) : _now - _last;
            FPS = 1000.0d / _ms;
            _last = _now;
            base.Update(gameTime);
        }
    }
}
