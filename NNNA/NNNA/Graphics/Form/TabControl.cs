using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class TabControl : Containing
    {
        #region ATTRIBUTS
        #endregion ATTRIBUTS

        #region GET/SET
        #endregion GET/SET

        public TabControl(TabItem[] children, Rectangle zone, string name)
            : base(children, zone, name) { }

        public override void Update(Souris s)
        {
            //a ajouter
            for (int i = 0; i < Children.Length; i++)
            {
                Children[i].Update(s);
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Draw(sb, sf);
                }
            }
        }
    }
}
