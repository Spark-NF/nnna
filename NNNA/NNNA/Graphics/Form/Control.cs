using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NNNA.Form
{
    abstract class Control
    {
        #region ATTRIBUTS
        private string _name;
        protected Color _textColor;
        protected Rectangle _zone;
        protected bool _visible;
        protected Color _backgroundColor;
        protected Texture2D _background;
        #endregion ATTRIBUTS

        #region GET/SET
        public string Name
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

        public Control(Rectangle zone, string name)
        {
            _zone = zone;
            _name = name;
            _textColor = Color.Silver;
            _visible = false;
            _backgroundColor = Color.White;
            _background = new Texture2D(Game1.Graphics.GraphicsDevice, 1, 1);
            _background.SetData(new Color[] { _backgroundColor });
        }

        public abstract void Update(Souris s);

        public abstract void Draw(SpriteBatch sb, SpriteFont sf);
    }
}
