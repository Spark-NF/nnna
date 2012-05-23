using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class Info_Bulle : Control
    {
        public Info_Bulle(Rectangle zone, string name)
            : base(zone, name)
        {  }

        public override void Update(Souris s) { }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            sb.Draw(_background, _zone, null, _backgroundColor);
        }
    }
}