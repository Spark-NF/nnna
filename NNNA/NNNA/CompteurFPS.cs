using System;
using Microsoft.Xna.Framework;


namespace NNNA
{
    public class CompteurFPS : GameComponent
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
