using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class TabItem : Containing
    {
        #region ATTRIBUTS
        #endregion ATTRIBUTS

        #region GET/SET
        #endregion GET/SET

        public TabItem(Control[] children, Rectangle zone, string name)
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
                sb.DrawString(sf, Name, new Vector2(_zone.X, _zone.Y), _textColor);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Draw(sb, sf);
                }
            }
        }
    }
}
