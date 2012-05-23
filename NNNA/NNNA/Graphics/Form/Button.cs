using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    delegate void Delegate();
    class Button : Control
    {
        #region ATTRIBUTS
        private string _text;
        Delegate _click;
        Info_Bulle _info;
        bool _draw_info_bulle;
        private bool _borderEffect;
        #endregion ATTRIBUTS

        #region GET/SET
        public string Text
        { get { return _text; } set { _text = value; } }

        public bool BorderEffect
        { get { return _borderEffect; } set { _borderEffect = value; } }
        #endregion GET/SET

        public Button(Rectangle zone, string name, Delegate click, bool visible = false, Info_Bulle info = null)
            : base(zone, name)
        {
            _draw_info_bulle = false;
            _text = click.Method.Name;
            _click = click;
            _info = info;
            _visible = visible;
        }

        public override void Update(Souris s)
        {
            _draw_info_bulle = false;
            if (_visible && _zone.Intersects(new Rectangle(s.X,s.Y,1,1)))
            {
                if (s.Clicked(MouseButton.Left))
                {
                    _click();
                }
                _draw_info_bulle = true;
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
                if (_info != null && _draw_info_bulle)
                {
                    _info.Draw(sb, sf);
                }
            }
        }
    }
}
