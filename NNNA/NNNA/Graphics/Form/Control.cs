using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NNNA;

namespace NNNA.Form
{
    abstract class Control
    {
        #region ATTRIBUTS
        private Control[] _children;
        private string _name;
        protected Color _textColor;
        protected Rectangle _zone;
        protected bool _visible;
        protected Color _backgroundColor;
        protected Texture2D _background;
        #endregion ATTRIBUTS

        #region GET/SET
        public Control[] Children
        { get { return _children; } set { _children = value; } }

        protected string Name
        { get { return _name; } set { _name = value; } }

        public Color TextColor
        { get { return _textColor; } set { _textColor = value; } }

        public Rectangle Zone
        { get { return _zone; } set { _zone = value; } }

        public bool Visible
        { get { return _visible; } set { _visible = value; } }

        public Color BackgroundColor
        { get { return _backgroundColor; } set { _backgroundColor = value; } }

        public Texture2D Background
        { get { return _background; } set { _background = value; } }
        #endregion GET/SET

        public Control(Control[] children, Rectangle zone, string name)
        {
            _children = children;
            _zone = zone;
            _name = name;
            _textColor = Color.Silver;
            _visible = false;
            _backgroundColor = Color.White;
            _background = new Texture2D(Game1._graphics.GraphicsDevice, 1, 1);
            _background.SetData(new Color[] { _backgroundColor });
        }

        public virtual void Update(Souris s) { }

        public virtual void Draw(SpriteBatch sb, SpriteFont sf) { }
    }
}
