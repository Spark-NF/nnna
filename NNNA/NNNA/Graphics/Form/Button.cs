using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    delegate void Delegate();
    class Button : Control
    {
        #region ATTRIBUTS
        private string _text;
        private readonly Delegate _click;
        private readonly InfoBulle _info;
        bool _drawInfoBulle;
        private bool _borderEffect;
        #endregion ATTRIBUTS

        #region GET/SET
        public string Text
        { get { return _text; } set { _text = value; } }

        public bool BorderEffect
        { get { return _borderEffect; } set { _borderEffect = value; } }
        #endregion GET/SET

        public Button(Rectangle zone, string name, Delegate click, bool visible = false, InfoBulle info = null)
            : base(zone, name)
        {
            _drawInfoBulle = false;
            _text = click.Method.Name;
            _click = click;
            _info = info;
            _visible = visible;
        }

        public override void Update(Souris s)
        {
            _drawInfoBulle = false;
            if (_visible && _zone.Intersects(new Rectangle(s.X,s.Y,1,1)))
            {
                if (s.Clicked(MouseButton.Left))
                {
                    _click();
                }
                _drawInfoBulle = true;
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
            }
        }
        
        public void Draw_Info(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                if (_info != null && _drawInfoBulle)
                {
                    _info.Draw(sb, sf);
                }
            }
        }
    }
}
