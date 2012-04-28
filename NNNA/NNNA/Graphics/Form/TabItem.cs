using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    class TabItem : Containing
    {
        #region ATTRIBUTS
        private bool selected;
        #endregion ATTRIBUTS

        #region GET/SET
        public bool Selected
        { get { return selected; } set { selected = value; } }
        #endregion GET/SET

        public TabItem(Control[] children, Rectangle zone, string name)
            : base(children, zone, name) 
        {
            selected = false;
        }

        public bool Select(Souris s)
        {
            if (_visible && s.Clicked(MouseButton.Left) && _zone.Intersects(new Rectangle(s.X, s.Y, 1, 1)))
                return true;
            else return false;
        }

        public override void Update(Souris s)
        {
            if (selected)
            {
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].Update(s);
                }
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, selected ? _backgroundColor : Color.Gray);
                sb.DrawString(sf, Name, new Vector2(_zone.X, _zone.Y) + new Vector2(Zone.Width / 2, Zone.Height / 2) - sf.MeasureString(Name) / 2, selected ? Color.Black : _textColor);
                if (selected)
                {
                    for (int i = 0; i < Children.Length; i++)
                    {
                        Children[i].Draw(sb, sf);
                    }
                }
            }
        }
    }
}
