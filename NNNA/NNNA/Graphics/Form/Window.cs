using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class Window : Containing
    {
        #region ATTRIBUTS
        private string _title;
        #endregion ATTRIBUTS

        #region GET/SET
        public string Title
        { get { return _title; } set { _title = value; } }
        #endregion GET/SET

        public Window(Control[] children, Rectangle zone, string name)
            : base(children, zone, name)
        {
            _title = name;
        }

        public override void Update(Souris s)
        {
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
                sb.DrawString(sf, _title, new Vector2(_zone.X + _zone.Width / 2 - sf.MeasureString(_title).X / 2, _zone.Y + _zone.Height / 26), _textColor);
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Draw(sb, sf);
                }
            }
        }

    }
}
