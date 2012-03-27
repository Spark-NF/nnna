using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private bool _borderEffect;
        #endregion ATTRIBUTS

        #region GET/SET
        public string Text
        { get { return _text; } set { _text = value; } }

        public bool BorderEffect
        { get { return _borderEffect; } set { _borderEffect = value; } }
        #endregion GET/SET

        public Button(Rectangle zone, string name, Delegate click)
            : base(zone, name)
        {
            _text = click.Method.Name; 
            _backgroundColor = Color.Gray;
            _click = click;
        }

        public override void Update(Souris s)
        {
            if (s.Clicked(MouseButton.Left) && _zone.Intersects(new Rectangle(s.X,s.Y,1,1)))
            {
                _click();
            }
        }

        public override void Draw(SpriteBatch sb, SpriteFont sf)
        {
            if (_visible)
            {
                sb.Draw(_background, _zone, null, _backgroundColor);
                sb.DrawString(sf, _text, new Vector2(_zone.X, _zone.Y), _textColor);
            }
        }
    }
}
